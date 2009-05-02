using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaetzel.TwitterDA
{
    public static class Config
    {
        private static string _username;
        private static string _password;
        private static int _maxMessageLength = 140;

        public static int MaxMessageLength
        {
            get
            {
                return _maxMessageLength;
            }
        }

        internal static string Username
        {
            get
            {
                return _username;
            }
        }

        internal static string Password
        {
            get
            {
                return _password;
            }
        }

        public static void SetConfigurations(string username, string password)
        {
            _username = username;
            _password = password;
        }




    }
}
