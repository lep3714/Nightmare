using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Robley worked on this class

namespace NightmareGame
{
    public class MapObject : GameObject
    {
        // FIELDS
        private Texture2D hitboxImage;

        // PROPERTIES
        // none yet

        // CONSTRUCTOR
        public MapObject(Texture2D[] textures, int x, int y)
            : base(x, y, textures) { hitboxImage = textures[1]; }

        // METHODS
        /// <summary>
        /// Takes in a game object and checks to see if they are 
        /// colliding with this map object (tree, rock, etc).
        /// </summary>
        /// <param name="go"></param>
        public override bool CheckCollision(GameObject go)
        {
            if (hitBox.Intersects(go.HitBox))
            {
                if (go is Enemy)
                {
                    OverrideMovement((Enemy)go, this);
                }
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(textures[0], position, Color.White);
        }
        
        /// <summary>
        /// If an entity hits a map object, the map object will push it back in the oppisite 
        /// direction and the same speed that the entity is going at the map object with so 
        /// that things cant move through the map objects.
        /// </summary>
        /// <param name="go"></param>
        public void OverrideMovement(Enemy e, MapObject collidedObject)
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
