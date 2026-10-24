using SeaCaptinGame.Core.Controller;
using SeaCaptinGame.Core.Model;
using SeaCaptinGame.Core.View;
using SeaCaptinGame.Views;

namespace SeaCaptinGame;

public class Program
{
    public static void Main()
    {
        string pathDir = Path.Combine(AppContext.BaseDirectory, "Data");
        GameMap gameMap = GameMap.FromFile(Path.Combine(pathDir, "map.txt"));
        Dictionary<int, Item> items = Item.LoadFromFile(Path.Combine(pathDir, "Items.json"));
        List<Island> islands = Island.LoadFormFile(Path.Combine(pathDir, "Islands.json"), items);


        ISeaView view = new ConsoleSeaView();
        new GameController(view, gameMap).Run();
    }
}