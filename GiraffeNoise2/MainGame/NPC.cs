using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    class NPC : GameObject
    {
        // fields
        private Rectangle npcRect;
        private Texture2D npcTexture;
        private string message;

        // delegate to handle interaction with the npc
        public delegate void FriendlyNPCInteraction(string a);
        private FriendlyNPCInteraction niceInteraction;

        // properties
        public Rectangle NPCRect { get { return npcRect; } }
        public Texture2D NPCTexture { get { return npcTexture; } }
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        // constructor
        public NPC(Rectangle rect, Texture2D txt, string message) : base(rect,txt)
        {
            this.npcRect = rect;
            this.npcTexture = txt;
            this.message = message;
        }

        
    }
}
