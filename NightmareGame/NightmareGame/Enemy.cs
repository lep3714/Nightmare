using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// AUTHOR: Robley Evans
/// PURPOSE: The class that all the enemies in the game 
/// will use or inherit from. Since we only have one at
/// the current time, we have stuff that wouldn't be defined
/// yet in there.
/// </summary>
namespace NightmareGame
{
    public enum DirectionBear
    {
        FaceRight,
        WalkRight,
        FaceLeft,
        WalkLeft,
        FaceUp,
        WalkUp,
        FaceDown,
        WalkDown
    }
    public class Enemy : GameObject
    {
        // FIELDS
        private bool isActive;
        private Player p;
        private const int aggroRange = 325;
        private Texture2D hitboxImage;
        
        //Animation fields
        private int frame;
        private double timeCounter;
        private double fps = 10.0;
        private double timePerFrame;
        private DirectionBear direction;
        private const int WalkFrameCount = 3;
        private int prevX;
        private int prevY;
        private Game1 game;


        // PROPERTIES
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DirectionBear Direction
        {
            get { return direction; }
        }
        public double Frame
        {
            get { return frame; }
        }
        public int PrevX
        {
            get { return prevX; }
            set { prevX = value; }
        }
        public int PrevY
        {
            get { return prevY; }
            set { prevY = value; }
        }

        /// <summary>
        /// Finds the distance between the enemy and the player.
        /// </summary>
        public double DistanceToPlayer
        {
            get { return Math.Sqrt(
                Math.Pow(this.X - p.HitBoxX, 2) + Math.Pow(this.Y - p.HitBoxY, 2)
                ); }
        }

        // CONSTRUCTOR
        public Enemy(Texture2D[] textures, int x, int y, Player p, Game1 game)
            : base(x, y, textures)
        {
            isActive = true;
            this.p = p;
            speed = 3;
            hitboxImage = textures[2];
            direction = DirectionBear.FaceDown;
            prevX = this.X;
            prevY = this.Y;
            timePerFrame = 1.0 / fps;
            position = new Rectangle(x, y, 50, 100);
            hitBoxX = x;
            hitBoxY = y + (100 / 4 * 3);
            hitBox = new Rectangle(hitBoxX, hitBoxY, position.Width, position.Height / 4);
            this.game = game;
        }

