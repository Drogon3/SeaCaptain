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
        //private int _hp = 100;
        public string _shipName { get; private set; }
        public int _maxCargo { get; private set; }
        public Ship(int x, int y, string shipName)
        {
            X = x;
            Y = y;
            _shipName = shipName;
        }
        public event Action<int, int, int, int>? Moved;
        private void Move(int dx, int dy)
        {
            int oldX = X;
            int oldY = Y;
            X += dx;
            Y += dy;

            Moved?.Invoke(oldY, oldX, X, Y);
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
