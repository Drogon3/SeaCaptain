using SeaCaptinGame.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Model
{
    public class Island
    {
        public string islandName { get; }
        public int X { get; }
        public int Y { get; }
        public int range { get; }
        public List<Item> itemsForSale { get; }
        public List<Item> itemsToBuy { get; }

        public List<int> itemsForSaleId { get; init; }
        public List<int> itemsToBuyId { get;  init; }
        public double priceFactor { get; }

        public Island(string islandName, int X, int Y, int range, List<Item> itemsForSale, List<Item> itemsToBuy, double priceFactor = 1.0)
        {
            this.islandName = islandName;
            this.X = X;
            this.Y = Y;
            this.range = range;
            this.itemsForSale = itemsForSale;
            this.itemsToBuy = itemsToBuy;
            this.priceFactor = priceFactor;
        }
        public bool InRange(int shipX, int shipY, int shipDraft)
        {
            int halfWidth = shipDraft / 2;
            int halfRange = range / 2;
            if ((shipX - halfWidth) >= X - halfRange && (shipX - halfWidth) <= X + halfRange
                && shipY >= Y - halfRange && shipY <= Y + halfRange)
                return true;
            else if ((shipX + halfWidth) >= X - halfRange && (shipX + halfWidth) <= X + halfRange
                && shipY >= Y - halfRange && shipY <= Y + halfRange)
                return true;
            return false;
        }
        public bool IsForSale(Item item) => itemsForSale.Contains(item);
        public bool IsToBuy(Item item) => itemsToBuy.Contains(item);
        public double ItemBuyPrice(Item item)
        {
            return (Math.Round((item.baseValue * priceFactor * 1.1), 2));
        } 
        public double ItemSalePrice(Item item)
        {
            return (Math.Round(item.baseValue * priceFactor * 0.9, 2));
        }
        public static List<Island> LoadFormFile(string path, Dictionary<int, Item> items)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Brak pliku z wyspami w podanej ścieżce.");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Island>? data = JsonSerializer.Deserialize<List<Island>>(File.ReadAllText(path), options);

            var islands = new List<Island>();
            var itemsId = new List<int>();

            foreach(Island island in data)
            {
                List<Item> forSale = ConnectIdToItem(island.itemsForSaleId, items);
                List<Item> toBuy = ConnectIdToItem(island.itemsToBuyId, items);

                islands.Add(new Island(island.islandName, island.X, island.Y, island.range, forSale, toBuy, island.priceFactor));
            }
            return islands;

        }
        private static List<Item> ConnectIdToItem(List<int> islandItems, Dictionary<int, Item> items)
        {
            var itemList = new List<Item>();
            foreach(int id in islandItems)
            {
                if(items.ContainsKey(id)) itemList.Add(items[id]);
            }
            return itemList;
        }

    }
}