import yaml
from re import sub

cached_endpoints = ['get_abi', 'get_raw_abi']
ignored_endpoints = ['/fio_api_endpoint', '/push_transaction']

YAML_TYPE_TO_CS = {
  'integer': 'int',
  'boolean': 'bool'
}
def get_cs_type(t):
  if t in YAML_TYPE_TO_CS:
    return YAML_TYPE_TO_CS[t]
  return t


def to_upper_camel_case(s):
  return sub(r"(_|-)+", " ", s).title().replace(" ", "")


def to_camel_case(s):
  upper = to_upper_camel_case(s)
  return ''.join([upper[0].lower(), upper[1:]])


def get_endpoint_return_type(endpoint):
  '''Expects an enpoint without a slash at the beginning'''
  endpoint_camel = to_upper_camel_case(endpoint)
  return endpoint_camel + "Response"


def get_parameters(post_info):
  '''Returns each parameter and its type in a json dictionary'''
  if 'parameters' in post_info:
    return post_info['parameters'][0]['schema']['properties']
  return {}


def build_function_params(params):
  '''
  Builds the parameters of the function as a string and returns it.
  Example: "string fioPubKey, int limit, int offset"
  '''
  param_list = []
  for p_name, p_info in params.items():
    p_name_camel = to_camel_case(p_name)
    t = get_cs_type(p_info['type'])
    param_list.append(f'{t} {p_name_camel}')
  return ', '.join(param_list)


def build_dict_params(params):
  '''
  Builds the parameters into a dictionary structure as a string.
  Example: '{ "fio_pub_key", fioPubKey }, { "limit", limit }, { "offset", offset }'
  '''
  param_list = []
  for p_name, p_info in params.items():
    p_name_camel = to_camel_case(p_name)
    param_list.append(f'{{ "{p_name}", {p_name_camel} }}')
  return ', '.join(param_list)


def create_endpoint(f, endpoint, post_info):
  endpoint_function = to_upper_camel_case(endpoint)
  params = get_parameters(post_info)
  endpoint_return_type = get_endpoint_return_type(endpoint)
  param_list = build_function_params(params)
  param_dict = build_dict_params(params)
  print(endpoint_function)
  print(params)

  is_post = len(params) != 0 
  request_type = 'PostJson' if is_post else 'GetJson'
  cached_var = ''
  cached_reload = ''
  if endpoint in cached_endpoints:
    request_type = 'PostJsonWithCache'
    cached_var = ', bool reload = true'
    cached_reload = ', reload: reload'

  # If we are posting, then we need to include the parameters, otherwise endpoint is all that is needed
  f.write(f'''        
        public async Task<{endpoint_return_type}> {endpoint_function}({param_list}{cached_var}) 
        {{''')
  to_write = ''
  if is_post:
    f.write(f'''
          return await {request_type}<{endpoint_return_type}>("{endpoint}",
            new Dictionary<string, dynamic>()
            {{
                {param_dict}
            }}{cached_reload});''')
  else:
    f.write(f'''
          return await {request_type}<{endpoint_return_type}>("{endpoint}");''')
  
  f.write('\n        }')


api_actions = []
with open('fio-api-actions.yaml') as y:
  api_actions = yaml.safe_load(y)['paths']

with open('FioApi.cs', 'w') as f:
  f.write('''using FioSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
  
namespace FioSharp.Core.Api.v1
{
	/// <summary>
    /// EosApi defines api methods to interface with a http handler
    /// </summary>
    public class FioApi
    {
        public FioConfigurator Config { get; set; }
        public IHttpHandler HttpHandler { get; set; }

		    /// <summary>
        /// Eos Client api constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        /// <param name="httpHandler">Http handler implementation</param>
        public FioApi(FioConfigurator config, IHttpHandler httpHandler)
        {
           Config = config;
           HttpHandler = httpHandler;
        }

        public async Task<TResponseData> GetJson<TResponseData>(string endpoint)
        {
            var url = string.Format("{0}/v1/chain/{1}", Config.HttpEndpoint, endpoint);
            return await HttpHandler.GetJsonAsync<TResponseData>(url);
        }
        public async Task<TResponseData> PostJson<TResponseData>(string endpoint, Dictionary<string, dynamic> data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/{1}", Config.HttpEndpoint, endpoint);
            return await HttpHandler.PostJsonAsync<TResponseData>(url, data);
        }
        public async Task<TResponseData> PostJsonWithCache<TResponseData>(string endpoint, Dictionary<string, dynamic> data, bool reload = false)
        {
            var url = string.Format("{0}/v1/chain/{1}", Config.HttpEndpoint, endpoint);
            return await HttpHandler.PostJsonWithCacheAsync<TResponseData>(url, data, reload);
        }

        public async Task<PushTransactionResponse> PushTransaction(string[] signatures,
            string packedTrx)
        {
            return await PostJson<PushTransactionResponse>("push_transaction",
              new Dictionary<string, dynamic>()
              {
                { "signatures", signatures },
                { "compression", 0 },
                { "packed_context_free_data", "" },
                { "packed_trx", packedTrx }
              });
        }
''')

  
  for path, info in api_actions.items():
    # Skip endpoints that aren't HTTP specific
    if (not path.startswith('/') or path in ignored_endpoints):
      continue
    endpoint = path[1:]
    create_endpoint(f, endpoint, info['post'])
  
  f.write('''
    }
}
''')
