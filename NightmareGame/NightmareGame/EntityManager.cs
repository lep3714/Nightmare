using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

/// <summary>
/// AUTHOR: Robley Evans
/// PURPOSE: The manager to the game that draws and updates
/// all of the items and entities in the game
/// </summary>
namespace NightmareGame
{
    enum HitBoxState
    {
        Yes,
        No
    }

    class EntityManager
    {
        // FIELDS
        private List<GameObject> objects;
        private List<GameObject> objectsInDrawOrder;
        private Player p;
        private Game1 game;
        private Random rng;
        private InsanityBar insane;
        private KeyboardState kbState;
        private KeyboardState lastKbState;
        private HitBoxState hitBoxState;

        // PROPERTIES
        // none yet

        // CONSTRUCTOR
        public EntityManager(Game1 game, InsanityBar insane)
        {
            hitBoxState = HitBoxState.No;
            objects = new List<GameObject>();
            objectsInDrawOrder = new List<GameObject>();
            this.game = game;
            this.insane = insane;
            p = game.Player;

            for (int i = 0; i < game.TreeTiles.Count; i++)
            {
                objects.Add(game.TreeTiles[i]);
                objectsInDrawOrder.Add(game.TreeTiles[i]);
            }

            for (int i = 0; i < game.EnemyTiles.Count; i++)
            {
                objects.Add(game.EnemyTiles[i]);
                objectsInDrawOrder.Add(game.EnemyTiles[i]);
            }

            for (int i = 0; i < game.BoundaryTiles.Count; i++)
            {
                objects.Add(game.BoundaryTiles[i]);
                objectsInDrawOrder.Add(game.BoundaryTiles[i]);
            }

            //objects.Add(game.Rug);
            //objectsInDrawOrder.Add(game.Rug);

            objects.Add(p);
            objectsInDrawOrder.Add(p);
            rng = new Random();
        }

        // METHODS
        /// <summary>
        /// Method that does all the math for the objects and their stuff
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // TOGGLING HITBOXES FOR ERIN MODE
            kbState = Keyboard.GetState();


            if (kbState.IsKeyDown(Keys.H) && kbState != lastKbState)
            {
                if (hitBoxState == HitBoxState.Yes)
                {
                    hitBoxState = HitBoxState.No;
                }
                else
                {
                    hitBoxState = HitBoxState.Yes;
                    game.Insanity.InsanityNum = 0;
                }
            }

            lastKbState = kbState;
            // loop to update the entities on the map
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is Enemy)
                {
                    Enemy e = (Enemy)objects[i];

                    e.UpdateAnimation(gameTime);
                    if(e.SearchPlayer())
                    {
                        e.BearDirectionCheck();
                    }

                    insane.RaiseInsanityBar(e.AttackPlayer());
                    insane.ChangeImage();
                    RemoveEnemy(e);
                    e.PrevX = e.X;
                    e.PrevY = e.Y;
                }
                else if (objects[i] is MapObject)
                {
                    MapObject mo = (MapObject)objects[i];
                    for (int j = 0; j < objects.Count; j++)
                    {
                        if (objects[j] is Enemy || objects[j] is Player)
                        {
                            mo.CheckCollision(objects[j]);
                        }
                    }
                }
            }

            objectsInDrawOrder.Sort((x, y) => x.HitBoxY.CompareTo(y.HitBoxY));
        }

        /// <summary>
        /// The draw method that draws all the other objects. This is called in Game1
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (hitBoxState == HitBoxState.No)
            {
                // Loop that will draw all of the objects in order
                for (int i = 0; i < objectsInDrawOrder.Count; i++)
                {
                    objectsInDrawOrder[i].Draw(sb);
                }
            }
            else
            {
                // Loop that will draw all of the objects in order
                for (int i = 0; i < objectsInDrawOrder.Count; i++)
                {
                    sb.Draw(game.HitboxImage, objectsInDrawOrder[i].HitBox, Color.White);
                    objectsInDrawOrder[i].Draw(sb);
                }
            }

        }
        
        /// <summary>
        /// Removes enemies that become inactive when touching the players
        /// </summary>
        /// <param name="e"></param>
        public void RemoveEnemy(Enemy e)
        {
            if (!e.IsActive)
            {
                objects.Remove(e);
                objectsInDrawOrder.Remove(e);
            }
        }

    }
}
