using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using RobsPhysics;
using RobsSprite;

namespace Three_Thing_Game
{
    class Player : RigidBody
    {

        int currentFrame = 0, maxframe = 9;
        private float playerSpeed = 200f, jumpForce = 200f;
        public bool flipImage;
        public bool isFalling = false;

        public Player(Vector2 pos, int width, int height) : base(null, pos, width, height, 1, 60) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int spriteWidth = (int)(Width * mScale);
            int spriteHeight = (int)(Height * mScale);

            int spriteX = (int)(Position.X * mScale);
            int spriteY = (int)(Position.Y * mScale);

            Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
            Rectangle sourceRectangle = new Rectangle(35 * currentFrame, 0, 35, 35);

            Vector2 spriteOrigin = new Vector2((spriteTexture.Width / (float)maxframe) / 2f, spriteTexture.Height / 2f);

            if(!flipImage)
                spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, Rotation, spriteOrigin, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, Rotation, spriteOrigin, SpriteEffects.FlipHorizontally, 0);
        }

        public void Update(float deltaTime)
        {

            var currentKeyboardState = Keyboard.GetState();
            var currentGamepadState = GamePad.GetState(PlayerIndex.One);

            float hori = 0;
            bool verti = false;

            //Left/Right
            if (currentKeyboardState.IsKeyDown(Keys.A))
                hori = -1;
            if (currentKeyboardState.IsKeyDown(Keys.D))
                hori = 1;
            if (currentKeyboardState.IsKeyDown(Keys.Left))
                hori = -1;
            if (currentKeyboardState.IsKeyDown(Keys.Right))
                hori = 1;

            //Jump
            if (currentKeyboardState.IsKeyDown(Keys.W))
                verti = true;
            if (currentKeyboardState.IsKeyDown(Keys.Up))
                verti = true;

            Velocity = new Vector2(playerSpeed * hori * deltaTime, Velocity.Y);

            if(!isFalling && verti)
                AddForce(new Vector2(0,-jumpForce));

            if (hori < 0)
                flipImage = true;
            if (hori > 0)
                flipImage = false;

        }

    }
}
