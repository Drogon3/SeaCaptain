using SeaCaptinGame.Core.Command;
using SeaCaptinGame.Core.Model;
using SeaCaptinGame.Core.View;
using Spectre.Console.Rendering;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace SeaCaptinGame.Views;

public class ConsoleGameView
{
    private ConsoleStartView _startView;

    private GameMap _gameMap;
    private Ship _ship;

    private int _width, _height;

    public ConsoleGameView()
    {
        _startView = new ConsoleStartView();
    }

    public void Init(GameMap gameMap, Ship ship)
    {
        _gameMap = gameMap;
        _ship = ship;
        // Dodanie typu statku

        Console.CursorVisible = false;
        Console.Clear();
        _ship.Moved += OnShipMoved;
        Render();
    }

    private void OnShipMoved(int oldX, int oldY, int newX, int newY) => Render();
    public void Redraw()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        CalculateLayout();
        Render();
    }
    private void CalculateLayout()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;
        

    }
    private void Render()
    {

        if (Console.WindowWidth != _width || Console.WindowHeight != _height)
        {
            Console.Clear();
            CalculateLayout();
        }

        //BuildFrame();
        //Flush();
        //DrawHud();
    }

    private static ConsoleColor ColorFor(char symbol)
    {
        switch (symbol)
        {
            case '~': return ConsoleColor.Blue;
            case 'o': return ConsoleColor.DarkGray;
            case '.': return ConsoleColor.DarkYellow;
            case '[': return ConsoleColor.Yellow;
            case ']': return ConsoleColor.Yellow;
            case '/': return ConsoleColor.Red;
            case '\\': return ConsoleColor.Red;
            case '-': return ConsoleColor.DarkRed;
            case '0': return ConsoleColor.DarkGreen;
            default: return ConsoleColor.Gray;
        }
    }

    public GameCommands ReadCommand()
    {
        ConsoleKey key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.W: return GameCommands.North;
            case ConsoleKey.S: return GameCommands.South;
            case ConsoleKey.D: return GameCommands.East;
            case ConsoleKey.A: return GameCommands.West;
            case ConsoleKey.B: return GameCommands.Cargo;
            case ConsoleKey.M: return GameCommands.Map;
            case ConsoleKey.Escape: return GameCommands.Quit;
            default: return GameCommands.None;
        }
    }

    public void Close()
    {
        Console.ResetColor();
        Console.Clear();
    }
}