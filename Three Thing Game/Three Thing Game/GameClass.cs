using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using RobsPhysics;
using RobsSprite;

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

        Texture2D block;
        int[,] map;
        List<Sprite> mapSprites;

        public GameClass()
        {
            graphics = new GraphicsDeviceManager(this);
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

            camera = new Camera(new Vector2(0, 0), 50f);

            map = new int[,] { 
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }};

            mapSprites = new List<Sprite>();

            for (int col = 0; col < map.GetLength(0); col++)
            {
                for (int row = 0 ; row < map.GetLength(1); row++)
                {
                    if (map[col, row] > 0)
                    {
                        mapSprites.Add(new Sprite(null, new Vector2(row, col), 1, 1));
                    }
                }
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

            block = Content.Load<Texture2D>("TestBlock");

            foreach (Sprite sprite in mapSprites)
            {
                sprite.spriteTexture = block;
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                      BlendState.AlphaBlend,
                      null,
                      null,
                      null,
                      null,
                      camera.get_transformation(device));

            foreach (Sprite sprite in mapSprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
