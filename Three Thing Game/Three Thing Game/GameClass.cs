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
        Player player;

        Texture2D blockTexture, playerTexture;
        int[,] map;
        List<Sprite> mapSprites;
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
            Vector2 location;
            camera = new Camera(new Vector2(315, 175), 3);

            

            //Load the Level
            map = new int[,] { 
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1 }, 
            { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 }, 
            { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1 }, 
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }};

            mapSprites = new List<Sprite>();

            myMap = new MapHandler();
            
           //player = new Player(new Vector2(25, 10), 2, 2);
            player = new Player(myMap.getFree(), 2, 2);

           for (int col = 0; col < myMap.Map.GetLength(0); col++)
           {
               for (int row = 0; row < myMap.Map.GetLength(1); row++)
               {
                   if (myMap.Map[col, row] > 0)
                   {
                       mapSprites.Add(new Sprite(null, new Vector2(row, col), 1, 1));
                   }
               }
           }

           PhysicsManager.colliderMap = myMap.Map;

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

            foreach (Sprite sprite in mapSprites)
            {
                sprite.spriteTexture = blockTexture;
            }

            player.spriteTexture = playerTexture;
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
                      BlendState.AlphaBlend,
                      SamplerState.PointClamp,
                      null,
                      null,
                      null,
                      camera.get_transformation(device));

            foreach (Sprite sprite in mapSprites)
            {
                sprite.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
