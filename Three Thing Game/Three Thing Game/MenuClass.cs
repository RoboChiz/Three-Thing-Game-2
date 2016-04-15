using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RobsSprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Three_Thing_Game
{
    class MenuClass : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;

        private Sprite background, title, start, options, quit;
        private Texture2D backgroundT, titleT, startT, optionsT, quitT;

        //temp
        private int screenWidth = 800;
        private int screenHeight = 600;
        private int sectX, sectY;

        private int option;

        public MenuClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            sectX = screenWidth / 16;
            sectY = screenHeight / 16;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundT = Content.Load<Texture2D>("blackSquare");
            titleT = Content.Load<Texture2D>("blueSquare");
            startT = Content.Load<Texture2D>("blueSquare");
            optionsT = Content.Load<Texture2D>("blueSquare");
            quitT = Content.Load<Texture2D>("blueSquare");

            background = new Sprite(backgroundT, new Vector2(0, 0), screenWidth, screenHeight);
            title = new Sprite(titleT, new Vector2(0, -2 * sectY), 4 * sectX, 2* sectY);
            start = new Sprite(startT, new Vector2(0, 2 * sectY), 4 * sectX, (int)(1.5 * sectY));
            options = new Sprite(optionsT, new Vector2(0, 4 * sectY), 4 * sectX, (int)(1.5 * sectY));
            quit = new Sprite(quitT, new Vector2(0, 6 * sectY), 4 * sectX, (int)(1.5 * sectY));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                option -= 1;
                if (option == -1) option = 2;
            }
            else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                option += 1;
                if (option == 2) option = 0;
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (option == 0)
                {
                    // TODO - start game
                }
                else if (option == 1)
                {
                    //TODO - OPTIONS 
                }
                else if (option == 2)
                {
                    Exit();
                }

            }
            base.Update(gameTime);
        }

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
            title.Draw(spriteBatch);

            start.Draw(spriteBatch);
            options.Draw(spriteBatch);
            quit.Draw(spriteBatch);

            background.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
