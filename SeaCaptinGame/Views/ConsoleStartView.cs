using SeaCaptinGame.Core.Command;
using SeaCaptinGame.Core.Model;
using SeaCaptinGame.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Views
{
    public class ConsoleStartView
    {
        private ConsoleShipView _shipView;

        public MenuCommands ShowMenu()
        {
            Console.CursorVisible = false;
            Console.Clear();

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            string title = "SEA CAPTIN";
            //Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine(title);
            //Console.ResetColor();
            ConsoleHelper.WriteAt(width / 2 - title.Length, 5, title, ConsoleColor.Cyan);

            int selected = 0;

            MenuCommands[] options = { MenuCommands.Start, MenuCommands.Quit };
            for(int i = 0; i < options.Length; i++)
            {
                DrawMenuOption(options[i], i, i == selected);
            }

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                int prev = selected;
                switch (key)
                {
                    case ConsoleKey.W:
                        selected = (selected + options.Length - 1) % options.Length;
                        break;
                    case ConsoleKey.S:
                        selected = (selected + 1) % options.Length;
                        break;
                    case ConsoleKey.Enter:
                        return options[selected];
                    case ConsoleKey.Escape:
                        return MenuCommands.Quit;
                }
                if(prev != selected)
                {
                    DrawMenuOption(options[selected], selected, true);
                    DrawMenuOption(options[prev], prev, false);
                }
            }
        }
        public string? ChooseShipName()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Console.CursorVisible = false;
            string text = "Wybierz statek i nadaj mu imie!";
            ConsoleHelper.WriteAt(width / 2 - text.Length + 11, 10, text, ConsoleColor.DarkCyan);

            ConsoleHelper.WriteAt(width / 2 - text.Length + 11, 11, "> ", ConsoleColor.DarkCyan);
            Console.SetCursorPosition(width / 2 - text.Length + 13, 11);
            Console.ResetColor();

            StringBuilder sb = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                switch (info.Key)
                {
                    case ConsoleKey.Enter:
                        string shipName = sb.ToString().Trim();
                        
                        if (shipName.Length > 0)
                            return shipName;
                        
                        string errorText = "Nazwa statku nie może być pusta.";
                        ConsoleHelper.WriteAt(width / 2 - errorText.Length + 12, 12, errorText, ConsoleColor.Red);
                        Console.SetCursorPosition(width / 2 - text.Length + 13, 11);
                        Console.ResetColor();
                        break;
                    case ConsoleKey.Escape:
                        return null;
                    case ConsoleKey.Backspace:
                        if(sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            Console.Write("\b \b");
                        }
                        break;
                    default:
                        sb.Append(info.KeyChar);
                        Console.Write(info.KeyChar);
                        break;
                }
            }
        }

        public ShipType? ChooseShipType()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            int selected = 0;
            ShipType[] types = (ShipType[])Enum.GetValues(typeof(ShipType));

            string path = Path.Combine(AppContext.BaseDirectory, "Assets/Ships");
            var shipFiles = Directory.GetFiles(path, "*.txt");
            var shipsToChoose = new List<(string fileName, string[] ship)>();

            foreach(string shipFile in shipFiles)
            {
                string shipName = Path.GetFileNameWithoutExtension(shipFile);
                string[] shipFields = File.ReadAllLines(shipFile);
                
                shipsToChoose.Add((shipName, shipFields));
            }
            DrawShipToChoose(shipsToChoose[selected].ship, width);

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                int prev = selected;

                switch (key)
                {
                    case ConsoleKey.A:
                        selected = (selected + shipsToChoose.Count - 1) % shipsToChoose.Count;
                        break;
                    case ConsoleKey.D:
                        selected = (selected + 1) % shipsToChoose.Count;
                        break;
                    case ConsoleKey.Enter:
                        return types[selected];
                    case ConsoleKey.Escape:
                        return null;
                }
                if (prev != selected)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        ConsoleHelper.WriteAt(width / 2 - 25, 12 + i, "                                      ", ConsoleColor.Gray);
                    }
                    DrawShipToChoose(shipsToChoose[selected].ship, width);
                }
            }
        }
        private string MenuOptions(MenuCommands option)
        {
            switch (option)
            {
                case MenuCommands.Start:
                    return "Rozpocznij piracka przygode!";
                case MenuCommands.Quit:
                    return "Wyjdz";
                default:
                    return "";
            }
        }
        private void DrawMenuOption(MenuCommands option, int row, bool isSelected)
        {
            string text = isSelected ? "~ " + MenuOptions(option) : "  " + MenuOptions(option);
            ConsoleHelper.WriteAt(40, 7 + row, text,
                isSelected
                ? Console.ForegroundColor = ConsoleColor.Cyan
                : Console.ForegroundColor = ConsoleColor.Gray);
        }

        private void DrawShipToChoose(string[] ship, int width)
        {
            int max = 0;
            foreach (string line in ship)
            {
                if (line.Length > max) max = line.Length;
            }
            for (int i = 0; i < ship.Length; i++)
            {
                ConsoleHelper.WriteAt(width / 2 - max / 2 - 5, 12 + i, ship[i], ConsoleColor.Gray);
            }
        }
    }
}
