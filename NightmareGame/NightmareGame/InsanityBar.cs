using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Marc worked on this class

namespace NightmareGame
{
    public class InsanityBar 
    {
        //field to hold which interval the insanity is at & to hold the current 
        private int insanityLevel;
        private int insanityNum;

        //starting image for the insanity bar
        private Texture2D image;

        //will hold the images of the insanity bar
        private Texture2D[] imageList;

        private Rectangle position;

        //two properties so other classes can see these two fields
        //however, they cannot change
        public int InsanityLevel
        {
            get { return insanityLevel; }
        }

        public int InsanityNum
        {
            get { return insanityNum; }
            set { insanityNum = value; }
        }


        //parameterized constructor to instantiate all the fields
        public InsanityBar(Texture2D vImage, Texture2D[] vImageList)
        {
            insanityNum = 0;
            insanityLevel = 0;


            image = vImage;

            position = new Rectangle(0, 0, image.Width, image.Height);

            imageList = vImageList;
        }
        public InsanityBar()
        {
            insanityNum = 0;
        }

        //will raise the insanity number based on the int used,
        //when the player gets attacked by an enemy
        public void RaiseInsanityBar(int attack)
        {
            insanityNum += attack;
        }

        //will decrease the insanity bar based on the int inputed, when an
        //objective is completed
        public void DecreaseInsanityBar(int number)
        {
            insanityNum -= number;
            if (insanityNum < 0)
            {
                insanityNum = 0;
            }
        }

        //will change the insanity bar level based on 
        //interval of insanity num. Based on what insanity level
        //the bar is at, will change the images of the teddy bears
        public int CheckInsanityBar()
        {
            if (insanityNum < 50)
            {
                insanityLevel = 1;
                return 1;
            }
            else if (insanityNum >= 50)
            {
                insanityLevel = 2;
                return 2;
            }
            else
            {
                insanityLevel = 0;
                return 0;
            }
        }

        //will change the image based on where the insanity number is at
        //it will set the image to an image in the list
        public void ChangeImage()
        {
            if (insanityNum < 20)
            {
                image = imageList[0];
            }
            else if (insanityNum >= 20 && insanityNum < 40)
            {
                image = imageList[1];
            }
            else if (insanityNum >= 40 && insanityNum < 60)
            {
                image = imageList[2];
            }
            else if (insanityNum >= 60 && insanityNum < 80)
            {
                image = imageList[3];
            }
            else if (insanityNum >= 80 && insanityNum < 100)
            {
                image = imageList[4];
            }
            else if (insanityNum >= 100)
            {
                image = imageList[5];
            }
        }
        
        //draws the insanity bar to the screen
        public void Draw(SpriteBatch sb)
        {
           sb.Draw(image, new Rectangle(250, 10, 300, 25), Color.White);
        }

        //will reset the insanity bar back to zero
        public void ResetInsanityBar()
        {
            insanityNum = 0;
        }
    }
}
