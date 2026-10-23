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
        public float _coins { get; private set; }        
        public int _shipDraft { get; private set; } // zanurzona część statku 
        public ShipType _shipType { get; }
        public List<(Item item, int number)> _cargo { get; set; }

        public Ship(int x, int y, string shipName, ShipType shipType, int shipDraft)
        {
            X = x;
            Y = y;
            _shipName = shipName;
            _shipType = shipType;
            _shipDraft = shipDraft;
            _coins = 100;
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
                _currentCargo += (_cargo[i].item._weight * _cargo[i].number);
            }
        }

        public bool CanAdd(Item item, int number)
        {
            int fullWeight = number * item._weight;
            CountCurrentCargo();
            return (fullWeight + _currentCargo <= _maxCargo && number > 0);
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
