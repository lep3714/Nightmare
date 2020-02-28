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
    //objective tea party class that will be used for our main objective for the game
    //will do all the calculations into the game.
    class TeaParty : Objective
    {

        //three fields that will be used in the objective.
        //They are all objects that will be seen in the game
        private ObjectiveObject teaKettle;

        private ObjectiveObject teaCups;

        private ObjectiveObject plate;

        private ObjectiveObject rug;

        private List<Point> initialObjSpawn;
        private List<Point> objSpawn;
        private Random rand = new Random();
        private int spawnIndex;
        Game1 game;

        private InsanityBar insanity;

        int numberOfCompletions;

        //a boolean that can be changed later on in the code
        //to check to see if the objective is active at this moment
        public override bool Active
        {
            get { return active; }
            set
            {
                if (value != active)
                {
                    active = value;
                }
            }
        }

        //parameterized constructor that will set th efields to objects that are inputed into the constructor
        public TeaParty(ObjectiveObject vTeaKettle, ObjectiveObject vTeaCups, ObjectiveObject vPlate,
            List<Point> initialObjSpawn, Game1 game, ObjectiveObject rug, InsanityBar vInsanity)
            : base()
        {
            teaKettle = vTeaKettle;
            teaCups = vTeaCups;
            plate = vPlate;
            this.rug = rug;
            numberOfCompletions = 0;

            this.initialObjSpawn = initialObjSpawn;
            this.game = game;
            objSpawn = new List<Point>();

            insanity = vInsanity;
        }


        //will check the x and y values of each item and check to see the difference between them is
        //between -50 units and 50 units. If so then the items are in close enough proximity to each other
        //this will make all the objective objects disapear and return an integer to lower the insanity bar
        //can be completed 3 times, but once it has been completed three times the game is won
        public override bool Solution()
        {
            if (numberOfCompletions < 3)
            {
                if (active == true)
                {
                    //will check to see if all the objects are intersecting
                    //with the rug object if so complete part of the objective
                    if (rug.Position.Intersects(teaKettle.Position) &&
                        rug.Position.Intersects(teaCups.Position) &&
                        rug.Position.Intersects(plate.Position))
                    {
                        //sets the objective to not active so it cannot be completed again
                        active = false;


                        teaCups.Position = new Rectangle(0, 0, 0, 0);
                        teaKettle.Position = new Rectangle(0, 0, 0, 0);
                        plate.Position = new Rectangle(0, 0, 0, 0);
                        //will up the number of completions and reset the solution
                        numberOfCompletions++;
                        ResetSolution();

                        insanity.DecreaseInsanityBar(40);
                    }


                }
            }
            else if (numberOfCompletions == 3)
            {
                return true;
            }

            return false;
        }

        //will reset the objective so it can be completed again
        //the objects will be placed back at their starting positions.
        public override void Reset()
        {
            active = true;

            // Copys the initial list to the working list
            objSpawn = initialObjSpawn;

            // For each of the three objectives, takes a random number from the working list's count.
            //      The object is placed at the point found at objSpawn[random number]. The index
            //      is then removed from the list so that objects are not place on top of eachother.
            spawnIndex = rand.Next(objSpawn.Count);
            teaCups.Position = new Rectangle(game.Origin.X + objSpawn[spawnIndex].X,
                game.Origin.Y + objSpawn[spawnIndex].Y, 25, 25);
            objSpawn.RemoveAt(spawnIndex);

            spawnIndex = rand.Next(objSpawn.Count);
            teaKettle.Position = new Rectangle(game.Origin.X + objSpawn[spawnIndex].X,
                game.Origin.Y + objSpawn[spawnIndex].Y, 25, 25);
            objSpawn.RemoveAt(spawnIndex);

            spawnIndex = rand.Next(objSpawn.Count);
            plate.Position = new Rectangle(game.Origin.X + objSpawn[spawnIndex].X,
                game.Origin.Y + objSpawn[spawnIndex].Y, 25, 25);
            objSpawn.RemoveAt(spawnIndex);

            numberOfCompletions = 0;
        }

        //won't reset the objective fully only used 
        //when the solution hasn't been done 3 times
        //resets the objects so they can be picked
        //up again
        public void ResetSolution()
        {
            active = true;
            
            // Resets the working list to the initial list by clearing it and adding each point from
            //      the initial list to the working list
            objSpawn.Clear();
            objSpawn = new List<Point>();
            for (int i = 0; i < initialObjSpawn.Count; i++)
            {
                objSpawn.Add(initialObjSpawn[i]);
            }

            // For each of the three objectives, takes a random number from the working list's count.
            //      The object is placed at the point found at objSpawn[random number]. The index
            //      is then removed from the list so that objects are not place on top of eachother.
            spawnIndex = rand.Next(objSpawn.Count);
            teaCups.Position = new Rectangle((game.Origin.X + objSpawn[spawnIndex].X),
                (game.Origin.Y + objSpawn[spawnIndex].Y), 25, 25);
            objSpawn.RemoveAt(spawnIndex);

            spawnIndex = rand.Next(objSpawn.Count);
            teaKettle.Position = new Rectangle((game.Origin.X + objSpawn[spawnIndex].X),
                (game.Origin.Y + objSpawn[spawnIndex].Y), 25, 25);
            objSpawn.RemoveAt(spawnIndex);

            spawnIndex = rand.Next(objSpawn.Count);
            plate.Position = new Rectangle((game.Origin.X + objSpawn[spawnIndex].X),
                (game.Origin.Y + objSpawn[spawnIndex].Y), 25, 25);
            objSpawn.RemoveAt(spawnIndex);
        }
    }
}
