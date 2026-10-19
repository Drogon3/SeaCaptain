using SeaCaptinGame.Core.Command;
using SeaCaptinGame.Core.Model;
using SeaCaptinGame.Core.View;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace SeaCaptinGame.Views;

public class ConsoleGameView : ISeaView
{
    private ConsoleStartView _startView;
    public ConsoleGameView()
    {
        _startView = new ConsoleStartView();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Init(GameMap gameMap, Ship ship)
    {
        throw new NotImplementedException();
    }

    public GameCommands ReadCommand()
    {
        throw new NotImplementedException();
    }

    public string? ReadShipName()
    {
        return _startView.ChooseShipAndName();
    }

    public void ShowMap()
    {
        throw new NotImplementedException();
    }

    public MenuCommands ShowStartMenu()
    {
        return _startView.ShowMenu();
    }
}