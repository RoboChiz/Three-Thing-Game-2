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
        private float playerSpeed = 200f, jumpForce = 450f, attackDistance = 0f, chargeSpeed = 2f, fadeTime = 0f;

        public float maxDistance = 3f;
        public bool flipImage;
        public int pHealth;

        public Vector4 displayRect;

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

            //spriteBatch.Draw(collideTexture, new Rectangle((int)(actualX * mScale), (int)(actualY * mScale), (int)(collideWidth * mScale), (int)(collideHeight * mScale)), sourceRectangle, Color.White, Rotation, Vector2.Zero, SpriteEffects.None, 0);

            if (!flipImage)
                spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, Rotation, Vector2.Zero, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, Rotation, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            Color flash = new Color(255, 255, 255, (byte)MathHelper.Clamp(fadeTime, 0, 255));

            Rectangle drawRect = new Rectangle((int)(displayRect.X * mScale), (int)(displayRect.Y * mScale), (int)(displayRect.Z * mScale), (int)(displayRect.W * mScale));
            spriteBatch.Draw(collideTexture, drawRect, sourceRectangle, flash, Rotation, Vector2.Zero, SpriteEffects.None, 0);

        }

        public int health
        {
            get { return pHealth; }
            set { width = value; }
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

            if (hori != 0)
            {
                if (currentFrame == 0)
                {
                    currentFrame = 1;
                    currentFrameTime = 1f;
                }

                currentFrameTime += deltaTime * 10f;
                currentFrame = (int)currentFrameTime;

                if (currentFrameTime >= 9)
                    currentFrameTime = 1;
            }
            else
            {
                currentFrame = 0;
                currentFrameTime = 0f;
            }

            if (!isFalling && verti)
                AddForce(new Vector2(0, -jumpForce));

            isFalling = true;

            if (hori < 0)
                flipImage = true;
            if (hori > 0)
                flipImage = false;

            PhysicsManager.playerAttack = Vector4.Zero;

            bool attacking = false;
            if (currentKeyboardState.IsKeyDown(Keys.Enter) || currentKeyboardState.IsKeyDown(Keys.Space))
                attacking = true;

            if (attacking)
            {
                attackDistance += deltaTime * chargeSpeed;

                if (attackDistance < 1)
                    attackDistance = 1;
                if (attackDistance > maxDistance)
                    attackDistance = maxDistance;
            }
            else if(attackDistance > 0)
            {
                Console.WriteLine("ATTACK " + attackDistance);

                fadeTime = 255f;

                float actualX = (Position.X + width / 2f);
                float actualY = (Position.Y + (height - collideHeight));

                if(flipImage)
                    PhysicsManager.playerAttack = new Vector4(actualX - attackDistance, actualY, attackDistance, collideHeight);
                else
                    PhysicsManager.playerAttack = new Vector4(actualX, actualY, attackDistance, collideHeight);

                displayRect = PhysicsManager.playerAttack;
                attackDistance = 0;
            }

            if (fadeTime > 0)
                fadeTime -= deltaTime * 150f;
            else
                fadeTime = 0;
        }

    }
}
