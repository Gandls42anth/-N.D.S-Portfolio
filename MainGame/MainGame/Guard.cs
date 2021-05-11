using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    class Guard : GameObject
    {
        // fields
        private Rectangle view;
        private int alert;
        private Rectangle guardRect;
        private Texture2D guardTexture;
        private Rectangle guardCollision;
        private Texture2D viewCone;
        private int orientation;
        private bool turn;

        // properties for fields
        public Texture2D ViewCone
        {
            get { return this.viewCone; }
        }

        public int X
        {
            get { return this.guardRect.X; }

            set
            {
                this.guardRect = new Rectangle(new Point(value, this.guardRect.Y), this.guardRect.Size);
                this.guardCollision = new Rectangle(new Point(guardRect.X - 50, guardRect.Y), new Point(30, 50));
            }
        }

        public int Y
        {
            get { return this.guardRect.Y; }

            set
            {
                this.guardRect = new Rectangle(new Point(value, this.guardRect.X), this.guardRect.Size);
                this.guardCollision = new Rectangle(new Point(guardRect.X - 50, guardRect.Y), new Point(30, 50));
            }
        }

        public int GuardHeight
        {
            get { return guardRect.Height; }
        }

        public int GuardWidth
        {
            get { return guardRect.Width; }
        }

        public Rectangle CollisionBox
        {
            get { return this.guardCollision; }
        }

        public int Orientation
        {
            get { return this.orientation; }
            set
            {
                orientation = value;
            }
        }

        // constructor
        public Guard(Rectangle rect,Texture2D txt,int alert,Texture2D viewCone, bool turn = false) : base(rect, txt)
        {
            this.guardTexture = txt;
            this.guardRect = rect;
            this.alert = alert;
            this.guardCollision = new Rectangle(new Point(guardRect.X-50,guardRect.Y), new Point(30,50));
            this.turn = turn;
            this.viewCone = viewCone;
        }

        
        // guard movement; we only want them to move horizontally
        public void GuardMovement(GameTime gameTime)
        {
            // move right/left 5 units depending on if they turned or not
            if (this.turn)
            {
                this.X += 5;
            }
            else
            {
                this.X -= 5;
            }

            // so the guard auto turns around after moving
            this.turn = !this.turn;
        }

        // collision property
        public Rectangle CollisionRectangle
        {
            get { return this.guardCollision; }
        }

        // guard draw method
        public void Draw(SpriteBatch sb)
        {

            //sb.Draw(playerTexture, playerRect, null, Color.White, (float)(((double)orientation / 180) * Math.PI), new Vector2(50, 50), SpriteEffects.None, 0f);
            if (turn)
            {
                sb.Draw(guardTexture, guardRect, null, Color.White, (float)(((double)orientation / 180) * Math.PI), new Vector2(50, 50), SpriteEffects.None, 0f);
            }
            else if (!turn)
            {
                sb.Draw(guardTexture, guardRect, null, Color.White, (float)(((double)orientation / 180) * Math.PI), new Vector2(50, 50), SpriteEffects.FlipHorizontally, 0f);
            }
            sb.Draw(viewCone,CollisionRectangle, Color.Red * 0.3f);
        }
    }
}
