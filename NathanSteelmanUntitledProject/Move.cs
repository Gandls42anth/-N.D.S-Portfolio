using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NathanSteelmanUntitledProject
{
    class Move
    {
        private string name;
        private string baseName;
        private int deviation;
        private int maxDeviation;
        private int baseMaxDev;
        private List<KeyCondition> keyOrder;
        private List<KeyCondition> baseKeyOrder;
        private KeyCondition current;
        private int currentIndex;
        private Dictionary<char, int> saved;
        private Rectangle keyRect;

        public Dictionary<char,int> Saved
        {
            get { return this.saved; }
            set
            {
                this.saved = value;
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
            }
        }
        public int Deviation
        {
            get { return this.deviation; }
            set
            {
                this.deviation = value;
            }
        }

        public int MaxDeviation
        {
            get { return this.maxDeviation; }
            set
            {
                this.deviation = value;
            }
        }

        public List<KeyCondition> KeyOrder
        {
            get { return this.keyOrder; }
        }

        //A move should have 
        //a name,
        //a max amount of wasted keystrokes(deviations),
        //a list of keyConditions(in an order),
        //and optional parameters for starting deviation and saved key-->int dictionary
        public Move(string name, int maxDeviation,List<KeyCondition> keyOrder,Rectangle keyRect)
        {
            this.name =this.baseName = name;
            this.maxDeviation =this.baseMaxDev = maxDeviation;
            this.keyOrder =this.baseKeyOrder = keyOrder;
            this.keyRect = keyRect;
        }
        /// <summary>
        /// This method updates and returns the status of the move (In regards to its keyconditions being satisfied and thererfore casting)
        /// The status is represented by a 1,0, or -1. 1 indicates its ready for use, 0 indicates that it still needs updates, -1 indicates that its doomed to fail
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Check(Dictionary<char,int> param)
        {
            //First, check if its already ready, if not attempt to update
            if (!Ready())
            {
                //If the current deviation has already exceeded the max, dont bother checking further keystrokes
                if (deviation < maxDeviation)
                {
                    //Filter the new parameter dictionary by your currently saved dictionary, this ensures only new keystrokes are passed into the future calculations
                    Dictionary<char, int> filter = new Dictionary<char, int>();
                    //If there is no currently saved dictionary (meaning this is the first run-through) don't attempt to filter
                    if (saved != null)
                    {
                        foreach (char n in saved.Keys)
                        {
                            if (param.ContainsKey(n))
                            {
                                filter.Add(n, param[n] - saved[n]);
                            }
                        }
                    }
                    //Set the "current" keycondition to closest incomplete one in the list
                    for (int i = 0; i < keyOrder.Count; i++)
                    {
                        //This means that "current" is the incomplete condition closest to the front (and therefore first in the order) in the list
                        if (current == null)
                        {
                            current = keyOrder[0];
                            currentIndex = 0;
                        }else if (current.Complete && currentIndex!=(keyOrder.Count))
                        {
                            current = keyOrder[i + 1];
                            currentIndex++;
                        }
                    }
                    //Now the current refers to the keycondition that needs to be fulfilled
                    //And the currentIndex refers to its position in the KeyOrder List 

                    foreach (char m in filter.Keys)
                    {
                        if (m == current.Key)
                        {
                            //Update the current keystroke number
                            current.Current += filter[m];
                        }
                        else if (currentIndex != 0 && m == KeyOrder[currentIndex - 1].Key)
                        {
                            //entering this block means that the filtered list contains a value for the previous keycondition
                            //I.E this is for when the players has technically completed the previous condition but is still holding the key
                            //Meaning that these keystrokes will be forgiven and not counted as "deviation"s until they pass out of the range of forgiveness

                            //Update its "current"
                            keyOrder[currentIndex - 1].Current += filter[m];

                            //If its gone overboard, count it as a deviation
                            if (keyOrder[currentIndex - 1].OverBoard)
                            {
                                deviation += filter[m];
                            }

                        }
                        else
                        {
                            //This means the filtered list contains a key outside of the current or previous keyconditions
                            //It should be counted as a deviation
                            deviation += filter[m];
                        }
                    }
                    //Save the original parameter as the new "saved"
                    saved = param;

                    //Since filter exists on the stack it does not need to be cleared at the end of the method
                    //And the garbage collector will clear out the heap data it used due to the "new" keyword
                }
            }
            //Although this could be chained as an "else" to the previous if statement, that would neglect the deviation>max deviation outcome
            if (Ready())
            {
                return 1;
            }else if (deviation > maxDeviation)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// This returns true if there are any keystrokes that are overboard
        /// </summary>
        /// <returns></returns>
        public bool AnyOverBoard()
        {
            bool anyOverBoard = false;
            foreach(KeyCondition n in keyOrder)
            {
                if (n.OverBoard)
                {
                    anyOverBoard = true;
                }
            }
            return anyOverBoard;
        }
        /// <summary>
        /// Returns the amount of keyconditions with overboard amounts
        /// </summary>
        /// <returns></returns>
        public int OverBoard()
        {
            int sum = 0;
            foreach(KeyCondition n in keyOrder)
            {
                if (n.OverBoard)
                {
                    sum++;
                }
            }
            return sum;
        }

        /// <summary>
        /// This returns True if all Keyconditions of Keyorder list are considered "complete", otherwise false
        /// </summary>
        /// <returns></returns>
        public bool Ready()
        {
            bool ready = true;
            foreach(KeyCondition n  in keyOrder)
            {
                if (!n.Complete)
                {
                    ready = false;
                }
            }
            return ready;
        }
        /// <summary>
        /// Resets the move to its default state
        /// </summary>
        public void Reset()
        {
            current = null;
            this.name = this.baseName;
            this.deviation = 0;
            this.maxDeviation = this.baseMaxDev;
            this.saved = null;
            foreach(KeyCondition n in keyOrder)
            {
                n.Reset();
            }
        }

        public void Draw(SpriteBatch sb,SpriteFont keyFont, SpriteFont numFont,Texture2D texture)
        {
            int p = 0;
            for(int i = 0; i < keyOrder.Count; i++)
            {
                if(i%3 == 0)
                {
                    p++;
                }
                keyOrder[i].Draw(sb, keyFont, numFont, this.keyRect, new Vector2((i % 3 * keyRect.Width) + keyRect.Width, (p - 1) * keyRect.Height),texture);
            }
        }
    }
}
