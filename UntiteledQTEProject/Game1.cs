using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace NathanSteelmanUntitledProject
{
    public class Game1 : Game
    {
        public enum GameState
        {
            title,
            qte,
            pick,
            startup,
            finishmove,
            win,
            lose
        }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Queue<KeyboardState> KBSqueue;
        private Dictionary<char,int> inputs;
        private SpriteFont keyFont;
        private SpriteFont numFont;
        private GameState curGS;
        private KeyboardState previousKBS;
        private KeyboardState KBS;
        int frame;
        private Texture2D keyBackground;
        private KeyCondition conditionQ;
        private KeyCondition conditionT;
        private KeyCondition conditionE;
        private Move testMove;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            curGS = GameState.qte;
            KBSqueue = new Queue<KeyboardState>();
            inputs = new Dictionary<char, int>();
            conditionQ = new KeyCondition(30, 25,'Q');
            conditionT = new KeyCondition(30, 25, 'T');
            conditionE = new KeyCondition(30, 25, 'E');
            testMove = new Move("Test", 1000, new List<KeyCondition> { conditionQ, conditionT, conditionE }, new Rectangle(20, 20, 50, 50));



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.keyBackground = Content.Load<Texture2D>("keyBackground");
            this.keyFont = Content.Load<SpriteFont>("KeyFont");
            this.numFont = Content.Load<SpriteFont>("NumFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            frame++;

            switch (curGS)
            {
                default:
                    break;


                case GameState.qte:
                    KBS = Keyboard.GetState();
                    KBSqueue.Enqueue(KBS);
                    if (KBSqueue.Count > 360)
                    {
                        KBSqueue.Dequeue();
                    }
                    
                    inputs = CastToChars(KeysHeld(KBSqueue));
                    previousKBS = Keyboard.GetState();
                    testMove.Check(inputs);
                    break;

            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here
            if (SingleKeyHold(previousKBS, KBS, Keys.W))
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            _spriteBatch.Begin();
            if(inputs.ContainsKey('D'))
            {
                //_spriteBatch.DrawString(keyFont, inputs['D'].ToString(), new Vector2(100,100), Color.Black);
            }
            //_spriteBatch.DrawString(keyFont, inputs.Count.ToString(), new Vector2(100,200), Color.Black);
            //_spriteBatch.DrawString(keyFont, KBSqueue.Count.ToString(), new Vector2(100,300), Color.Black);
            testMove.Draw(_spriteBatch,keyFont,numFont,keyBackground);

            _spriteBatch.End();

            base.Draw(gameTime);
        }



        public bool SingleKeyPress(KeyboardState prevKBS, KeyboardState KBS,Keys key)
        {
            return prevKBS.IsKeyUp(key) && KBS.IsKeyDown(key);
        }

        public bool SingleKeyHold(KeyboardState prevKBS,KeyboardState KBS, Keys key)
        {
            return prevKBS.IsKeyDown(key) && KBS.IsKeyDown(key);
        }
        public bool SingleKeyRelease(KeyboardState prevKBS,KeyboardState KBS, Keys key)
        {
            return previousKBS.IsKeyDown(key) && KBS.IsKeyUp(key);
        }

        /// <summary>
        /// This takes in a queue of keyboard states and turns into a dictionary of keys with associated weights of how long they were held
        /// </summary>
        /// <param name="KBSqueue"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Dictionary<Keys,int> KeysHeld(Queue<KeyboardState> KBSqueue)
        {
            KeyboardState prevKBS;
            KeyboardState KBS;
            Dictionary<Keys, int> param = new Dictionary<Keys, int> { };
            for(int i = 0; i < KBSqueue.Count-1; i++)
            {
                prevKBS = KBSqueue.ToArray()[i];
                KBS = KBSqueue.ToArray()[i+1];
                param = HoldingKeysInitialize(prevKBS, KBS, param);
            }
            return param;
        }

        public Dictionary<Keys,int> SingleKeyIntialize(KeyboardState prevKBS,KeyboardState KBS, Dictionary<Keys,int> param)
        {
            Keys curkey = Keys.A;
            if (SingleKeyPress(prevKBS, KBS, curkey)){if(!param.ContainsKey(curkey)){param.Add(curkey, 1);}}
            curkey = Keys.A;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.B;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.C;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.D;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.E;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.F;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.G;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.H;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.I;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.J;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.K;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.L;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.M;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.N;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.O;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.P;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Q;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.R;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.S;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.T;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.U;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.V;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.W;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.X;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Y;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Z;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Left;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Up;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Down;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Right;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Space;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.Enter;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.LeftControl;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }
            curkey = Keys.LeftAlt;
            if (SingleKeyPress(prevKBS, KBS, curkey)) { if (!param.ContainsKey(curkey)) { param.Add(curkey, 1); } }

            return param;
        }
        public Dictionary<Keys,int> HoldingKeysInitialize(KeyboardState prevKBS,KeyboardState KBS, Dictionary<Keys,int> param)
        {
            Keys curkey = Keys.A;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.B;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++;} else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.C;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.D;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.E;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.F;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.G;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.H;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.I;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.J;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.K;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.L;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.M;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.N;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.O;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.P;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.Q;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.R;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.S;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.T;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.U;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.V;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.W;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.X;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.Y;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if(SingleKeyHold(prevKBS,KBS,curkey)){ param.Add(curkey, 2); }
            curkey = Keys.Z;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS,KBS,curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.Up;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.Left;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.Down;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.Right;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.Space;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.Enter;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.LeftControl;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }
            curkey = Keys.LeftAlt;
            if (param.ContainsKey(curkey) && SingleKeyHold(prevKBS, KBS, curkey)) { param[curkey]++; } else if (SingleKeyHold(prevKBS, KBS, curkey)) { param.Add(curkey, 2); }

            return param;
        }

        public Dictionary<char,int> CastToChars(Dictionary<Keys, int> param)
        {
            Dictionary<char, int> result = new Dictionary<char, int> { };
            foreach(Keys n in param.Keys)
            {
                result.Add(n.ToString().ToUpper().ToCharArray()[0], param[n]);
            }
            return result;
        }


    }
}
