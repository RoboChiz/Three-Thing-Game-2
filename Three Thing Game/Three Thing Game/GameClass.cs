using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using RobsPhysics;
using RobsSprite;
using System;

namespace Three_Thing_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameClass : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        Player player;
        List<Enemy> enemies;

        Texture2D blockTexture, playerTexture, heartTexture ,heartETexture, flashTexture, coinTexture;
        int[,] map;
        List<Sprite> mapSprites;

        List<Sprite> hearts;
        MapHandler myMap;

        public GameClass()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(new Vector2(315, 175), 4);

            //Load the Level
            map = new int[,] { 
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1 }, 
            { 1, 0, 0, 2, 3, 0, 0, 0, 1, 0, 1 }, 
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }};

            mapSprites = new List<Sprite>();

            myMap = new MapHandler();

            map = myMap.Map;
            player = new Player(myMap.getFree(), 2, 2);

            //player = new Player(new Vector2(1,0), 2, 2);
            enemies = new List<Enemy>();

            player.pHealth = 3;
            for (int col = 0; col < map.GetLength(0); col++)
            {
                for (int row = 0; row < map.GetLength(1); row++)
                {
                    if (map[col, row] > 0)
                    {
                        mapSprites.Add(new Sprite(null, new Vector2(row, col), 1, 1));
                    }
                }
            }

            PhysicsManager.colliderMap = map;

            Random randMonsterPosition = new Random();
            for (int i = 0; i < 50; i++)
            {
                enemies.Add(new Lurker(myMap.getFreeMonster(randMonsterPosition), player));
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blockTexture = Content.Load<Texture2D>("TestBlock");
            playerTexture = Content.Load<Texture2D>("Professor");
            heartTexture = Content.Load<Texture2D>("heart");
            heartETexture = Content.Load<Texture2D>("heart_Empty");
            flashTexture = Content.Load<Texture2D>("Flash");
            coinTexture = Content.Load<Texture2D>("coin");

            foreach (Sprite sprite in mapSprites)
            {
                sprite.spriteTexture = blockTexture;
            }

            player.spriteTexture = playerTexture;
            player.collideTexture = flashTexture;

            player.collideWidth = 0.5f;
            player.collideHeight = 1.8f;

            foreach (Enemy enemy in enemies)
            {
                enemy.spriteTexture = blockTexture;
            }

            hearts = new List<Sprite>();
            for (int i = 0; i < player.health; i++)
            {
                hearts.Add(new Sprite(heartTexture, new Vector2(20 + (40 * i), 30), 30, 30));
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (gameTime.ElapsedGameTime.Milliseconds / 1000f);

            camera._pos = Vector2.Lerp(camera._pos, player.Position * 10f, deltaTime * 2f);

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = enemies[i];

                e.Update(deltaTime);

                if (e.health <= 0)
                {
                    enemies.Remove(e);
                    continue;
                }
            }

            player.Update(deltaTime);

            PhysicsManager.Step(deltaTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var device = graphics.GraphicsDevice;
           
            spriteBatch.Begin(SpriteSortMode.Immediate,
                      BlendState.NonPremultiplied,
                      SamplerState.PointWrap,
                      null,
                      null,
                      null,
                      camera.get_transformation(device));

            foreach (Sprite sprite in mapSprites)
            {
                if (Vector2.Distance(sprite.Position, player.Position) < 25)
                    sprite.Draw(spriteBatch);
            }

            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            for (int i = 0; i < 3; i++)
            {
                if (player.pHealth >= (i + 1))
                    hearts[i].spriteTexture = heartTexture;
                else
                    hearts[i].spriteTexture = heartETexture;

                hearts[i].DrawNoRotCentre(spriteBatch);


            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
