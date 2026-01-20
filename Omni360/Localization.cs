using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Omnicheck360
{
    public static class Localization
    {
        public static Dictionary<string, string> Strings = new Dictionary<string, string>();

        public static void Load(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Strings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
        }

        public static string T(string key, params object[] args)
        {
            if (Strings.TryGetValue(key, out var val))
                return args.Length > 0 ? string.Format(val, args) : val;
            return key; // fallback
        }
    }
}