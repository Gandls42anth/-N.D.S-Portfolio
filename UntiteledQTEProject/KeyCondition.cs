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
        private char key;
        private int current;
        private int baseGoal;
        private int baseForgiveness;
        private char baseKey;
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
        public char Key
        {
            get { return this.key; }
            set
            {
                this.key = value;
            }
        }

        public KeyCondition(int goal,int forgiveness, char key)
        {
            this.current = 0;
            this.goal = this.baseGoal = goal;
            this.forgiveness =this.baseForgiveness= forgiveness;
            this.key =this.baseKey= key;
        }
        /// <summary>
        /// Resets the  key condition to its default state
        /// </summary>

        //This exists for the possibility that these values may become altered
        //I.e You have an ability that allows your next move to allow  more forgiveness, or be all one key, or lessen/increase the goals so it can be performed faster/slower
        //Essentially this humors the possibility of altering the nature of your keyconditions
        public void Reset()
        {
            this.goal = this.baseGoal;
            this.forgiveness = this.baseForgiveness;
            this.key = this.baseKey;
            this.current = 0;
        }
        /// <summary>
        /// This method should draw the key in a large font, and the range centered below it in a smaller font
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb,SpriteFont keyFont,SpriteFont numFont,Rectangle rect, Vector2 position,Texture2D texture)
        {
            Rectangle frontRect = new Rectangle(rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);
            Vector2 frontVector = new Vector2(position.X+5, position.Y+5);
            sb.Draw(texture, position, rect, Color.Black);
            if (this.Complete && !this.OverBoard)
            {
                sb.Draw(texture, frontVector, frontRect, Color.Green);
            }else if (this.OverBoard)
            {
                sb.Draw(texture, frontVector, frontRect, Color.Red);
            }
            else
            {
                sb.Draw(texture, frontVector, frontRect, Color.White);
            }
            sb.DrawString(keyFont, this.key.ToString(), new Vector2(position.X + 20, position.Y + 15),Color.Black);
            sb.DrawString(numFont, (this.goal - forgiveness) + "-" + (this.goal + forgiveness), new Vector2(position.X + 15, position.Y + 35), Color.Black);
            sb.DrawString(numFont, this.current.ToString(), new Vector2(position.X + 15, position.Y + 55), Color.Black);

        }

    }
}
