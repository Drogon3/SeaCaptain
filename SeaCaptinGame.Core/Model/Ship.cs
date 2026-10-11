using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Model
{
    public class Ship
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        private int _hp;
        //private int _direction; // kierunek do celu może jak będzie mi sie chciało xd
        private string _shipName;
        private int _maxCargo;
        public Ship(int x, int y, int hp, string shipName, int maxCargo)
        {
            X = x;
            Y = y;
            _hp = hp;
            _shipName = shipName;
            _maxCargo = maxCargo;
        }

        public void N()
        {
            Y -= 1;
        }
        public void S()
        {
            Y += 1;
        }
        public void E()
        {
            X += 1;
        }
        public void W()
        {
            X -= 1;
        }
    }
}
