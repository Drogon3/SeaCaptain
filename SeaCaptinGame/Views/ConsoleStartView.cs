using SeaCaptinGame.Core.Command;
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
            ConsoleWriteAt.WriteAt(width / 2 - title.Length, 5, title, ConsoleColor.Cyan);

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
        public string? ChooseShipAndName()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Console.CursorVisible = false;
            string text = "Wybierz statek i nadaj mu imie!";
            ConsoleWriteAt.WriteAt(width / 2 - text.Length + 11, 10, text, ConsoleColor.DarkCyan);

            ConsoleWriteAt.WriteAt(width / 2 - text.Length + 11, 11, "> ", ConsoleColor.DarkCyan);
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
                        ConsoleWriteAt.WriteAt(width / 2 - errorText.Length + 12, 12, errorText, ConsoleColor.Red);
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
            ConsoleWriteAt.WriteAt(40, 7 + row, text,
                isSelected
                ? Console.ForegroundColor = ConsoleColor.Cyan
                : Console.ForegroundColor = ConsoleColor.Gray);
        }
    }
}
