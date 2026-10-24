using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaCaptinGame.Core.Model;
using Spectre.Console;

namespace SeaCaptinGame.Views
{
    public class ConsoleTradeView
    {            
        public void ShowStock(Island island, Ship ship)
        {
            var mainTable= new Table().Expand();

            mainTable.AddColumn("Lewa");
            mainTable.AddColumn("Prawa");

            var islandTable = new Table().Expand();
            islandTable.Title(island.islandName);

            islandTable.AddColumn("Nazwa");
            islandTable.AddColumn("Cena Zakupu");
            islandTable.AddColumn("Cena sprzedaż");
            islandTable.AddColumn("Waga (1 szt.)");

            if(island.itemsForSale != null)
            {
                for(int i = 0; i < island.itemsForSale.Count; i++)
                {
                    Item forShort = island.itemsForSale[i];
                    islandTable.AddRow(ConsoleHelper.ColorItemRarity(forShort.rarity, forShort.itemName),
                        island.ItemBuyPrice(forShort).ToString(),
                        island.ItemSalePrice(forShort).ToString(),
                        forShort.weight.ToString());
                }
            }
            
            var shipTable = new Table().Expand();
            shipTable.Title(ship._shipName);

            shipTable.AddColumn("Nazwa");
            shipTable.AddColumn("Ilość");
            shipTable.AddColumn("Waga (1 szt.)");
            shipTable.AddColumn("Waga całkowita");

            if(ship._cargo != null)
            {
                for (int i = 0; i < ship._cargo.Count; i++)
                {
                    Item forShort = ship._cargo[i].item;
                    shipTable.AddRow(ConsoleHelper.ColorItemRarity(forShort.rarity, forShort.itemName),
                        ship._cargo[i].number.ToString(),
                        forShort.weight.ToString(),
                        (forShort.weight * ship._cargo[i].number).ToString());
                }
            }

            mainTable.AddRow(islandTable, shipTable);
            AnsiConsole.Write(mainTable);

        }
    }
}
