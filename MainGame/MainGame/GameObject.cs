using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    // parent class for all game objects, since all game objects need a few things
    class GameObject
    {
        protected Rectangle position; // object position
        protected Texture2D texture; // object's image

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        // protected constructor
        protected GameObject(Rectangle position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }
    }
}
