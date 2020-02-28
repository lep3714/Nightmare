using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NightmareGame
{
    //class that will take the abstract class game object and
    //create objects that will be used for objectives specifically
    public class ObjectiveObject : GameObject
    {
        //field to check if the objects have been picked up at this very moment
        private bool pickedUp;

        //property to change if the object is being picked up by the character
        public bool PickedUp
        {
            get { return pickedUp; }
            set { pickedUp = value; }
        }

        public Texture2D[] Textures
        {
            get { return textures; }
        }

        //parameterized constructer that should only pass 
        public ObjectiveObject(int x, int y, Texture2D[] textures)
            : base(x, y, textures)
        {
            position = new Rectangle(x, y, 25, 25);
            pickedUp = false;
        }

        //needed to complete inheritence but will not be used
        public override bool CheckCollision(GameObject obj)
        {
            return false;
        }

        //will draw the objective object to the game.
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(textures[0], position, Color.White);
        }

    }
}
