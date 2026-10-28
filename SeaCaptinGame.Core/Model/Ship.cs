using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Model
{
    public enum ShipType
    {
        BasicShip,
        BigShip,
        Kayak,
        Student
    }
    public class Ship
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        //private int _hp = 100;
        public string _shipName { get; private set; }
        public int _maxCargo { get; private set; }  
        public int _currentCargo { get; private set; }  
        public double _coins { get; private set; }        
        public int _shipDraft { get; private set; } // zanurzona część statku 
        public ShipType _shipType { get; }
        public List<(Item item, int number)> _cargo { get; private set; } = new();

        public Ship(int x, int y, string shipName, ShipType shipType, int shipDraft)
        {
            X = x;
            Y = y;
            _shipName = shipName;
            _shipType = shipType;
            _shipDraft = shipDraft;
            _coins = 500;
            _currentCargo = 0;

            switch (shipType)
            {
                case ShipType.BigShip:
                    _maxCargo = 250;
                    break;
                case ShipType.BasicShip:
                    _maxCargo = 150;
                    break;
                case ShipType.Kayak:
                    _maxCargo = 100;
                    break;
                case ShipType.Student:
                    _maxCargo = 50;
                    break;
            }
        }
        public void CountCurrentCargo()
        {
            _currentCargo = 0;
            for(int i = 0; i < _cargo.Count; i++)
            {
                _currentCargo += (_cargo[i].item.weight * _cargo[i].number);
            }
        }
        public int FreeSpace => _maxCargo - _currentCargo;

        public int CountOf(Item item)
        {
            for(int i = 0; i < _cargo.Count; i++)
            {
                if (_cargo[i].item == item) return _cargo[i].number;
            }
            return 0;
        }
        public void AddCoins(double amount)
        {
            _coins += Math.Round(amount, 2);
        }
        public bool TrySpendCoins(double amount)
        {
            if(amount < 0 || amount > _coins) return false;
            _coins -= Math.Round(amount, 2);
            return true;
        }


        public bool CanAddItem(Item item, int number)
        {
            int fullWeight = number * item.weight;
            int found = -1;
            CountCurrentCargo();

            if(fullWeight + _currentCargo <= _maxCargo && number > 0)
            {
                for(int i = 0; i < _cargo.Count; i++)
                {
                    if (_cargo[i].item == item) found = i;
                }
                if(found >= 0) _cargo[found] = (item, _cargo[found].number + number);
                else _cargo.Add((item, number));

                _currentCargo += item.weight * number;

                return true;
            }
            else return false;
        }
        public bool CanRemoveItem(Item item, int number)
        {
            int found = -1;
            CountCurrentCargo();

            for (int i = 0; i < _cargo.Count; i++)
            {
                if (_cargo[i].item == item) found = i;
            }
            if(number == _cargo[found].number)
            {
                _currentCargo -= item.weight * number;
                _cargo.RemoveAt(found);
                return true;
            }

            if (number <= _cargo[found].number) _cargo[found] = (item, _cargo[found].number - number);
            else return false;

            _currentCargo -= item.weight * number;
            return true;

        }

        public event Action<int, int, int, int>? Moved;
        private void Move(int dx, int dy)
        {
            int oldX = X;
            int oldY = Y;
            X += dx;
            Y += dy;

            Moved?.Invoke(oldX, oldY, X, Y);
        }
        public void N()
        {
            Move(0, -1);
        }
        public void S()
        {
            Move(0, 1);
        }
        public void E()
        {
            Move(1, 0);
        }
        public void W()
        {
            Move(-1, 0);
        }
    }
}
