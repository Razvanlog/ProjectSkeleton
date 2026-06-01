using Silk.NET.SDL;
using System.Diagnostics;
using System.Text.Json;
using TheAdventure.Entities.Enemies;
using TheAdventure.Entities.Gun.Projectile;
using TheAdventure.Entities.Objects.Goal;
using TheAdventure.Entities.Player;
using TheAdventure.Entities.Wall;
using TheAdventure.EntityManager;
using TheAdventure.Input.InputKey;
using TheAdventure.Input.InputLogic;
using TheAdventure.Models.Data;
using TheAdventure.Input;
namespace TheAdventure;

public static class Program
{

    public static bool winner = false;
    public static bool lose = false;
    private static readonly TheAdventure.Camera.Camera camera = new();
    public static int cameraWidth = 800;
    public static int cameraHeight = 600;
    public static int arenaWidth = 1000;
    public static int arenaHeight = 1000;
    public static int score = 0;
    private static readonly Dictionary<string, Tile> loadedTiles = new();
    private static readonly Dictionary<int, Tile> tileIdMap = new();
    public static void Main()
    {
        camera.Width = cameraWidth;
        camera.Height = cameraHeight;
        var sdl = new Sdl(new SdlContext());
        var sdlInitResult = sdl.Init(Sdl.InitVideo | Sdl.InitAudio | Sdl.InitEvents | Sdl.InitTimer |
            Sdl.InitGamecontroller | Sdl.InitJoystick);

        if (sdlInitResult<0)
        {
            throw new InvalidOperationException("failed to init sdl");
        }var inputManager = new InputKey (sdl);

        var gameLogic = new GameLogic.GameLogic();
        var inputLogic = new InputLogic(sdl, gameLogic, inputManager);
        var gameWindow = new GameWindow(sdl, cameraWidth, cameraHeight);
        var gameRenderer = new GameRenderer.GameRenderer(sdl, gameWindow, gameLogic);
        

        gameLogic.InitGame(camera);

        bool quit = false;

        while (!quit)
        {
            gameRenderer.Render();
            quit = inputLogic.ProcessInput();
            if (EntityManager.EntityManager.Instance != null)
            {
                quit = quit || !EntityManager.EntityManager.Instance.PlayerIsAlive();
            }
            if (quit)
            {
                break;
            }
        }
        Console.WriteLine("Congratulations!! Score: " +score);
        gameWindow.Destroy();
        sdl.Quit();
    }
}
