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

            spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White, Rotation, spriteOrigin, SpriteEffects.None, 0);

        }

        public void Update(float deltaTime)
        {

        }

    }
}
