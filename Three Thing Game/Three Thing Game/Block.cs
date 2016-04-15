using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RobsPhysics;
using RobsSprite;

namespace Three_Thing_Game
{
    class Block : Sprite
    {
         public Block(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal) : base(textureVal,pos,widthVal,heightVal)
         {

         }

    }
}
