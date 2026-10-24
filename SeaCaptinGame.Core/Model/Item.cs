using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Model
{
    public enum Rarity
    {
        Common,
        Rare,
        Epic
    }
    public class Item
    {
        public int idItem { get; init; }
        public string itemName { get; init; }
        public double baseValue { get; init; }
        public int weight { get; init; }
        public Rarity rarity { get; init; }

        public Item(int idItem, string itemName, double baseValue, int weight, Rarity rarity)
        {
            this.idItem = idItem;
            this.itemName = itemName;
            this.baseValue = baseValue;
            this.weight = weight;
            this.rarity = rarity;
        }

        public static Dictionary<int, Item> LoadFromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Brak pliku z itemkami w podanej ścieżce.");

            var options = new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            List<Item>? data = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(path), options);
            var items = new Dictionary<int, Item>();

            foreach(Item item in data)
            {
                items.Add(item.idItem, item);
            }

            return items;
        }
    }
}
