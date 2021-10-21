using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    class Player : GameObject
    {
        // fields
        private int orientation;
        private Rectangle playerRect;
        private Texture2D playerTexture;
        private bool flip;
        private Rectangle playerCollision;
        private KeyboardState KBS;
        private KeyboardState prevKBS;

        //Position Properties to help change values
        public Rectangle CollisionBox
        {
            get { return this.playerCollision; }
        }
        public int X // player's x coord
        {
            get { return this.playerRect.X; }
            set
            {
                this.playerRect = new Rectangle(new Point(value, this.playerRect.Y), this.playerRect.Size);
                this.playerCollision = new Rectangle(new Point(this.playerRect.X+60, this.playerRect.Y + (playerRect.Height - 40)), this.playerCollision.Size);
            }
        }
        public int Y // player's y coord
        {
            get { return this.playerRect.Y; }
            set
            {
                this.playerRect = new Rectangle(new Point(this.playerRect.X, value), this.playerRect.Size);
                this.playerCollision = new Rectangle(new Point(this.playerRect.X+60, this.playerRect.Y + (playerRect.Height - 40)), this.playerCollision.Size);
            }
        }

        // player's dimensions
        public int PlayerHeight
        {
            get { return playerRect.Height; }
        }

        public int PlayerWidth
        {
            get { return playerRect.Width; }
        }

        //This represents the orientation in degrees, Clockwise, from  the +x axis
        public int Orientation
        {
            get { return this.orientation; }
            set
            {
                orientation = value;
            }
        }
        //Simple bool for whether or not to flip the sprite
        public bool Flip
        {
            get { return this.flip; }
            set
            {
                this.flip = value;
            }
        }
        //This is for changing the texture(to simulate walking)
        public Texture2D PlayerTexture
        {
            get { return this.playerTexture; }
            set
            {
                this.playerTexture = value;
            }
        }

        // constructor
        public Player(Rectangle rect,Texture2D txt) : base(rect, txt)
        {
            this.playerTexture = txt;
            this.playerRect = rect;
            this.orientation = 000;
            this.flip = true;

            this.playerCollision = new Rectangle(new Point(this.X+60, this.Y + (playerRect.Height - 40)), new Point(50, 30));
        }

        // draw
        public void Draw(SpriteBatch sb)
        {
            
            if (!flip)
            {
                sb.Draw(playerTexture, playerRect, null, Color.White, (float)(((double)orientation/180)*Math.PI), new Vector2(50, 50), SpriteEffects.None, 0f);
            }else if(flip)
            {
                sb.Draw(playerTexture, playerRect, null, Color.White, (float)(((double)orientation/180)*Math.PI), new Vector2(50, 50), SpriteEffects.FlipHorizontally, 0f);
            }
            sb.Draw(playerTexture, playerCollision, Color.White);
        }

        // key input help; method names are self explanatory
        protected bool SingleKeyPress(Keys key, KeyboardState kbs, KeyboardState prevkbs)
        {
            return (kbs.IsKeyUp(key) && prevkbs.IsKeyDown(key));
        }

        protected bool KeyHold(Keys key, KeyboardState kbs, KeyboardState prevkbs)
        {
            return (kbs.IsKeyDown(key) && prevkbs.IsKeyDown(key));
        }

        // player movement
        public void Update(GameTime gameTime)
        {
            KBS = Keyboard.GetState();

            if (SingleKeyPress(Keys.W, KBS, prevKBS)) // move up
            {
                //This is the top barrier
                if (this.Y > 50)
                {
                    this.Y -= 50;
                }
            }
            if (SingleKeyPress(Keys.S, KBS, prevKBS)) // move down
            {
                //This is the bottom barrier
                if (this.Y < 250)
                {
                    this.Y += 50;
                }
            }
            if (KeyHold(Keys.D, KBS, prevKBS)) // right
            {
                Flip = true;
                //Front Barrier
                if (this.X < 600)
                {
                    this.X += 3;
                }
            }
            if (KeyHold(Keys.A, KBS, prevKBS)) // left
            {
                Flip = false;
                //Back Barrier
                if (this.X > 30)
                {
                    this.X -= 3;
                }
            }
            prevKBS = Keyboard.GetState();
        }

    }
}
