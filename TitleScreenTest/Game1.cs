using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TitleScreenTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont backLayer;
        private SpriteFont frontLayer;
        private SpriteFont Subtitle;
        private SpriteFont Normal;
        private List<Color> Colors;
        private int c;
        private int shift;
        private Gamemode curState;
        private KeyboardState KBS;
        private KeyboardState prevKBS;
        private MouseState MS;
        private MouseState prevMS;
        public enum Gamemode
        {
            Mainmenu,
            Normal,
            Hard,
            Speedrun
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            c = 0;
            shift = 0;
            Colors = new List<Color>(6) { Color.Red, Color.DarkOrange, Color.Yellow, Color.Green, Color.Blue, Color.Violet };
            curState = Gamemode.Mainmenu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.backLayer = this.Content.Load<SpriteFont>("File");
            this.frontLayer = this.Content.Load<SpriteFont>("FrontLayer");
            this.Subtitle = this.Content.Load<SpriteFont>("Subtitle");
            this.Normal = this.Content.Load<SpriteFont>("Normal");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            this.KBS = Keyboard.GetState();
            this.MS = Mouse.GetState();
            if (c % 30 == 0)
            {
                shift++;
            }
            c++;

            switch (curState)
            {
                default:
                    break;
                case Gamemode.Mainmenu:
                    if (SingleKeyPress(Keys.N, KBS, prevKBS))
                    {
                        curState = Gamemode.Normal;
                    }else if (SingleKeyPress(Keys.H, KBS, prevKBS))
                    {
                        curState = Gamemode.Hard;
                    }else if (SingleKeyPress(Keys.S, KBS, prevKBS))
                    {
                        curState = Gamemode.Speedrun;
                    }
                    break;
                case Gamemode.Normal:
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        curState = Gamemode.Mainmenu;
                    }
                    break;
                case Gamemode.Hard:
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        curState = Gamemode.Mainmenu;
                    }
                    break;
                case Gamemode.Speedrun:
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        curState = Gamemode.Mainmenu;
                    }
                    break;
            }

            this.prevKBS = Keyboard.GetState();
            this.prevMS = Mouse.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            switch (curState) {
                default:
                    break;
                case Gamemode.Mainmenu:
                _spriteBatch.DrawString(frontLayer, "Giraffe Noise 2", new Vector2(35, 7), Color.OrangeRed);
                _spriteBatch.DrawString(Subtitle, "Choose Mode: ", new Vector2(35, 187), Color.Green);
                _spriteBatch.DrawString(Normal, "Normal- Press 'N'\nHard - Press 'H'", new Vector2(35, 237), Color.Gray);
                _spriteBatch.DrawString(frontLayer, "B", new Vector2(35, 87 + (15 * (float)Math.Sin(c / 6))), Colors.ToArray()[(shift) % Colors.Count]);
                _spriteBatch.DrawString(frontLayer, "E", new Vector2(95, 87 + (15 * (float)Math.Sin(c / 6 + 30))), Colors.ToArray()[(1 + shift) % Colors.Count]);
                _spriteBatch.DrawString(frontLayer, "G", new Vector2(155, 87 + (15 * (float)Math.Sin(c / 6 + 60))), Colors.ToArray()[(2 + shift) % Colors.Count]);
                _spriteBatch.DrawString(frontLayer, "I", new Vector2(235, 87 + (15 * (float)Math.Sin(c / 6 + 90))), Colors.ToArray()[(3 + shift) % Colors.Count]);
                _spriteBatch.DrawString(frontLayer, "N", new Vector2(275, 87 + (15 * (float)Math.Sin(c / 6 + 120))), Colors.ToArray()[(4 + shift) % Colors.Count]);
                _spriteBatch.DrawString(frontLayer, "?", new Vector2(345, 87 + (15 * (float)Math.Sin(c / 6 + 150))), Colors.ToArray()[(5 + shift) % Colors.Count]);
                _spriteBatch.DrawString(Normal, "Speedrun - Press 'S'", new Vector2(35, 307), Color.Gray);
                break;
                case Gamemode.Normal:
                    _spriteBatch.DrawString(frontLayer, "Placeholder for \nNormal mode", new Vector2(35, 7), Color.OrangeRed);
                    _spriteBatch.DrawString(Normal, "To exit, press enter", new Vector2(35, 307), Color.Yellow);
                    break;
                case Gamemode.Hard:
                    _spriteBatch.DrawString(frontLayer, "Placeholder for \nHard mode", new Vector2(35, 7), Color.OrangeRed);
                    _spriteBatch.DrawString(Normal, "To exit, press enter", new Vector2(35, 307), Color.Yellow);
                    break;
                case Gamemode.Speedrun:
                    _spriteBatch.DrawString(frontLayer, "Placeholder for \nSpeedrun mode", new Vector2(35, 7), Color.OrangeRed);
                    _spriteBatch.DrawString(Normal, "To exit, press enter", new Vector2(35, 307), Color.Yellow);
                    break;
        }

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected bool SingleKeyPress(Keys key,KeyboardState kbs,KeyboardState prevkbs)
        {
            return (kbs.IsKeyUp(key) && prevkbs.IsKeyDown(key));
        }

        protected bool SingleMousePress(ButtonState b,MouseState ms, MouseState prevms)
        {
            return (ms.LeftButton != b && prevms.LeftButton == b);
        }
    }
}
