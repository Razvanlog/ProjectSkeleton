using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using TheAdventure.Camera;
using TheAdventure.Entities.Enemies;
using TheAdventure.Entities.Gun.Projectile;
using TheAdventure.Entities.Player;
using TheAdventure.Entities.Wall;
using TheAdventure.EntityManager;
using TheAdventure.Input.InputKey;
using TheAdventure.Models.Data;

namespace TheAdventure.GameLogic
{
    internal class GameLogic
    {
        private readonly Dictionary<string, TileSet> loadedTileSets = new();
        private readonly Dictionary<int, Tile> tileIdMap = new();
        public EntityManager.EntityManager EntityManager { get; } = new();
        public TheAdventure.Camera.Camera? Camera { get; set; }

        public Player? Player { get; private set; }
        public bool over { get; private set; }

        private int wave = 1;
        private Level? currentLevel;

        public void InitGame(TheAdventure.Camera.Camera Camera)
        {
            this.Camera = Camera;
            Player = new Player(10, 10, 30, 300);
            var wall0 = new Wall(0, 0, 1000, 10);
            var wall1 = new Wall(0, 0, 10, 1000);
            var wall2 = new Wall(1000, 0, 10, 1000);
            var wall3 = new Wall(0, 1000, 1010, 10);
            EntityManager.add(Player);
            EntityManager.add(new BasicEnemy(150, 150, 50, 50, Player));
            EntityManager.add(wall0);
            EntityManager.add(wall1);
            EntityManager.add(wall2);
            EntityManager.add(wall3);
            var levelContent = System.IO.File.ReadAllText(System.IO.Path.Combine("Assets", "ground.tmj"));
            currentLevel = JsonSerializer.Deserialize<Level>(levelContent);

            if (currentLevel == null)
            {
                throw new Exception("no ground");
            }

            foreach (var pieceRef in currentLevel.TileSets)
            {
                var tileSetContent = System.IO.File.ReadAllText(System.IO.Path.Combine("Assets", pieceRef.Source));
                var tileSet = JsonSerializer.Deserialize<TileSet>(tileSetContent);

                if (tileSet == null) continue;

                foreach (var piece in tileSet.Tiles)
                {
                    piece.TextureId = GameRenderer.GameRenderer.LoadTexture(System.IO.Path.Combine("Assets", piece.Image), out _);

                    if (piece.Id.HasValue)
                    {
                        tileIdMap.Add(piece.Id.Value, piece);
                    }
                }

                if (!string.IsNullOrEmpty(tileSet.Name))
                {
                    loadedTileSets.Add(tileSet.Name, tileSet);
                }
            }
        }

        public void RenderAll(float delta, GameRenderer.GameRenderer renderer)
        {
            if (over)
                return;

            var entities = EntityManager.getEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(delta);
            }

            EntityManager.CheckAndRunInteractions();
            EntityManager.CleanDeadEntities();

            if (EntityManager.getEnemyCount() == 0 && Player!=null)
            {
                wave++;
                int toSpawn = wave * 3;
                for (int i = 0; i < toSpawn; i++)
                {
                    EntityManager.add(new BasicEnemy(Random.Shared.Next(900), Random.Shared.Next(900), 50, 50, Player));
                }
            }
            if (Player != null)
            {
                if (Player.isDead())
                    over = true;
                if (Camera != null)
                {
                    Camera.X = (int)Player.X;
                    Camera.Y = (int)Player.Y;
                }
            }
        }

        public void RenderGround(GameRenderer.GameRenderer renderer)
        {
            if (currentLevel == null || currentLevel.Layers == null)
            {
                Console.WriteLine("skipped drawing ground");
                return;
            }
            foreach(var currentLayer in currentLevel.Layers)
            {
                if (currentLayer.Data == null || currentLayer.Width == null || currentLayer.Height == null)
                {
                    Console.WriteLine("skipped drawing ground");
                    continue;
                }

                if (!tileIdMap.TryGetValue(0, out var currentTile))
                {
                    return;
                }
                if (Camera == null)
                {
                    return;
                }
                var tileWidth = currentTile.ImageWidth ?? 92;
                var tileHeight = currentTile.ImageHeight ?? 92;

                var sourceRect = new Silk.NET.Maths.Rectangle<int>(0,0, tileWidth, tileHeight);
                
                
                int tileNumX = (Camera.Width / tileWidth) + 4;
                int tileNumY = (Camera.Height / tileHeight) + 4;
                
                
                int startGridX = (int)Math.Floor((double)Camera.X / tileWidth)-tileNumX/2;
                int startGridY = (int)Math.Floor((double)Camera.Y /  tileHeight)-tileNumY/2;
                
                int endGridX = startGridX + tileNumX;
                int endGridY = startGridY + tileNumY;

                for (int i = startGridX; i < endGridX; i++)
                {
                    for (int j = startGridY; j < endGridY; j++)
                    {
                        var worldRect = new Silk.NET.Maths.Rectangle<int>(i * tileWidth, j * tileHeight, tileWidth, tileHeight);

                        var screenRect = Camera.ToScreenCoordinates(worldRect);

                        var destRect = new Silk.NET.Maths.Rectangle<int>
                            (
                            (int)screenRect.Origin.X,
                            (int)screenRect.Origin.Y,
                            (int)screenRect.Size.X,
                            (int)screenRect.Size.Y
                            );

                        renderer.RenderTexture(currentTile.TextureId, sourceRect, destRect);
                    }
                }
            }
        }
    }
}
