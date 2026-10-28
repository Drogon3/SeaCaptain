using System;

namespace SeaCaptinGame.Core.Model
{
    public enum TradeResult
    {
        Ok,
        NotAvailable,
        NotEnoughCoins,
        NoSpace,
        NotEnoughItems,
        InvalidAmount
    }

    public enum TradeActionKind
    {
        Buy,
        Sell
    }
    public record TradeAction(TradeActionKind Kind, Item Item, int number);

    public static class Trade
    {
        public static double BuyCost(Island island, Item item, int number) => Math.Round(island.ItemBuyPrice(item) * number, 2);
        public static double SellIncome(Island island, Item item, int number) => Math.Round(island.ItemSalePrice(item) * number, 2);
        public static TradeResult CheckBuy(Ship ship, Island island, Item item, int number)
        {
            if (number <= 0) return TradeResult.InvalidAmount;
            if (!island.IsForSale(item)) return TradeResult.NotAvailable;
            if (ship._coins < BuyCost(island, item, number)) return TradeResult.NotEnoughCoins;
            if (item.weight * number > ship.FreeSpace) return TradeResult.NoSpace;
            return TradeResult.Ok;
        }
        public static TradeResult CheckSell(Ship ship, Island island, Item item, int number)
        {
            if (number <= 0) return TradeResult.InvalidAmount;
            if (!island.IsToBuy(item)) return TradeResult.NotAvailable;
            if (ship.CountOf(item) < number) return TradeResult.NotEnoughItems;
            return TradeResult.Ok;
        }

        public static TradeResult Buy(Ship ship, Island island, Item item, int number)
        {
            TradeResult check = CheckBuy(ship, island, item, number);
            if (check != TradeResult.Ok) return check;

            ship.TrySpendCoins(BuyCost(island, item, number));
            ship.CanAddItem(item, number);
            return TradeResult.Ok;
        }
        public static TradeResult Sell(Ship ship, Island island, Item item, int number)
        {
            TradeResult check = CheckSell(ship, island, item, number);
            if (check != TradeResult.Ok) return check;

            ship.CanRemoveItem(item, number);
            ship.AddCoins(SellIncome(island, item, number));
            return TradeResult.Ok;
        }

        public static int MaxCanBuy(Ship ship, Island island, Item item)
        {
            if (!island.IsForSale(item)) return 0;
            double price = island.ItemBuyPrice(item);

            int byCoins = (int)Math.Round(ship._coins / price, 0);
            int byWeight = ship.FreeSpace / item.weight;

            return byCoins < byWeight ? byCoins : byWeight;
        }

        public static int MaxCanSell(Ship ship, Island island, Item item) => island.IsToBuy(item) ? ship.CountOf(item) : 0;
    }
}