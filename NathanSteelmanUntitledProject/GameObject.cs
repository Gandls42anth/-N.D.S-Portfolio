using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace NathanSteelmanUntitledProject
{
    class GameObject
    {
        private Rectangle baserect;
        private Rectangle position;
        private Texture2D texture;

        public Rectangle Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
            }
        }

        public Rectangle defaultPosition
        {
            get { return this.baserect; }
        }

        public Texture2D Texture
        {
            get { return this.texture; }
            set
            {
                this.texture = value;
            }
        }

        public int X
        {
            get { return this.position.X; }
            set
            {
                this.position.X = value;
            }
        }

        public int Y
        {
            get { return this.position.Y; }
            set
            {
                this.position.Y = value;
            }
        }

        public GameObject(Rectangle position,Texture2D texture)
        {
            this.position = position;
            this.baserect = position;
            this.texture = texture;
        }

        public virtual void Reset()
        {
            this.position = this.baserect;
        }


    }
}
