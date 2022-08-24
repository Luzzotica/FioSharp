using System;
using System.Text.RegularExpressions;

namespace FioSharp.Util
{
    public class RegexExpressions
    {
        public const string FIO_ADDRESS = "^(?:(?=.{3,64}$)[a-zA-Z0-9]{1}(?:(?:(?!-{2,}))[a-zA-Z0-9-]*[a-zA-Z0-9]+){0,1}@[a-zA-Z0-9]{1}(?:(?:(?!-{2,}))[a-zA-Z0-9-]*[a-zA-Z0-9]+){0,1}$)";
        public const string FIO_DOMAIN = "^[a-z0-9\\-]{1,62}$";
        public const string FIO_PUBLIC_KEY = "^FIO\\w{1,62}$";
        public const string CHAIN_TOKEN_CODE = "^[A-Za-z0-9]{1,10}$";
        public const string NATIVE_BLOCKCHAIN_ADDRESS = "^\\w{1,128}$";
    }

    public class Validation
    {
        public static bool Validate(string source, string regex)
        {
            Console.WriteLine(source);
            return Regex.IsMatch(source, regex);
        }
    }
}
