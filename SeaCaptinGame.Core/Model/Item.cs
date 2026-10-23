using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private int _idItem;
        public string _itemName { get; }
        public float _baseValue { get; }
        public int _weight { get; }
        public Rarity _rarity { get; }

        public Item(int idItem, string itemName, int baseValue, int weight, Rarity rarity)
        {
            _idItem = idItem;
            _itemName = itemName;
            _baseValue = baseValue;
            _weight = weight;
            _rarity = rarity;
        }
    }
}
