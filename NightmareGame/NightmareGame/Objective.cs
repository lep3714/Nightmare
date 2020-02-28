using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareGame
{
    //abstact class that creates the outlines for all the objectives to come
    abstract class Objective
    {
        //a field to check if the objective is true at the moment
        protected bool active;

        //starting of a property for active
        public abstract bool Active
        {
            get;
            set;
        }

        //default constructor that will start with active to be true
        protected Objective()
        {
            active = true;
        }

        //checks to see if all the objects are in proximity of each other
        //will return an int and if completed should make all the objects disappear
        public abstract bool Solution();

        //will reset the objective so it can be played again
        public abstract void Reset();
    }
}
