using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Model
{
    public class Item
    {
        private int _idItem;
        public string _itemName { get; }
        public float _baseValue { get; }
        private int _weight;

        public Item(int idItem, string itemName, int baseValue, int weight)
        {
            _idItem = idItem;
            _itemName = itemName;
            _baseValue = baseValue;
            _weight = weight;
        }
    }
}
