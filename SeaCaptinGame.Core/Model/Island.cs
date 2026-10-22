using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Model
{
    public class Island
    {
        private string _islandName { get; }
        private int _X { get; }
        private int _Y { get; }
        public int _range { get; }
        private List<int> _itemsForSale;
        private List<int> _itemsToBuy;
        private double _priceFactor { get; }

        public Island(string islandName, int X, int Y, int range, List<int> itemsForSale, List<int> itemsToBuy, double priceFactor = 1.0)
        {
            _islandName = islandName;
            _X = X;
            _Y = Y;
            _range = range;
            _itemsForSale = itemsForSale;
            _itemsToBuy = itemsToBuy;
            _priceFactor = priceFactor;
        }
        public bool InRange(int shipX, int shipY, int shipDraft)
        {
            return (Math.Abs(shipX + (shipDraft / 2) - _X) < _range 
                || Math.Abs(shipX - (shipDraft / 2) - _X) < _range);
        }
        public double ItemBuyPrice(Item item)
        {
            return (Math.Round((item._baseValue * _priceFactor), 2));
        }
        public double ItemSalePrice(Item item)
        {
            return (Math.Round(item._baseValue, 2));
        }
    }
}