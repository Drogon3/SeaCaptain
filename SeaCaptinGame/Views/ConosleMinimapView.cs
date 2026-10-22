using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaCaptinGame.Views
{
    public class ConosleMinimapView
    {
        public void ShowMap()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", "minimap3.txt");

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;

            string[] miniMap = File.ReadAllLines(path);
            int marginW = Console.WindowWidth - miniMap[0].Length;
            int marginH = Console.WindowHeight - miniMap.Length;


            if(marginW > 0) { marginW = marginW / 2; }
            if(marginH > 0) { marginH = marginH / 2; }

            for(int i=0; i<miniMap.Length; i++)
            {
                Console.SetCursorPosition(marginW, i + marginH);
                Console.Write(miniMap[i]);
            }

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.M)
                    break;
            }

            Console.ResetColor();
            Console.Clear();
        }
    }
}
