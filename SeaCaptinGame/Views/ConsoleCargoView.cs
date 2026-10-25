using SeaCaptinGame.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace SeaCaptinGame.Views
{
    public class ConsoleCargoView
    {
        public void ShowCargo(Ship ship)
        {
            var itemList = ship._cargo;

            var cargoTable = new Table();
            cargoTable.Expand();

            cargoTable.Title("ŁADOWNIA");
            cargoTable.AddColumn("Nazwa");
            cargoTable.AddColumn("Ilość");
            cargoTable.AddColumn("Waga (1 szt.)");
            cargoTable.AddColumn("Waga całkowita");

            if(itemList != null)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    Item item = itemList[i].item;
                    cargoTable.AddRow(ConsoleHelper.ColorItemRarity(item.rarity, item.itemName),
                        itemList[i].number.ToString(),
                        item.weight.ToString(),
                        (item.weight * itemList[i].number).ToString());
                }
            }
            
            Console.Clear();
            AnsiConsole.Write(cargoTable);

            Console.Write($"Ładunek {ship._currentCargo} / {ship._maxCargo} ");
            Console.Write("[");
            for(int i = 0; i < ship._maxCargo; i+=10)
            {
                if (i < ship._currentCargo) Console.Write("#");
                else Console.Write("-");
            }
            Console.Write("]");

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.B) break;
            }

        }
    }
}
