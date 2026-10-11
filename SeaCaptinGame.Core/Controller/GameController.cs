using SeaCaptinGame.Core.Command;
using SeaCaptinGame.Core.Model;
using SeaCaptinGame.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Controller
{
    public class GameController
    {
        private Ship _ship;
        private ISeaView _seaView;
        private int _width, _height;

        public GameController(Ship ship, ISeaView seaView, int width, int height)
        {
            _ship = ship;
            _seaView = seaView;
            _width = width;
            _height = height;
        }

        public void Run()
        {
            _seaView.Init(_ship);
            bool running = true;
            while (running)
            {
                GameCommands cmd = _seaView.ReadCommand();
                switch (cmd)
                {
                    case GameCommands.North:
                        if (_ship.Y > 0)
                            _ship.N();
                        break;
                    case GameCommands.South:
                        if (_ship.Y < _height - 1)
                            _ship.S();
                        break;
                    case GameCommands.East:
                        if (_ship.X < _width - 1)
                            _ship.E();
                        break;
                    case GameCommands.West:
                        if (_ship.X > 0)
                            _ship.W();
                        break;
                    case GameCommands.Quit:
                        running = false;
                        break;
                }
            }
            _seaView.Close();
        }
    }
}
