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

                    //Sort Player Stuff which dosen't Change
                    float playerWidth = rb.collideWidth, playerHeight = rb.collideHeight;

                    float halfWidth = (playerWidth / 2f), halfHeight = (playerHeight / 2f);
                    //Get top left corner as position
                    float actualX = (rb.Position.X + rb.width) - halfWidth;
                    float actualY = (rb.Position.Y + (rb.height - playerHeight));

                    int checkSize = 3;
                    for (int y = -checkSize; y < checkSize; y++)
                    {
                        for (int x = -checkSize; x < checkSize; x++)
                        {
                            int xPos = (int)actualX + x;
                            int yPos = (int)actualY + y;

                            int width = colliderMap.GetLength(1);
                            int height = colliderMap.GetLength(0);                           

                            //Check that block is inside of Check Range
                            if (xPos >= 0 && xPos < width && yPos >= 0 && yPos < height)
                            {
                                if (colliderMap[yPos, xPos] > 0)
                                {
                                    //Right Sides
                                    if (actualY + playerHeight > yPos + 0.005f && actualY < yPos + 1 && actualX < xPos + 1 && actualX + playerWidth > xPos)
                                    {
                                        Console.WriteLine("Right Collision with " + xPos + "," + yPos);
                                        DoCollision(xPos, yPos, rb, lastPos, false, true);
                                        actualX = (rb.Position.X + 1) - halfWidth;
                                        actualY = (rb.Position.Y + 1) - halfHeight;

                                    }

                                    //Left Sides
                                    if (actualY + playerHeight > yPos + 0.005f && actualY < yPos + 1 && actualX + playerWidth > xPos && actualX < xPos)
                                    {
                                        Console.WriteLine("Left Collision with " + xPos + "," + yPos);
                                        DoCollision(xPos, yPos, rb, lastPos, false, true);
                                        actualX = (rb.Position.X + 1) - halfWidth;
                                        actualY = (rb.Position.Y + 1) - halfHeight;
                                    }

                                    //Floor
                                    if (actualX + playerWidth > xPos && actualX < xPos + 1 && actualY + playerHeight >= yPos && actualY + playerHeight <= yPos + 1)
                                    {
                                        DoCollision(xPos, yPos, rb, lastPos, true, false);
                                        actualX = (rb.Position.X + 1) - halfWidth;
                                        actualY = (rb.Position.Y + 1) - halfHeight;

                                        person.isFalling = false;
                                        person.Velocity = new Vector2(rb.Velocity.X, 0);

                                        Console.WriteLine("Floor Collision with " + xPos + "," + yPos);
                                    }

                                    //Roof
                                    if (actualX + playerWidth > xPos && actualX < xPos + 1 && actualY < yPos + 1 && actualY > yPos)
                                    {
                                        DoCollision(xPos, yPos, rb, lastPos, true, false);
                                        actualX = (rb.Position.X + 1) - halfWidth;
                                        actualY = (rb.Position.Y + 1) - halfHeight;

                                        Console.WriteLine("Roof Collision with " + xPos + "," + yPos);
                                    }

                                }
                            }
                        }
                    }
                    #endregion

                }

            }

        }

        public static void DoCollision(int xPos, int yPos, RigidBody rb, Vector2 lastPos, bool pushY, bool pushX)
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
        public float collideWidth, collideHeight;

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
