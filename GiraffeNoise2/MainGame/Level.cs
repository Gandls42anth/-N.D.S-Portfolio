using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    class Level : GameObject
    {

        //This is the level class, its a gameobject that represents an entire level
        private GameState GS;
        private int LevelNum;
        List<Guard> guardList;
        private Rectangle finishLine;
        private Rectangle defaultRect;
        private Level baseLevel;
        
        //This represents a 2d  array of where guard should be in a level
        //Its first value is always 5 since thats the number of rows
        //The second value will be dependent on the length of the level, determined by the number of level and the difficulty
        private bool[,] guard;
        private Guard baseGuard;

        public void Reset()
        {
            this.position = this.defaultRect;
            this.LevelNum = 0;
            this.finishLine = new Rectangle(this.X + this.position.Width * (5 + LevelNum), this.Y, 100, this.position.Height);
            guardList.Clear();
            GuardGeneration();
        }

        public int Num
        {
            get { return this.LevelNum; }
        }

        //Helper Properties
        public int X
        {
            get { return this.position.X; }
            set
            {
                this.position.X = value;
                this.finishLine = new Rectangle(this.X + this.position.Width * (5 + LevelNum), this.Y, 100, this.position.Height);
            }
        }
        public int Y
        {
            get { return this.position.Y; }
            set
            {
                this.position.Y = value;
                this.finishLine = new Rectangle(this.X + this.position.Width * (5 + LevelNum), this.Y, 100, this.position.Height);
            }
        }

        public GameState GameState
        {
            get { return this.GS; }
            set
            {
                this.GS = value;
            }
        }

        public Rectangle DefaultRect
        {
            get { return this.defaultRect; }
        }

        public Rectangle FinishLine
        {
            get { return this.finishLine; }
        }

        public List<Guard> Guards
        {
            get { return this.guardList; }
        }

        public Guard BaseGuard
        {
            get { return this.baseGuard; }
        }

        // constructor
        public Level(GameState gs,int levelNum, Texture2D txt,Rectangle rect,Guard baseGuard) : base(rect,txt)
        {
            Random randy = new Random();
            this.defaultRect = rect;
            this.GS = gs;
            this.LevelNum = levelNum;
            bool curAdd = false;
            this.baseGuard = baseGuard;
            guardList = new List<Guard>();
            this.finishLine = new Rectangle(this.X + this.position.Width * (5 + LevelNum), this.Y, 100, this.position.Height);

            //This is where the random generation logic is handled for levels
            GuardGeneration();


        }
        //The the Guard 2d array now has randomly placed "trues" where guards are supposed to be

        // personalized draw method
        public void Draw(SpriteBatch sb,SpriteFont nsf,Texture2D  finishLine,SpriteFont finish)
        {
            //This for loop allows for almost infinitely large levels to be drawn, by redrawing the background level sprite  and shifting the x
            //value over and over
            for(int i = 0; i < 5+LevelNum; i++)
            {
                sb.Draw(this.texture, new Rectangle(new Point(this.position.X + this.position.Width*i,this.position.Y), this.position.Size),Color.White);
                sb.DrawString(nsf,string.Format("{0}",i),new Vector2(this.position.X + this.position.Width*i,this.position.Y),Color.Red);
            }
            for(int p = 0; p < guardList.Count; p++)
            {
                guardList[p].Draw(sb);
            }
            sb.Draw(finishLine, new Rectangle(this.X + this.position.Width * (5 + LevelNum), this.Y,100,this.position.Height),new Rectangle(this.X + this.position.Width * (5 + LevelNum), this.Y, 100, this.position.Height), Color.White * 2.0f);
            sb.DrawString(finish, "F\nI\nN\nI\nS\nH", new Vector2(this.X + this.position.Width * (5 + LevelNum) + 30,this.Y),Color.Black);
        }

        // for guard movement
        public void Move(int speed = 5)
        {
            for(int i = 0; i < guardList.Count; i++)
            {
                guardList[i].X -= speed+2;
            }

            this.X -= speed;
        }

        // collision detection for every guard
        public bool Collision(Player p)
        {
            for(int i = 0; i < guardList.Count; i++)
            {
                if (guardList[i].CollisionBox.Intersects(p.CollisionBox))
                {
                    return true;
                }
            }
            return false;
        }

        // player's win con
        public bool Win(Player p)
        {
            if (this.finishLine.Intersects(p.CollisionBox))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Next()
        {
            this.position = this.defaultRect;
            this.finishLine = new Rectangle(this.X + this.position.Width * (5 + LevelNum), this.Y, 100, this.position.Height);
            this.LevelNum += 1;
            GuardGeneration();
        }

        public void GuardGeneration()
        {
            //This is where the random generation logic is handled for levels
            guardList.Clear();
            Random randy = new Random();
            int levelNum = this.LevelNum;
            bool curAdd = false;
            GameState gs = this.GS;
            if (gs == GameState.Normal)
            {
                int guards = randy.Next(3 + levelNum, 5 + levelNum * 2);
                int positionsNum = (5 + levelNum) * 3;
                guard = new bool[5, positionsNum];
                for (int i = 0; i < positionsNum; i++)
                {
                    for (int p = 0; p < 5; p++)
                    {
                        //This is a bit complicated to explain, but it allows for reasonable distribution of the guards, balancing
                        //The amount of guards left with the amount of spots left on the map
                        //It also favors creating them on different rows instead of different columns
                        //(Since all of them on different rows would create an easier game)
                        if (guards != 0)
                        {
                            int positionsLeft = guard.Length - (i * 5) - p;
                            curAdd = randy.Next(positionsLeft) < guards;
                            guard[p, i] = curAdd;
                            if (curAdd)
                            {
                                guards -= 1;
                                guardList.Add(new Guard
                                    (new Rectangle
                                    (this.position.X + this.position.Width + ((int)this.position.Width / 3 * i),
                                    155 + 50 * p,
                                    (int)baseGuard.GuardWidth,
                                    (int)baseGuard.GuardHeight),
                                    baseGuard.Texture,
                                    levelNum,
                                    baseGuard.ViewCone,
                                    true));
                            }
                        }
                        else
                        //If we're all out of guard to give, the remaining spots will be set to false
                        {
                            guard[p, i] = false;
                        }
                    }
                }
            }
            else if (gs == GameState.Hard)
            {
                int guards = randy.Next(5 + levelNum * 2, 7 + levelNum * 3);
                int positionsNum = (5 + levelNum) * 3;
                guard = new bool[5, positionsNum];
                for (int i = 0; i < positionsNum; i++)
                {

                    {
                        for (int p = 0; p < 5; p++)
                        {
                            //This is a bit complicated to explain, but it allows for reasonable distribution of the guards, balancing
                            //The amount of guards left with the amount of spots left on the map
                            //It also favors creating them on different rows instead of different columns
                            //(Since all of them on different rows would create an easier game)
                            if (guards != 0)
                            {
                                int positionsLeft = guard.Length - (i * 5) - p;
                                curAdd = randy.Next(positionsLeft) < guards;
                                guard[p, i] = curAdd;
                                if (curAdd)
                                {
                                    guards -= 1;
                                    guardList.Add(new Guard
                                        (new Rectangle
                                        (this.position.X + this.position.Width + ((int)this.position.Width / 3 * i),
                                        155 + 50 * p,
                                        (int)baseGuard.GuardWidth,
                                        (int)baseGuard.GuardHeight),
                                        baseGuard.Texture, levelNum,
                                        baseGuard.ViewCone,
                                        true));
                                }
                            }
                            else
                            //If we're all out of guard to give, the remaining spots will be set to false
                            {
                                guard[p, i] = false;
                            }
                        }
                    }
                }
            }
            else
            {
                //This code is never supposed to run
                guard = new bool[0, 0];
            }
        }












    }
}
