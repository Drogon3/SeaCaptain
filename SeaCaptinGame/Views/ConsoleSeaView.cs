using SeaCaptinGame.Core.View;
using SeaCaptinGame.Core.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaCaptinGame.Core.Model;

namespace SeaCaptinGame.Views
{
    public class ConsoleSeaView : ISeaView
    {
        private ConsoleStartView _startView;
        private ConsoleGameView _gameView;
        private ConosleMinimapView _mapView;

        public ConsoleSeaView()
        {
            _startView = new ConsoleStartView();
            _gameView = new ConsoleGameView();
            _mapView = new ConosleMinimapView();
        }

        public MenuCommands ShowStartMenu() => _startView.ShowMenu();
        public string? ReadShipName() => _startView.ChooseShipName();
        public (ShipType, int)? ReadShipType() => _startView.ChooseShipType();

        public void Init(GameMap map, Ship ship) => _gameView.Init(map, ship);
        public GameCommands ReadCommand() => _gameView.ReadCommand();
        public void ShowMap()
        {
            _mapView.ShowMap();
            _gameView.Redraw();
        }
        public void Close() => _gameView.Close();
    }
}
