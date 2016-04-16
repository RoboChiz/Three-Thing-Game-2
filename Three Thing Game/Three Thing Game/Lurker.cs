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
        public Lurker(Player _player, Vector2 _position) : base(new Vector2(0.0f, 0.0f), 2, 2)
        {
            position = _position;
            player = _player;
        }

        override public void EnemyAIUpdate()
        {
            float distanceFromPlayer = Vector2.Distance(position, player.Position);

            if (distanceFromPlayer < 5.0f)
            {
                Vector2 dir = Vector2.Normalize(player.Position - this.Position);
                Position += dir * 2f;
            }
        }

    }
}
