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
        Vector2 position;
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
            float distanceFromPlayer = Vector2.Distance(position, player.Position);

            if (distanceFromPlayer < 5.0f)
            {
                Vector2 dir = Vector2.Normalize(player.Position - this.Position);
                dir.Y = 0;

                Velocity += dir * 2f * deltaTime;
            }
        }

    }
}
