using FioSharp.Core;

namespace FioSharp.Unity3D
{
    /// <summary>
    /// Fio client wrapper using general purpose HttpHandler
    /// </summary>
    public class Fio : FioBase
    {
        /// <summary>
        /// EOSIO Client wrapper constructor.
        /// </summary>
        /// <param name="config">Configures client parameters</param>
        public Fio(FioConfigurator configuratior) : 
            base(configuratior, new HttpHandler())
        {
        }
    }
}
