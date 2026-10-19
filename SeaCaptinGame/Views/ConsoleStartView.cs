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

            //Console.WriteLine(MenuOptions(option), isSelected
            //    ? Console.ForegroundColor = ConsoleColor.Cyan
            //    : Console.ForegroundColor = ConsoleColor.Gray);
            //Console.ResetColor();
        }
    }
}
