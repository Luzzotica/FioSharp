using EosSharp.Core;

namespace EosSharp
{
    /// <summary>
    /// EOSIO client wrapper using general purpose HttpHandler
    /// </summary>
    public class Eos : FioBase
    {
        /// <summary>
        /// EOSIO Client wrapper constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        public Eos(EosConfigurator config) : 
            base(config, new HttpHandler())
        {
        }
    }
}