        // METHODS
        /// <summary>
        /// Checks to see if something collides with the enemy. Will be used in the attack 
        /// method to make sure the player gets hit and will be used to see if the enemy hits 
        /// any map objects.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public override bool CheckCollision(GameObject go)
        {
            if (hitBox.Intersects(go.HitBox))
            {
                OverrideMovement(this, go);
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (game.Insanity.CheckInsanityBar() == 1)
            {
                switch (direction)
                {
                    case DirectionBear.FaceDown:
                        sb.Draw(textures[0], position, new Rectangle(100, 1000, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.WalkDown:
                        sb.Draw(textures[0], position, new Rectangle((50 * frame) + 100, 1000, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.FaceLeft:
                        sb.Draw(textures[0], position, new Rectangle(100, 1300, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                        break;
                    case DirectionBear.WalkLeft:
                        sb.Draw(textures[0], position, new Rectangle((frame * 50) + 100, 1300, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                        break;
                    case DirectionBear.FaceUp:
                        sb.Draw(textures[0], position, new Rectangle(100, 1150, 45, 85), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.WalkUp:
                        sb.Draw(textures[0], position, new Rectangle((frame * 50) + 100, 1150, 45, 85), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.FaceRight:
                        sb.Draw(textures[0], position, new Rectangle(100, 1300, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.WalkRight:
                        sb.Draw(textures[0], position, new Rectangle((frame * 50) + 100, 1300, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case DirectionBear.FaceDown:
                        sb.Draw(textures[0], position, new Rectangle(100, 550, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.WalkDown:
                        sb.Draw(textures[0], position, new Rectangle((50 * frame) + 100, 550, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.FaceLeft:
                        sb.Draw(textures[0], position, new Rectangle(100, 850, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                        break;
                    case DirectionBear.WalkLeft:
                        sb.Draw(textures[0], position, new Rectangle((frame * 50) + 100, 850, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                        break;
                    case DirectionBear.FaceUp:
                        sb.Draw(textures[0], position, new Rectangle(100, 700, 45, 85), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.WalkUp:
                        sb.Draw(textures[0], position, new Rectangle((frame * 50) + 100, 700, 45, 85), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.FaceRight:
                        sb.Draw(textures[0], position, new Rectangle(100, 850, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                    case DirectionBear.WalkRight:
                        sb.Draw(textures[0], position, new Rectangle((frame * 50) + 100, 850, 45, 90), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        break;
                }
            }
            
                
        }

        /// <summary>
        /// Searchs for the player within the aggro range. If found, it activates the 
        /// AttackPlayer() method.
        /// </summary>
        public bool SearchPlayer()
        {
            if (DistanceToPlayer <= aggroRange)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// If the SearchPlayer() method activates, then this method will keep the enemy 
        /// moving toward the player until the player is out of the aggro range or until 
        /// the enemy hits the player. When the enemy hits, it raises the insanity of the 
        /// player and then the enemy deactivates.
        /// </summary>
        public int AttackPlayer()
        {
            if (!CheckCollision(p) && SearchPlayer())               //2D
            {
                if (p.X >= this.X)
                {
                    this.X += speed;
                    this.HitBoxX += speed;
                }
                else
                {
                    this.X -= speed;
                    this.HitBoxX -= speed;
                }

                if (p.Y >= this.Y)
                {
                    this.Y += speed;
                    this.HitBoxY += speed;
                }
                else
                {
                    this.Y -= speed;
                    this.HitBoxY -= speed;
                }

                
            }

            if (CheckCollision(p) && SearchPlayer())
            {

                this.IsActive = false;
                return 20;
            }


            return 0;
        }
        //Animation
        public void UpdateAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;

                if (frame > WalkFrameCount)
                    frame = 1;

                timeCounter -= timePerFrame;
            }
        }
        //To see what direction state the Bears are in
        public void BearDirectionCheck()
        {
            switch (this.direction)
            {
                //For is the character is Facing Down
                case DirectionBear.FaceDown:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        this.direction = DirectionBear.FaceDown;
                    }
                    break;
                //For is the character is walking down
                case DirectionBear.WalkDown:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && prevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        this.direction = DirectionBear.FaceDown;
                    }
                    break;
                //For is the character is facing left
                case DirectionBear.FaceLeft:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        this.direction = DirectionBear.FaceLeft;
                    }
                    break;
                //For is the character is walking Left
                case DirectionBear.WalkLeft:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        direction = DirectionBear.FaceLeft;
                    }
                    break;
                //For is the character is facing up
                case DirectionBear.FaceUp:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        this.direction = DirectionBear.FaceUp;
                    }
                    break;
                //For is the character is walking up
                case DirectionBear.WalkUp:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        this.direction = DirectionBear.FaceUp;
                    }
                    break;
                //For is the character is facing right
                case DirectionBear.FaceRight:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        this.direction = DirectionBear.FaceRight;
                    }
                    break;
                //For is the character is walking right
                case DirectionBear.WalkRight:
                    if (this.PrevY > this.Y)
                    {
                        this.direction = DirectionBear.WalkDown;
                    }
                    else if (this.PrevX < this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkLeft;
                    }
                    else if (this.PrevY < this.Y)
                    {
                        this.direction = DirectionBear.WalkUp;
                    }
                    else if (this.PrevX > this.X && this.PrevY == this.Y)
                    {
                        this.direction = DirectionBear.WalkRight;
                    }
                    else
                    {
                        this.direction = DirectionBear.FaceRight;
                    }
                    break;
            }
        }
        public void OverrideMovement(Enemy e, GameObject collidedObject)
        {
            if (e.HitBoxX < collidedObject.HitBoxX + (collidedObject.HitBox.Width / 2))
            {
                // when enemy is touching the tree from the right, cant move left
                e.X -= 0;
                e.HitBoxX -= 0;
            }
            else
            {
                // moves regularly with the enemy
                e.X += e.Speed;
                e.HitBoxX += e.Speed;
            }

            if (e.HitBoxX > collidedObject.HitBoxX + (collidedObject.HitBox.Width / 2))
            {
                // when enemy is touching the tree from the left, cant move right
                e.X -= 0;
                e.HitBoxX -= 0;
            }
            else
            {
                // moves regularly with the enemy
                e.X -= e.Speed;
                e.HitBoxX -= e.Speed;
            }

            if (e.HitBoxY < collidedObject.HitBoxY + (collidedObject.HitBox.Height / 2))
            {
                // when enemy is touching the tree from the bottom, cant move up
                e.Y -= 0;
                e.HitBoxY -= 0;
            }
            else
            {
                // moves regularly with the enemy
                e.Y += e.Speed;
                e.HitBoxY += e.Speed;
            }

            if (e.HitBoxY > collidedObject.HitBoxY + (collidedObject.HitBox.Height / 2))
            {
                // when enemy is touching the tree from the bottom, cant move up
                e.Y -= 0;
                e.HitBoxY -= 0;
            }
            else
            {
                // moves regularly with the enemy
                e.Y -= e.Speed;
                e.HitBoxY -= e.Speed;
            }
        }
    }
}
