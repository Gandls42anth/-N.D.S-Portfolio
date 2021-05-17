using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NathanSteelmanUntitledProject
{
    class KeyCondition
    {
        private int goal;
        private int forgiveness;
        private Keys key;
        private int current;
        private KeyCondition basic;
        /// <summary>
        /// The amount of deviation outside of the target range( on the plus side, being below the required range means nothing)
        /// </summary>
        public int Deviation
        {
            get
            {
                if(this.current - (this.goal+this.forgiveness) > 0)
                {
                    return this.current - (this.goal + this.forgiveness);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Returns True if the current number is greater than the range maximum
        /// </summary>
        public bool OverBoard
        {
            get { return this.Deviation > 0; }
        }
        /// <summary>
        /// Stores the current amount 
        /// </summary>
        public int Current
        {
            get { return this.current; }
            set
            {
                this.current = value;
            }
        }
        /// <summary>
        /// Stores the optimal number
        /// </summary>
        public int Goal
        {
            get { return this.goal; }
            set
            {
                this.goal = value;
            }
        }
        /// <summary>
        /// Stores the uncertainty thats foriven, which forms the range thats considered acceptable
        /// </summary>
        public int Forgiveness
        {
            get { return this.forgiveness; }
            set
            {
                this.forgiveness = value;
            }
        }
        /// <summary>
        /// Returns true if the current number is greater than the range minimum
        /// </summary>
        public bool Complete
        {
            get { return this.current>=this.goal-forgiveness; }
        }
        /// <summary>
        /// Property for the required key
        /// </summary>
        public Keys Key
        {
            get { return this.key; }
            set
            {
                this.key = value;
            }
        }

        public KeyCondition(int goal,int forgiveness, Keys key)
        {
            this.current = 0;
            this.goal = goal;
            this.forgiveness = forgiveness;
            this.key = key;
            this.basic = new KeyCondition();
        }
        /// <summary>
        /// Private constructor used only to create a default version of the keycondition
        /// </summary>
        private KeyCondition()
        {
            this.basic.goal = this.goal;
            this.basic.forgiveness = this.forgiveness;
            this.basic.key = this.key;
            this.basic.current = this.current;
        }
        /// <summary>
        /// Resets the  key condition to its default state
        /// </summary>
        public void Reset()
        {
            this.goal = this.basic.goal;
            this.forgiveness = this.basic.forgiveness;
            this.key = this.basic.key;
            this.current = 0;
        }


    }
}
