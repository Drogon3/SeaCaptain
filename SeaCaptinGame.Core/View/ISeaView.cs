using SeaCaptinGame.Core.Command;
using SeaCaptinGame.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.View
{
    public interface ISeaView
    {
        MenuCommands ShowStartMenu();
        string ReadShipName();
        void Init(GameMap gameMap, Ship ship);
        void ShowMap();
        GameCommands ReadCommand();
        void Close();

    }
}
