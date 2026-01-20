using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Omnicheck360
{
    public static class BarcodeLoader
    {
        public static List<BarcodeItem> Load(string path)
        {
            var text = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<BarcodeItem>>(text);
        }
    }
}