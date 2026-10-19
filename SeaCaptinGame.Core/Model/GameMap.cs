using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Core.Model
{
    public class GameMap
    {
        private MapElements[,] _elements;
        private char[,] _symbols;

        public int Width { get; }
        public int Height { get; }
        public int StartX { get; private set; }
        public int StartY { get; private set; }

        private GameMap(int width, int height)
        {
            Width = width;
            Height = height;
            _elements = new MapElements[Width, Height];
            _symbols = new char[Width, Height];
            StartX = width / 2;
            StartY = height / 2;
        }

        public static GameMap FromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            GameMap gamemap = FromLines(lines);
            return gamemap;
        }

        public char GetSymbol(int x, int y)
        {
            if(IsInside(x, y)) return _symbols[x, y];
            return ' ';
        }
        public bool IsInside(int x, int y)
        {
            if(x >= 0 && x < Width && y >= 0 && y < Height) return true;
            return false;
        }
        public bool IsWater(int x, int y)
        {
            if (IsInside(x, y) && _elements[x, y] == MapElements.Water) return true;
            return false;
        }
        public static GameMap FromLines(string[] lines)
        {
            int height = lines.Length;
            int width = lines[0].Length;

            GameMap gameMap = new GameMap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char c = lines[y][x];
                    gameMap._symbols[x,y] = c;
                    if (c == ' ' || c == '~')
                        gameMap._elements[x, y] = MapElements.Water;
                    else
                        gameMap._elements[x, y] = MapElements.Land;
                    
                }
            }
            return gameMap;
        }
    }
}
