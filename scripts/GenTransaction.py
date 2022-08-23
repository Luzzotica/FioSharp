from io import TextIOWrapper
from sqlite3 import paramstyle
import string
import yaml
from re import sub

cached_endpoints = ['get_abi', 'get_raw_abi']
ignored_endpoints = ['transaction', 'packed_transaction']

LIST_TYPE = 'array'
YAML_TYPE_TO_CS = {
  'integer': 'int',
  'boolean': 'bool',
  'array': 'List'
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


def get_parameters(info: dict):
  '''Returns each parameter and its type in a json dictionary'''
  if 'parameters' in info and len(info['parameters']) > 0:
    return info['parameters'][0]['schema']['properties']
  return None


def build_param(name: string, info: dict):
  p_name_camel = to_camel_case(name)
  t = get_cs_type(info['type'])
  # If the type is an array, try to get that object's type
  if info['type'] == LIST_TYPE:
    t = f'{t}<object>'
  return f'{t} {p_name_camel}'


def build_object_variables(params: dict):
  '''
  Builds the parameters into the public variables of an object
  Example:
    public string actor;
    public string payeePublicKey;
    public string payerPublicKey;
    ...
  '''
  var_list = []
  for p_name, p_info in params.items():
    var = build_param(p_name, p_info)
    var_list.append(f'public {var};')
  return '\n        '.join(var_list)


def build_object_assignments(params: dict):
  '''
  Builds the code to assign each variable in a constructor.
  Example:
    this.actor = actor;
    this.payeePublicKey = payeePublicKey;
    this.payerPublicKey = payerPublicKey;
    ...
  '''
  var_list = []
  for p_name in params.keys():
    p_name_camel = to_camel_case(p_name)
    var_list.append(f'this.{p_name_camel} = {p_name_camel};')
  return '\n            '.join(var_list)


def build_function_params(params: dict):
  '''
  Builds the parameters of the function as a string and returns it.
  Example: "string fioPubKey, int limit, int offset"
  '''
  param_list = []
  for p_name, p_info in params.items():
    var = build_param(p_name, p_info)
    param_list.append(var)
  return ', '.join(param_list)


def build_dict_params(params: dict):
  '''
  Builds the parameters into a dictionary structure as a string.
  Example: '{ "fio_pub_key", fioPubKey }, { "limit", limit }, { "offset", offset }'
  '''
  param_list = []
  for p_name, p_info in params.items():
    p_name_camel = to_camel_case(p_name)
    param_list.append(f'{{ "{p_name}", {p_name_camel} }}')
  return ', '.join(param_list)


def create_transaction_object(f: TextIOWrapper, obj_name: string, params: dict):
  '''
  Creates the object for the transaction with the given name for the
  provided parameters of that object.
  '''
  # lang_obj_name = to_upper_camel_case(obj_name) if params != None else ''
  object_vars = build_object_variables(params) if params != None else ''
  object_assignments = build_object_assignments(params) if params != None else ''
  param_list = build_function_params(params) if params != None else ''
  dict_assignments = build_dict_params(params) if params != None else ''
  print(params)

  f.write(f'''  
    public class {obj_name} : ITransactionData 
    {{
        {object_vars}

        public {obj_name}({param_list})
        {{
            {object_assignments}
        }}

        public Dictionary<string, object> ToJsonObject() 
        {{
            return new Dictionary<string, object>() {{
                {dict_assignments}
            }};
        }}
    }}
        ''')


api_actions = []
with open('fio-api-actions.yaml') as y:
  api_actions = yaml.safe_load(y)['paths']

with open('FioTransactions.cs', 'w') as f:
  f.write('''using FioSharp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
  
namespace FioSharp.Core.Api.v1
{
    public interface ITransactionData
    {
      Dictionary<string, object> ToJsonObject();
    }
''')

  
  for path, info in api_actions.items():
    # Skip endpoints that aren't HTTP specific
    if (path.startswith('/') or path in ignored_endpoints):
      continue
    endpoint = path
    print(endpoint)
    print()
    create_transaction_object(f, endpoint, get_parameters(info['options']))
  
  f.write('''
}
''')
