using FioSharp.Core;
using FioSharp.Core.Interfaces;

namespace FioSharp
{
    /// <summary>
    /// EOSIO client wrapper using general purpose HttpHandler
    /// </summary>
    public class Fio : FioBase
    {
        /// <summary>
        /// EOSIO Client wrapper constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        public Fio(FioConfigurator config) : 
            base(config, new HttpHandler())
        {
        }

        public Fio(FioConfigurator config, IHttpHandler handler) :
            base(config, handler)
        {
        }

    }
}
