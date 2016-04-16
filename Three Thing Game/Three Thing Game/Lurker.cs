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
    class Lurker : Enemy
    {
        Player player;

        enum State
        {
            Idling,
            Moving
        }
        public Lurker(Vector2 _position, Player _player) : base(_position, 1, 1)
        {
            player = _player;
        }

        override public void EnemyAIUpdate(float deltaTime)
        {
            Vector2 dir = player.Position - Position;
            dir.Y = 0;

            if (dir.Length() < 5.0f)
            {
                dir.Normalize();
                Velocity += dir * 2f * deltaTime;
            }
        }

    }
}
