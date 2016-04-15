﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RobsSprite
{
    public class Sprite
    {
        // Asset name of Sprite texture
        public string assetName;

        // Texture of the sprite
        public Texture2D spriteTexture;

        //The size of the Sprite (with scale applied)
        public Rectangle Size;

        public static float X = 0.0f;
        public static float Y = 0.0f;

        public int width;
        public int height;

        //The position of the Sprite
        private Vector2 position = new Vector2(X, Y);

        //The amount to increase/decrease the size of the original sprite. 
        private float mScale = 1f;

        // Rotation of sprite
        public float rotation = 0f;

        // Direction of sprite
        private Vector2 direction;


        private Color colour;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

         public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(spriteTexture.Width * Scale), (int)(spriteTexture.Height * Scale));
            }
        }

        public Vector2 GetCentre()
         {
             return Position + new Vector2(Width/2f, Height/2f);
         }

         public Color Colour
         {
             get { return colour; }
             set { colour = value; }
         }

        public Sprite(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
        {
            spriteTexture = textureVal;
            Position = pos;
            Width = widthVal;
            Height = heightVal;
            colour = Color.White;
        }

        public float FaceDirection(Vector2 dir)
        {
            rotation = (float)(Math.Atan2(-dir.Y, dir.X) + (85 * (Math.PI / 180)));
            return rotation;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            int spriteWidth = (int)(Width);
            int spriteHeight = (int)(Height);

            int spriteX = (int)(Position.X);
            int spriteY = (int)(Position.Y);

            Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);

            Vector2 spriteOrigin = new Vector2(spriteTexture.Width / 2f, spriteTexture.Height / 2f);

            spriteBatch.Draw(spriteTexture, destinationRectangle, null, Color.White, Rotation, spriteOrigin, SpriteEffects.None, 0);

        }

        public void DrawNoRot(SpriteBatch spriteBatch)
        {
            int spriteWidth = (int)(Width);
            int spriteHeight = (int)(Height);
            int spriteX = (int)(Position.X - (spriteWidth / 2f));
            int spriteY = (int)(Position.Y - (spriteHeight / 2f));

            spriteBatch.Draw(spriteTexture, new Rectangle(spriteX , spriteY, spriteWidth, spriteHeight), Colour);
        }

        public void DrawNoRotCentre(SpriteBatch spriteBatch)
        {
            int spriteWidth = (int)(Width);
            int spriteHeight = (int)(Height);
            int spriteX = (int)(Position.X);
            int spriteY = (int)(Position.Y);

            spriteBatch.Draw(spriteTexture, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Colour);
        }

        

    }

        public class Camera
        {
            protected float _zoom; // Camera Zoom
            public Matrix _transform; // Matrix Transform
            public Vector2 _pos; // Camera Position
            protected float _rotation; // Camera Rotation

            public Camera()
            {
                _zoom = 1f;
                _rotation = 0.0f;
                _pos = Vector2.Zero;
            }

            public Camera(Vector2 pos, float zoom)
            {
                _pos = pos;
                _zoom = zoom;
            }


            // Sets and gets zoom
            public float Zoom
            {
                get { return _zoom; }
                set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
            }

            public float Rotation
            {
                get { return _rotation; }
                set { _rotation = value; }
            }

            // Auxiliary function to move the camera
            public void Move(Vector2 amount)
            {
                _pos += amount;
            }
            // Get set position
            public Vector2 Pos
            {
                get { return _pos; }
                set { _pos = value; }
            }

            public Matrix get_transformation(GraphicsDevice graphicsDevice)
            {
                _transform =       // Thanks to o KB o for this solution
                  Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                             Matrix.CreateRotationZ(Rotation) *
                                             Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                             Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
                return _transform;
            }
        }

}
