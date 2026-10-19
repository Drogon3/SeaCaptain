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
        private GameMap _gameMap;

        public GameController(ISeaView seaView, GameMap gameMap)
        {
            _seaView = seaView;
            _gameMap = gameMap;
        }

        public void Run()
        {
            string shipName = RunStartScreen();
            if (shipName == null)
            {
               _seaView.Close();
                return;
            }
            _ship = new Ship(_gameMap.StartX, _gameMap.StartY, shipName);

            _seaView.Init(_gameMap, _ship);
            bool running = true;
            while (running)
            {
                GameCommands cmd = _seaView.ReadCommand();
                switch (cmd)
                {
                    case GameCommands.North:
                        if (_gameMap.IsWater(_ship.X, _ship.Y - 1))
                            _ship.N();
                        break;
                    case GameCommands.South:
                        if (_gameMap.IsWater(_ship.X, _ship.Y + 1))
                            _ship.S();
                        break;
                    case GameCommands.East:
                        if (_gameMap.IsWater(_ship.X + 1, _ship.Y))
                            _ship.E();
                        break;
                    case GameCommands.West:
                        if (_gameMap.IsWater(_ship.X - 1, _ship.Y))
                            _ship.W();
                        break;
                    case GameCommands.Cargo:
                        // open cargo
                        break;
                    case GameCommands.Map:
                        _seaView.ShowMap();
                        break;
                    case GameCommands.Quit:
                        running = false;
                        break;
                }
            }
            _seaView.Close();
        }
        private string RunStartScreen()
        {
            while (true)
            {
                MenuCommands option = _seaView.ShowStartMenu();
                if(option == MenuCommands.Quit)
                {
                    return null;
                }
                string shipName = _seaView.ReadShipName();
                if(shipName != null)
                {
                    return shipName;
                }
            }
        }
    }
}
