using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Marc worked on this class
//Lucas Worked on this class

public enum Direction
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

namespace NightmareGame
{
    //player class that will work with everything for the player such as,
    //drawing animations to the screen, picking up objects, checking which direction
    //the player is looking 
    public class Player : GameObject
    {
        //Animation fields
        private int frame;
        private double timeCounter;
        private double fps = 10.0;
        private double timePerFrame;
        //Source Rect
        private const int WalkFrameCount = 3;

        //private List<Texture2D> playerImages;

        //will be the starting image for the player 
        private Texture2D image;

        private Texture2D hitboxImage;

        //i need this but need help getting this implemented
        //calling the object class
        //private Object obj;

        private Texture2D picture;

        //will be a list of the player holding items
        private List<Texture2D> objImages;

        //will be used to check to see if a button is being clicked
        private KeyboardState kbState;

        //will check to see if an item is already pickedup or not
        private bool objectPickedUp;

        //will be used to cycle through images
        private Direction direction;
        public Direction Direction
        {
            get { return direction; }
        }
        public double Frame
        {
            get { return frame; }
        }

        //letter also to be used in cycling images
        private Keys letter;

        private KeyboardState previousKbState;

        //property to get the image used for the player
        //should be able to change later on when trying to animate
        //and checking to see the character is colliding with any of the trees.
        public Texture2D Image
        {
            get { return image; }
            set
            {
                if (value != image)
                {
                    image = value;
                }
            }
        }

        //test
        public bool ObjectPickedUp
        {
            get { return objectPickedUp; }
            set { objectPickedUp = value; }
        }

        //parameterized constructor to set the two lists and call back to the entity constructor
        public Player(Texture2D vImage, int positionX, int positionY, Texture2D[] vPlayerImages, Texture2D invisible)
            : base(positionX, positionY, vPlayerImages)
        {
            //creating the image lists to hold the images
            objImages = new List<Texture2D>();

            //current image of the player
            image = vImage;
            //hitboxImage = vPlayerImages[1];

            picture = invisible;
            //character is not holding an object to begin with
            objectPickedUp = false;
            

            kbState = Keyboard.GetState();
            letter = Keys.P;
            direction = Direction.FaceDown;
            speed = 4;
            timePerFrame = 1.0 / fps;
        }
        
        //If the player is within a certain distance of an item they
        //can pick it up unless they are already holding something
        //if they are not holding anything and pick something up
        //that item should disappear and appear in a display box 
        //on the bottom right of the screen.
        public void InteractObject(ObjectiveObject item)
        {
            kbState = Keyboard.GetState();

            if (item.Textures[0] != picture)
            {


                if (objectPickedUp == false)
                {
                    //trying to get commmented out code to work
                    //if (kbState.IsKeyDown(Keys.E) && position.Intersects(item.Position) && previousKbState.IsKeyUp(Keys.E))
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E))
                    {
                        item.Position = new Rectangle(0, 0, 0, 0);
                        objectPickedUp = true;
                        item.PickedUp = true;
                    }
                }
                else
                {
                    //trying to get commented code to work
                    //if (kbState.IsKeyDown(Keys.E) && objectPickedUp && previousKbState.IsKeyUp(Keys.E))
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E))
                    {

                        item.Position = new Rectangle(this.X, this.Y + 100, 25, 25);
                        objectPickedUp = false;
                        item.PickedUp = false;
                    }
                }
            }
            previousKbState = Keyboard.GetState();
        }

        //will check to see if the player and any kind of object collide
        public override bool CheckCollision(GameObject obj)
        {
            if (hitBox.Intersects(obj.HitBox) && obj is MapObject)
            {
                MapObject mo = (MapObject)obj;
                //mo.OverrideMovement(this);
                return true;
            }


            return false;
        }
        //object images to go through the player images while holding an object
        public void AddItemImage(Texture2D picture)
        {
            objImages.Add(picture);
        }

        //method to draw the player to the screen
        public override void Draw(SpriteBatch sb)
        {
            switch (direction)
            {
                case Direction.FaceDown:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle(100, 100, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    break;
                case Direction.WalkDown:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle((frame * 50) + 100, 100, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    break;
                case Direction.FaceLeft:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle(100, 100, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    break;
                case Direction.WalkLeft:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle((frame * 50) + 150, 400, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
                    break;
                case Direction.FaceUp:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle(100, 250, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    break;
                case Direction.WalkUp:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle((frame * 50) + 100, 250, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    break;
                case Direction.FaceRight:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle(100, 100, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    break;
                case Direction.WalkRight:
                    sb.Draw(image, new Vector2(400, 240), new Rectangle((frame * 50) + 150, 400, 35, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    break;
            }

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

        //To see what direction state the character is in
        //This will check to see if the player is moving when facing a certain direction
        //when facing one direction this will check to see where the character is walking
        //if the character is not walking this will stop the character and set him to the
        //side that the character was last facing
        //this will always be checking which way the character is currently facing 
        public void DirectionCheck()
        {
            switch (direction)
            {
                //For is the character is Facing Down
                case Direction.FaceDown:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceDown;
                    }
                    break;
                //For is the character is walking down
                case Direction.WalkDown:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceDown;
                    }
                    break;
                //For is the character is facing left
                case Direction.FaceLeft:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceLeft;
                    }
                    break;
                //For is the character is walking Left
                case Direction.WalkLeft:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceLeft;
                    }
                    break;
                //For is the character is facing up
                case Direction.FaceUp:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceUp;
                    }
                    break;
                //For is the character is walking up
                case Direction.WalkUp:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceUp;
                    }
                    break;
                //For is the character is facing right
                case Direction.FaceRight:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceRight;
                    }
                    break;
                //For is the character is walking right
                case Direction.WalkRight:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        direction = Direction.WalkDown;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        direction = Direction.WalkLeft;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        direction = Direction.WalkUp;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        direction = Direction.WalkRight;
                    }
                    else
                    {
                        direction = Direction.FaceRight;
                    }
                    break;
            }
        }
    }
}
