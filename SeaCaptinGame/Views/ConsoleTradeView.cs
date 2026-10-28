using SeaCaptinGame.Core.Model;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaCaptinGame.Views
{
    public class ConsoleTradeView
    {
        private enum Focus { Shop, Inventory }
        private enum Mode { Browse, Quantity }

        private Focus _focus;
        private Mode _mode;
        private int _portCursor;
        private int _shipCursor;
        private int _quantity;
        private TradeActionKind _pendingKind;
        private Item? _pendingItem;
        private string _message = "";

        public void ShowStock(Island island, Ship ship, Func<TradeAction, TradeResult> execute)
        {
            _mode = Mode.Browse;
            _portCursor = 0;
            _shipCursor = 0;
            _quantity = 1;
            _pendingItem = null;
            _message = "";

            List<Item> shopItems = island.itemsForSale;
            foreach(Item item in island.itemsToBuy)
            {
                if(!shopItems.Contains(item)) shopItems.Add(item); 
            }

            int buyableCount = island.itemsForSale.Count;
            _focus = buyableCount > 0 ? Focus.Shop : Focus.Inventory;

            while (Console.KeyAvailable)
                Console.ReadKey(true);

            Console.CursorVisible = false;
            Console.Clear();

            int width = Console.WindowWidth;

           
            while (true)
            {
                if (Console.WindowWidth != width)
                {
                    width = Console.WindowWidth;
                    Console.Clear();
                }

                Console.SetCursorPosition(0, 0);
                AnsiConsole.Write(Build(island, ship, shopItems));

                int renderedBottom = Console.CursorTop;
                for (int row = renderedBottom; row < Console.WindowHeight; row++)
                {
                    Console.SetCursorPosition(0, row);
                    Console.Write(new string(' ', Math.Max(0, Console.WindowWidth - 1)));
                }
                Console.SetCursorPosition(0, 0);

                ConsoleKey key = Console.ReadKey(true).Key;

                bool exit = _mode == Mode.Browse
                    ? HandleBrowseKey(key, island, ship, buyableCount)
                    : HandleQuantityKey(key, island, ship, execute);

                if (exit)
                    break;

                ClampCursors(ship, buyableCount);
            }

            Console.CursorVisible = true;
            Console.Clear();
            
        }

        private bool HandleBrowseKey(ConsoleKey key, Island island, Ship ship, int buyableCount)
        {
            switch (key)
            {
                case ConsoleKey.T:
                    return true;

                case ConsoleKey.A:
                    if (buyableCount > 0)
                        _focus = Focus.Shop;
                    _message = "";
                    break;

                case ConsoleKey.D:
                    _focus = Focus.Inventory;
                    _message = "";
                    break;

                case ConsoleKey.W:
                    MoveCursor(-1, ship, buyableCount);
                    break;

                case ConsoleKey.S:
                    MoveCursor(1, ship, buyableCount);
                    break;

                case ConsoleKey.Enter:
                    OpenQuantityWindow(island, ship);
                    break;
            }

            return false;
        }

        private void MoveCursor(int step, Ship ship, int buyableCount)
        {
            _message = "";

            if (_focus == Focus.Shop && buyableCount > 0)
                _portCursor = (_portCursor + step + buyableCount) % buyableCount;
            else if (_focus == Focus.Inventory && ship._cargo.Count > 0)
                _shipCursor = (_shipCursor + step + ship._cargo.Count) % ship._cargo.Count;
        }

        private void OpenQuantityWindow(Island island, Ship ship)
        {
            if (_focus == Focus.Shop)
            {
                if (island.itemsForSale.Count == 0)
                    return;

                Item item = island.itemsForSale[_portCursor];

                if (Trade.MaxCanBuy(ship, island, item) == 0)
                {
                    _message = MessageFor(Trade.CheckBuy(ship, island, item, 1));
                    return;
                }

                _pendingKind = TradeActionKind.Buy;
                _pendingItem = item;
            }
            else
            {
                if (ship._cargo.Count == 0)
                {
                    _message = "[red]Ładownia jest pusty.[/]";
                    return;
                }

                Item item = ship._cargo[_shipCursor].item;

                if (Trade.MaxCanSell(ship, island, item) == 0)
                {
                    _message = $"[red]{Markup.Escape(island.islandName)} nie kupuje tego towaru.[/]";
                    return;
                }

                _pendingKind = TradeActionKind.Sell;
                _pendingItem = item;
            }

            _quantity = 1;
            _mode = Mode.Quantity;
            _message = "";
        }

        private bool HandleQuantityKey(
            ConsoleKey key,
            Island island,
            Ship ship,
            Func<TradeAction, TradeResult> execute)
        {
            Item item = _pendingItem!;
            bool buying = _pendingKind == TradeActionKind.Buy;
            int max = buying
                ? Trade.MaxCanBuy(ship, island, item)
                : Trade.MaxCanSell(ship, island, item);

            switch (key)
            {
                case ConsoleKey.W:
                    _quantity++;
                    break;

                case ConsoleKey.S:
                    _quantity--;
                    break;

                case ConsoleKey.D:
                    _quantity += 10;
                    break;

                case ConsoleKey.A:
                    _quantity -= 10;
                    break;

                case ConsoleKey.Escape:
                    _mode = Mode.Browse;
                    _message = "[grey]Anulowano.[/]";
                    return false;

                case ConsoleKey.Enter:
                    {
                        double amount = buying
                            ? Trade.BuyCost(island, item, _quantity)
                            : Trade.SellIncome(island, item, _quantity);

                        TradeResult result = execute(new TradeAction(_pendingKind, item, _quantity));

                        if (result == TradeResult.Ok)
                        {
                            string verb = buying ? "Kupiono" : "Sprzedano";
                            _message = $"[green]{verb} {_quantity} x {Markup.Escape(item.itemName)} za {amount:0.00}.[/]";
                        }
                        else
                        {
                            _message = MessageFor(result);
                        }

                        _mode = Mode.Browse;
                        _pendingItem = null;
                        return false;
                    }
            }

            _quantity = Math.Clamp(_quantity, 1, Math.Max(1, max));
            return false;
        }

        private void ClampCursors(Ship ship, int buyableCount)
        {
            _portCursor = Math.Clamp(_portCursor, 0, Math.Max(0, buyableCount - 1));
            _shipCursor = Math.Clamp(_shipCursor, 0, Math.Max(0, ship._cargo.Count - 1));
        }

        private IRenderable Build(Island island, Ship ship, List<Item> shopItems)
        {
            bool quantityMode = _mode == Mode.Quantity && _pendingItem != null;
            bool buying = quantityMode && _pendingKind == TradeActionKind.Buy;
            bool selling = quantityMode && _pendingKind == TradeActionKind.Sell;

            var invTable = new Table().Expand();
            invTable.AddColumn(" ");
            invTable.AddColumn("Produkt");
            invTable.AddColumn(new TableColumn("Ilość"));
            invTable.AddColumn(new TableColumn("Waga razem"));

            if (selling)
            {
                invTable.AddColumn(new TableColumn("Do sprzedaży"));
                invTable.AddColumn(new TableColumn("Ilość do sprzedaży"));
                invTable.AddColumn(new TableColumn("Przychód"));
                invTable.AddColumn(new TableColumn("Monety po"));
                invTable.AddColumn(new TableColumn("Ładunek po"));
            }

            if (ship._cargo.Count == 0)
            {
                if (selling)
                    invTable.AddRow(" ", "[grey](Ładownia jest pusta)[/]", "", "", "", "", "", "", "", "");
                else
                    invTable.AddRow(" ", "[grey](Ładownia jest pusta)[/]", "", "");
            }
            else
            {
                for (int i = 0; i < ship._cargo.Count; i++)
                {
                    Item item = ship._cargo[i].item;
                    int number = ship._cargo[i].number;
                    bool selected = _focus == Focus.Inventory && i == _shipCursor;
                    string name = selected
                        ? $"[black on aqua]{Markup.Escape(item.itemName)}[/]"
                        : ConsoleHelper.ColorItemRarity(item.rarity, item.itemName);
                    string offer = island.IsToBuy(item)
                        ? island.ItemSalePrice(item).ToString("0.00")
                        : "-";

                    if (selling && selected)
                    {
                        double income = Trade.SellIncome(island, item, _quantity);
                        int transactionWeight = item.weight * _quantity;
                        double coinsAfter = ship._coins + income;
                        double cargoAfter = ship._currentCargo - transactionWeight;

                        invTable.AddRow(
                            "[aqua]>[/]", name, number.ToString(),
                            (item.weight * number).ToString(), offer,
                            $"[yellow]{_quantity}[/]",
                            $"[green]{income:0.00}[/]",
                            $"[yellow]{coinsAfter:0.00}[/]",
                            $"{cargoAfter}/{ship._maxCargo}");
                    }
                    else if (selling)
                    {
                        invTable.AddRow(
                            selected ? "[aqua]>[/]" : " ", name, number.ToString(),
                            (item.weight * number).ToString(), offer, "-", "-", "-", "-", "-");
                    }
                    else
                    {
                        invTable.AddRow(
                            selected ? "[aqua]>[/]" : " ", name, number.ToString(),
                            (item.weight * number).ToString());
                    }
                }
            }

            var shopTable = new Table().Expand().Border(TableBorder.Rounded);
            shopTable.AddColumn(" ");
            shopTable.AddColumn("Produkt");
            shopTable.AddColumn(new TableColumn("Zakup"));
            shopTable.AddColumn(new TableColumn("Sprzedaż"));
            shopTable.AddColumn(new TableColumn("Waga/szt."));

            if (buying)
            {
                shopTable.AddColumn(new TableColumn("Do kupna"));
                shopTable.AddColumn(new TableColumn("Koszt"));
                shopTable.AddColumn(new TableColumn("Waga"));
                shopTable.AddColumn(new TableColumn("Monety po"));
                shopTable.AddColumn(new TableColumn("Ładunek po"));
            }

            for (int i = 0; i < shopItems.Count; i++)
            {
                Item item = shopItems[i];
                bool buyable = island.IsForSale(item);
                bool selected = _focus == Focus.Shop && buyable && i == _portCursor;

                string name;
                if (!buyable)
                    name = $"[grey]{Markup.Escape(item.itemName)}[/]";
                else if (selected)
                    name = $"[black on aqua]{Markup.Escape(item.itemName)}[/]";
                else
                    name = ConsoleHelper.ColorItemRarity(item.rarity, item.itemName);

                string buyPrice = buyable ? island.ItemBuyPrice(item).ToString("0.00") : "-";
                string sellPrice = island.IsToBuy(item) ? island.ItemSalePrice(item).ToString("0.00") : "-";

                if (buying && selected)
                {
                    double cost = Trade.BuyCost(island, item, _quantity);
                    int transactionWeight = item.weight * _quantity;
                    double coinsAfter = ship._coins - cost;
                    double cargoAfter = ship._currentCargo + transactionWeight;

                    shopTable.AddRow(
                        "[aqua]>[/]", name, buyPrice, sellPrice, item.weight.ToString(),
                        $"[yellow]{_quantity}[/]",
                        $"[yellow]{cost:0.00}[/]",
                        transactionWeight.ToString(),
                        $"[yellow]{coinsAfter:0.00}[/]",
                        $"{cargoAfter}/{ship._maxCargo}");
                }
                else if (buying)
                {
                    shopTable.AddRow(
                        selected ? "[aqua]>[/]" : " ", name, buyPrice, sellPrice, item.weight.ToString(),
                        "-", "-", "-", "-", "-");
                }
                else
                {
                    shopTable.AddRow(
                        selected ? "[aqua]>[/]" : " ", name, buyPrice, sellPrice, item.weight.ToString());
                }
            }

            var invPanel = new Panel(invTable)
                .Header($" Ładownia: {Markup.Escape(ship._shipName)} ")
                .BorderColor(_focus == Focus.Inventory ? Color.Aqua : Color.Grey)
                .Expand();

            var shopPanel = new Panel(shopTable)
                .Header($" Sklep: {Markup.Escape(island.islandName)} ")
                .BorderColor(_focus == Focus.Shop ? Color.Aqua : Color.Grey)
                .Expand();

            string status =
                $"[yellow]Monety: {ship._coins:0.00}[/]    Ładunek: {ship._currentCargo}/{ship._maxCargo}";

            string hints = quantityMode
                ? "[grey]W/S - ilość +-1   A/D - ilość +-10   Enter - zatwierdź   Esc - anuluj[/]"
                : "[grey]W/S - wybór   A/D - Port / Ładunek   Enter - Kup lub sprzedaj   T - Wyjdź[/]";


            return new Rows(
                Line(status),
                invPanel,
                shopPanel,
                Line(hints),
                Line(_message == "" ? " " : _message));
        }

        private static IRenderable Line(string markup)
        {
            return new Panel(new Markup(markup)).NoBorder().Padding(0, 0).Expand();
        }

        private static string MessageFor(TradeResult result)
        {
            switch (result)
            {
                case TradeResult.Ok: return "[green]Transakcja udana.[/]";
                case TradeResult.NotAvailable: return "[red]Ten towar nie jest tu dostępny w tę stronę.[/]";
                case TradeResult.NotEnoughCoins: return "[red]Za mało monet.[/]";
                case TradeResult.NoSpace: return "[red]Brak miejsca w ładowni.[/]";
                case TradeResult.NotEnoughItems: return "[red]Nie masz tyle tego towaru.[/]";
                case TradeResult.InvalidAmount: return "[red]Nieprawidłowa ilość.[/]";
                default: return "";
            }
        }
    }
}

