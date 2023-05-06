using System.Collections.Generic;

namespace PLANTILLA.Helpers
{
    public static class SessionHelper
    {
        private static IDictionary<string, string> Session { get; set; } = new Dictionary<string, string>();

        public static void setString(string key, string value)
        {
            Session[key] = value;
        }

        public static string getString(string key)
        {
            return Session.ContainsKey(key) ? Session[key] : string.Empty;
        }
    }
}
