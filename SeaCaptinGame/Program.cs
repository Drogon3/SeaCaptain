using SeaCaptinGame.Core.Controller;
using SeaCaptinGame.Core.Model;
using SeaCaptinGame.Core.View;
using SeaCaptinGame.Views;

namespace SeaCaptinGame;

public class Program
{
    public static void Main()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "map3.txt");
        GameMap gameMap = GameMap.FromFile(path);
        ISeaView view = new ConsoleSeaView();
        new GameController(view, gameMap).Run();
    }
}