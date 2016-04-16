#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RobsSprite;
using Three_Thing_Game;
#endregion

namespace RobsPhysics
{
    public static class PhysicsManager
    {

        public static List<RigidBody> objs = new List<RigidBody>();
        public static int[,] colliderMap; //Used for efficentcy

        public static void ResolveCollision(RigidBody A, RigidBody B)
        {

            A.Velocity = Vector2.Zero;
            B.Velocity = Vector2.Zero;

            // Calculate relative velocity
            Vector2 rv = B.Velocity - A.Velocity;
            Vector2 normal = B.Position - A.Position;           

            // Calculate relative velocity in terms of the normal direction
            float velAlongNormal = Vector2.Dot(rv, normal);

            /*// Do not resolve if velocities are separating
            if (velAlongNormal > 0)
                return;*/

            // Calculate restitution
            float e = 0.1f;

            // Calculate impulse scalar
            float j = -(1 + e) * velAlongNormal;
            j /= A.inv_mass + B.inv_mass;

            // Apply impulse
            Vector2 impulse = j * normal;
            A.AddForce(-impulse);
            B.AddForce(impulse);

        }

        public static void ResolveCollision(RigidBody rb, Vector2 normal)
        {
            //V = U - (1+e) * Vector2.Dot(N,U) * N
            rb.Velocity -= (1.1f) * Vector2.Dot(normal, rb.Velocity) * normal;
        }

        public static void Step(float deltaTime)
        {           

            foreach (RigidBody rb in objs)
            {

                Player person = rb as Player;
                //Add Gravity
                if(person == null || person.isFalling)
                    rb.AddForce(new Vector2(0, 10f));

                rb.colliding = false;

                foreach (RigidBody rbo in objs)
                {
                    if (rbo != rb && rb.checkCollision(rbo))
                    {
                        rb.colliding = true;
                        rb.collidingWith = rbo;
                        ResolveCollision(rb, rbo);
                    }
                }

                if (rb.Mass != 0)
                {

                    Vector2 currentVelocity = rb.Velocity;

                    rb.Velocity += (rb.Force / rb.Mass) * deltaTime;

                    if (rb.Velocity.Length() > rb.terminalVelocity)
                        rb.Velocity = currentVelocity;

                    rb.Force = new Vector2(0, 0);

                    Vector2 lastPos = rb.Position;

                    rb.Position += rb.Velocity * deltaTime;

                    #region Collisions
                    //Do Map Collisions
                    int checkSize = 3;
                    for (int y = -checkSize; y < checkSize; y++)
                    {
                        for (int x = -checkSize; x < checkSize; x++)
                        {
                            int xPos = (int)rb.Position.X + x;
                            int yPos = (int)rb.Position.Y + y;

                            int width = colliderMap.GetLength(1);
                            int height = colliderMap.GetLength(0);                           

                            //Check that block is inside of Check Range
                            if (xPos >= 0 && xPos < width && yPos >= 0 && yPos < height)
                            {
                                if (colliderMap[yPos, xPos] > 0)
                                {

                                    bool collision = false;
                                    bool pushX = false, pushY = false;

                                    float myX = rb.Position.X + 1f, myY = rb.Position.Y;
                                    //Top Of Block
                                    if (xPos >= myX && xPos <= myX + 1 && myY < yPos && myY + 1.9f > yPos)
                                    {
                                        pushY = true;

                                        person.isFalling = false;
                                        rb.Velocity = new Vector2(rb.Velocity.X, 0);

                                        collision = true;                                       
                                    }

                                    //Right Side of Block
                                    if (yPos > myY - 1 && yPos < myY + 1 && myX > xPos && myY < xPos + 1f)
                                    {
                                        pushX = true;
                                        collision = true;
                                    }
                                    

                                    if (collision)
                                    {


                                        switch (colliderMap[yPos, xPos])
                                        {
                                            case 1: //Collision

                                                if (pushY)
                                                    rb.Position = new Vector2(rb.Position.X, lastPos.Y);
                                                if (pushX)
                                                    rb.Position = new Vector2(lastPos.X, rb.Position.Y);

                                                break;
                                            case 2: //Health
                                                break;
                                            case 3: //Money
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    


                }

            }

        }

        public static bool BoxIntersection(Rectangle a, Rectangle b)
        {
            return (Math.Abs(a.X - b.X) * 2 < (a.Width + b.Width)) &&
                   (Math.Abs(a.Y - b.Y) * 2 < (a.Height + b.Height));
        }
        public static void AddObj(RigidBody rbody)
        {
            objs.Add(rbody);
        }

    }

    public class RigidBody : Sprite
    {

        public float Mass;
        public float inv_mass;

        public Vector2 Velocity;
        public Vector2 Force;

        public float terminalVelocity = 15f;

        public bool colliding;
        public RigidBody collidingWith;

        public RigidBody(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal, float mass, float maxSpeed)
            : base(textureVal, pos, widthVal, heightVal)
        {
            Mass = mass;

            if (mass == 0)
                inv_mass = 0;
            else
                inv_mass = 1 / mass;

            Velocity = new Vector2(0, 0);
            Force = new Vector2(0, 0);
            terminalVelocity = maxSpeed;
            colliding = false;

            PhysicsManager.AddObj(this);

        }

        public void AddForce(Vector2 dir)
        {
            Force += dir;
        }

        public bool checkCollision(RigidBody other)
        {

            Vector2 myCente = Position;
            Vector2 thereCente = other.Position;

            Vector2 normal = thereCente - myCente;

            if (normal.Length() < Width / 2)
                return true;

            return false;
        }

    }
}
