using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace NightmareGame
{
    /// <summary>
    /// AUTHOR: Robley Evans
    /// PUROPSE: An abstract class that most of the objects inheirit from
    /// so that they can interact with the player and other objects correctly
    /// </summary>
    public abstract class GameObject
    {
        // FIELDS
        protected Rectangle position; //sprite
        protected Rectangle hitBox; // half of half of sprite height
        protected Texture2D[] textures; //spritesheet
        protected int hitBoxX;
        protected int hitBoxY;
        protected int speed = 0;

        // PROPERTIES
        /// <summary>
        /// Uses the X properties to tell other areas the position of the 
        /// object's X coordinate.
        /// </summary>
        public int X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        /// <summary>
        /// Uses the Y properties to tell other areas the position of the 
        /// object's Y coordinate.
        /// </summary>
        public int Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        /// <summary>
        /// Gives the hitbox x value of the rectangle
        /// </summary>
        public int HitBoxX
        {
            get { return hitBox.X; }
            set { hitBox.X = value; }
        }

        public Texture2D[] Textures
        {
            get { return textures; }
        }

        /// <summary>
        /// Gives the hitbox y value of the rectangle
        /// </summary>
        public int HitBoxY
        {
            get { return hitBox.Y; }
            set { hitBox.Y = value; }
        }
        
        /// <summary>
        /// Allows other areas of the program to see the objects position
        /// </summary>
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Shows other areas of the code the speed of the object
        /// </summary>
        public int Speed
        {
            get { return speed; }
        }

        //will return the position of the hitbox
        public Rectangle HitBox
        {
            get { return hitBox; }
            set { hitBox = value; }
        }

        // CONSTRUCTOR
        public GameObject(int x, int y, Texture2D[] textures)
        {
            this.textures = textures;
            position = new Rectangle(x, y, textures[0].Width, textures[0].Height);
            hitBoxX = x;
            hitBoxY = y + (textures[0].Height / 4 * 3);
            hitBox = new Rectangle(hitBoxX, hitBoxY, position.Width, position.Height / 4);
        }

        // METHODS
        /// <summary>
        /// Takes in a game object "go" and checks to see if the object is 
        /// colliding with this game object.
        /// </summary>
        /// <param gameobject="go"></param>
        abstract public bool CheckCollision(GameObject go);

        /// <summary>
        /// Uses a draw method that can be called in Game1 so that it 
        /// is easier to draw overall.
        /// </summary>
        abstract public void Draw(SpriteBatch sb);
    }
}
