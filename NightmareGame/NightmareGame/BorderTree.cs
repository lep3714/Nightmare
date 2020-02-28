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
    /// PURPOSE: To make trees on the edge of the map not allow anyone
    /// to get off the map
    /// </summary>
    public class BorderTree : MapObject
    {
        // CONSTURCTOR
        public BorderTree(Texture2D[] textures, int x, int y)
            : base (textures, x, y)
        {
            this.textures = textures;
            hitBoxX = x;
            hitBoxY = y + (textures[0].Height / 2);
            hitBox = new Rectangle(hitBoxX, hitBoxY, position.Width, position.Height / 2);
        }
    }
}
