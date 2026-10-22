using SeaCaptinGame.Core.Command;
using SeaCaptinGame.Core.Model;
using SeaCaptinGame.Core.View;
using Spectre.Console.Rendering;
using System;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading;

namespace SeaCaptinGame.Views;

public class ConsoleGameView
{

    private GameMap _gameMap;

    private Ship _ship;
    private ShipType _shipType;
    private string[] _shipFile;

    private int _width, _height;

    private Pixel[,] _currentFrame = new Pixel[0, 0];
    private Pixel[,] _nextFrame = new Pixel[0, 0];
    private struct Pixel
    {
        public char symbol;
        public ConsoleColor color;
    }

    public void Init(GameMap gameMap, Ship ship)
    {
        _gameMap = gameMap;
        _ship = ship;
        _shipType = ship._shipType;
        _shipFile = GetShipLines(_shipType);

        Console.CursorVisible = false;
        Console.Clear();

        CalculateLayout();
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
        _width = Console.WindowWidth;
        _height = Console.WindowHeight;
        
        _currentFrame = new Pixel[_height, _width];
        _nextFrame = new Pixel[_height, _width];
    }
    private void Render()
    {

        if (Console.WindowWidth != _width || Console.WindowHeight != _height)
        {
            Console.Clear();
            CalculateLayout();
        }

        BuildFrame();
        DrawInfo($"{_ship._shipName} | Współrzędne: {_ship.X}, {_ship.Y} | Cargo: {_ship._maxCargo}");
        Flush();
    }

    private void BuildFrame()
    {
        int centerX = _width / 2;
        int centerY = _height / 2;

        for(int  i = 0; i < _height; i++)
        {
            for(int j = 0; j < _width; j++)
            {
                int worldX = _ship.X - centerX + j;
                int worldY = _ship.Y - centerY + i;

                (char mapElement, ConsoleColor colorElement) = GetSymbolData(worldX, worldY);
                _nextFrame[i, j] = new Pixel { symbol = mapElement, color = colorElement };
            }
        }
        DrawShip(centerX, centerY);
    }
    private void DrawShip(int anchorX, int anchorY)
    {
        int max = 0;
        foreach(string line in _shipFile)
        {
            if(line.Length > max) max = line.Length;
        }

        int left = anchorX - max / 2;
        int top = anchorY - _shipFile.Length + 1;

        for (int i = 0; i < _shipFile.Length; i++)
        {
            int y = top + i;
            string line = _shipFile[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == ' ') continue;

                int x = left + j;

                _nextFrame[y, x] = new Pixel { symbol = line[j], color = ConsoleColor.Gray };
            }
        }
        //_nextFrame[anchorY, anchorX] = new Pixel { symbol = '@', color = ConsoleColor.Red };
    }

    private (char, ConsoleColor) GetSymbolData(int worldX, int worldY)
    {
        char symbol = _gameMap.GetSymbol(worldX, worldY);
        return (symbol, ColorFor(symbol));
    }

    private string[] GetShipLines(ShipType shipType)
    {
        Dictionary<ShipType, string[]> shipFiles= new();

        string path = Path.Combine(AppContext.BaseDirectory, "Assets", "Ships", shipType + ".txt");
        string[] lines = File.ReadAllLines(path);

        //shipFiles[shipType] = lines;
        //Console.WriteLine(lines.Length);
        return lines;
    }

    private void Flush()
    {
        for (int y = 0; y < _height; y++)
        {
            int x = 0;
            while (x < _width)
            {
                if (IsSame(_currentFrame[y, x], _nextFrame[y, x]))
                {
                    x++;
                    continue;
                }

                ConsoleColor color = _nextFrame[y, x].color;
                int start = x;
                var sb = new StringBuilder();

                while (x < _width && !IsSame(_currentFrame[y, x], _nextFrame[y, x]) && _nextFrame[y, x].color == color)
                {
                    sb.Append(_nextFrame[y, x].symbol);
                    _currentFrame[y, x] = _nextFrame[y, x];
                    x++;
                }

                Console.SetCursorPosition(start, y);
                Console.ForegroundColor = color;
                Console.Write(sb.ToString());
            }
        }
        Console.ResetColor();
    }
    private bool IsSame(Pixel x, Pixel y)
    {
        return (x.symbol == y.symbol && x.color == y.color);
    }

    private void DrawInfo(string text)
    {
        int h = ConsoleHelper.HeightForText(text, _width) + 2;
        int countChar = 0;

        _nextFrame[_height - 1 - h, 0] = new Pixel { symbol = '+', color = ConsoleColor.Gray };
        for (int i = 1; i < _width; i++)
        {
            _nextFrame[_height - 1 - h, i] = new Pixel { symbol = '-', color = ConsoleColor.Gray };
        }
        _nextFrame[_height - 1 - h, _width - 1] = new Pixel { symbol = '+', color = ConsoleColor.Gray };

        for(int i = _height - h; i < _height - 1; i++)
        {
            for(int j = 1; j < _width - 1; j++)
            {
                if(countChar < text.Length)
                {
                    _nextFrame[i, j] = new Pixel { symbol = text[countChar], color = ConsoleColor.Gray };
                    countChar++;
                }
                else
                    _nextFrame[i, j] = new Pixel { symbol = ' ', color = ConsoleColor.Gray };
            }
        }

        _nextFrame[_height - 1, 0] = new Pixel { symbol = '+', color = ConsoleColor.Gray };
        for (int i = 1; i < _width; i++)
        {
            _nextFrame[_height - 1, i] = new Pixel { symbol = '-', color = ConsoleColor.Gray };
        }
        _nextFrame[_height - 1, _width - 1] = new Pixel { symbol = '+', color = ConsoleColor.Gray };
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
            //case '-': return ConsoleColor.DarkRed;
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