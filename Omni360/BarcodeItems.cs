using System.Text.Json.Serialization;

namespace Omnicheck360
{
    public class BarcodeItem
    {
        public int? Recipes { get; set; }
        public int? Index { get; set; }
        public string UPC { get; set; }
        [JsonPropertyName("Code de produit")]
        public string CodeDeProduit { get; set; }
        public string Description { get; set; }
    }
}