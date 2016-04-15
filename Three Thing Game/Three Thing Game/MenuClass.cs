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
    class MenuClass : GameClass
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Sprite background, title, start, options, quit;
        private Texture2D backgroundT, titleT, startT, optionsT, quitT;

        //temp
        private int screenWidth = 800;
        private int screenHeight = 600;
        private int sectX, sectY;

        public MenuClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        { 
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            sectX = screenWidth / 16;
            sectY = screenHeight / 16;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundT = Content.Load<Texture2D>("blackSquare.png");
            titleT = Content.Load<Texture2D>("blueSquare.png");
            background = new Sprite(backgroundT, new Vector2(8*sectX, 0), screenWidth, screenWidth); //GameClass.  
            title = new Sprite(backgroundT, new Vector2(0, 0), screenWidth, screenWidth);
       
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

    }
}
