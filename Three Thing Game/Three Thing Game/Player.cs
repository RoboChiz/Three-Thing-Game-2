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

        public Texture2D collideTexture;

        float currentFrameTime;
        int currentFrame = 0;
        private float playerSpeed = 200f, jumpForce = 450f, attackDistance = 0f, chargeSpeed = 2f;
        public float maxDistance = 3f;
        public bool flipImage;

        public Player(Vector2 pos, int width, int height) : base(null, pos, width, height, 1, 12) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int spriteWidth = (int)(width * mScale);
            int spriteHeight = (int)(height * mScale);

            int spriteX = (int)(Position.X * mScale);
            int spriteY = (int)(Position.Y * mScale);

            Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
            Rectangle sourceRectangle = new Rectangle(35 * currentFrame, 0, 35, 35);

            float actualX = (Position.X + width / 2f) - (collideWidth / 2f);
            float actualY = (Position.Y + (height - collideHeight));

            spriteBatch.Draw(collideTexture, new Rectangle((int)(actualX * mScale), (int)(actualY * mScale), (int)(collideWidth * mScale), (int)(collideHeight * mScale)), sourceRectangle, Color.White, Rotation, Vector2.Zero, SpriteEffects.None, 0);


            if(!flipImage)
                spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, Rotation, Vector2.Zero, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, Rotation, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
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

            if(hori != 0)
            {
                if (currentFrame == 0)
                {
                    currentFrame = 1;
                    currentFrameTime = 1f;
                }

                currentFrameTime += deltaTime * 10;
                currentFrame = (int)currentFrameTime;

                if (currentFrameTime >= 9)
                    currentFrameTime = 1;
            }
            else
            {
                currentFrame = 0;
                currentFrameTime = 0f;
            }

            if(!isFalling && verti)
                AddForce(new Vector2(0,-jumpForce));

            isFalling = true;

            if (hori < 0)
                flipImage = true;
            if (hori > 0)
                flipImage = false;

            //Do Attack
            bool attacking = false;
            if (currentKeyboardState.IsKeyDown(Keys.Enter) || currentKeyboardState.IsKeyDown(Keys.Space))
                attacking = true;

            if (attacking)
            {
                if (attackDistance < 1)
                    attackDistance = 1;
                if (attackDistance > maxDistance)
                    attackDistance = maxDistance;

                attackDistance += deltaTime * chargeSpeed;
            }
            else if(attackDistance > 0)
            {
                attackDistance = 0;
            }

        }

    }
}
