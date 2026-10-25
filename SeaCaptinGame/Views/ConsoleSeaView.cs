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
        private ConsoleCargoView _cargoView;
        private ConsoleTradeView _tradeView;

        public ConsoleSeaView()
        {
            _startView = new ConsoleStartView();
            _gameView = new ConsoleGameView();
            _mapView = new ConosleMinimapView();
            _cargoView = new ConsoleCargoView();
            _tradeView = new ConsoleTradeView();
        }

        public MenuCommands ShowStartMenu() => _startView.ShowMenu();
        public string? ReadShipName() => _startView.ChooseShipName();
        public (ShipType, int)? ReadShipType() => _startView.ChooseShipType();

        public void Init(GameMap map, Ship ship) => _gameView.Init(map, ship);
        public GameCommands ReadCommand() => _gameView.ReadCommand();

        public void SetNearbyIsland(Island? island) => _gameView.SetNearbyIsland(island);
        public void ShowMap()
        {
            _mapView.ShowMap();
            _gameView.Redraw();
        }
        public void ShowStock(Island island, Ship ship)
        {
            _tradeView.ShowStock(island, ship);
            _gameView.Redraw();
        }
        public void ShowCargo(Ship ship)
        {
            _cargoView.ShowCargo(ship);
            _gameView.Redraw();
        }


        public void Close() => _gameView.Close();
    }
}
