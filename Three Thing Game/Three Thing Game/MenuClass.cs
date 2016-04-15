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
        private int screenWidth = 1280;
        private int screenHeight = 720;
        private int sectX, sectY;
        private bool main;

        private int option;
        private KeyboardState _currentKeyboardState, _previousKeyboardState;
        private GamePadState _currentGamepadState, _previousGamepadState;


        public MenuClass()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            main = true;
            sectX = screenWidth / 16;
            sectY = screenHeight / 16;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundT = Content.Load<Texture2D>("blackSquare");
            titleT = Content.Load<Texture2D>("title");
            startT = Content.Load<Texture2D>("start");
            optionsT = Content.Load<Texture2D>("options");
            quitT = Content.Load<Texture2D>("quit");

            background = new Sprite(backgroundT, new Vector2(0, 0), screenWidth, screenHeight);
            title = new Sprite(titleT, new Vector2(0, -6 * sectY), 6 * sectX, 3 * sectY);
            start = new Sprite(startT, new Vector2(0, -2 * sectY), 4 * sectX, 2 * sectY);
            options = new Sprite(optionsT, new Vector2(0, 2 * sectY), 4 * sectX, 2 * sectY);
            quit = new Sprite(quitT, new Vector2(0, 6 * sectY), 4 * sectX, 2 * sectY);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if ((_currentKeyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up)) || ((_currentKeyboardState.IsKeyDown(Keys.W) && _previousKeyboardState.IsKeyUp(Keys.W))) || (_currentGamepadState.IsButtonDown(Buttons.DPadUp) && _previousGamepadState.IsButtonUp(Buttons.DPadUp)))
            {
                if (main)
                {
                    option -= 1;
                    if (option <= -1) option = 2;
                }
                else
                { //Options

                }
            }
            if ((_currentKeyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down)) || (_currentKeyboardState.IsKeyDown(Keys.S) && _previousKeyboardState.IsKeyUp(Keys.S)) || (_currentGamepadState.IsButtonDown(Buttons.DPadDown) && _previousGamepadState.IsButtonUp(Buttons.DPadDown)))
            {
                if (main)
                {
                    option += 1;
                    if (option >= 3) option = 0;
                }
                else
                {//Options

                }
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (main)
                {
                    if (option == 0)
                    {
                        // TODO - start game
                        using (var game = new GameClass())
                            game.Run();
                        Exit();
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
                else
                { //Options

                }
            }

            _previousKeyboardState = _currentKeyboardState;
            _previousGamepadState = _currentGamepadState;
            start.Colour = Color.White;
            options.Colour = Color.White;
            quit.Colour = Color.White;
            if (option == 0) { start.Colour = Color.Green; }
            if (option == 1) { options.Colour = Color.Green; }
            if (option == 2) { quit.Colour = Color.Green; }
            base.Update(gameTime);
        }

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
            background.DrawNoRot(spriteBatch);
            title.DrawNoRot(spriteBatch);

            start.DrawNoRot(spriteBatch);
            options.DrawNoRot(spriteBatch);
            quit.DrawNoRot(spriteBatch);


            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
