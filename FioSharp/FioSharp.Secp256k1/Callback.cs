using System;

namespace FioSharp.Secp256k1
{
    public class Callback : EventArgs
    {
        public Callback()
        {
        }

        public Callback(string message)
        {
            Message = message;
        }

        public string Message;
    }
}