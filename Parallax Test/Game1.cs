using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Parallax_Test
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D Wall;
        private Rectangle WallR1;
        private Rectangle WallR2;
        private Vector2 WallV1;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            WallR1 = new Rectangle(0,100,400,100);
            WallR2 = new Rectangle(800, 100, 400, 100);
            WallV1 = new Vector2(0,0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Wall = this.Content.Load<Texture2D>("wall");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.D))
            {
                WallV1 = new Vector2(WallV1.X-5, WallV1.Y);
            }
            if(kbs.IsKeyDown(Keys.A))
            {
                WallV1 = new Vector2(WallV1.X+5, WallV1.Y);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //These are the back walls
            _spriteBatch.Draw(Wall, new Rectangle((int)(WallR1.X + 0.33*WallV1.X - 1.67*WallR1.Width), WallR1.Y-20, 3*WallR1.Width,WallR1.Height), Color.Black);
            _spriteBatch.Draw(Wall, new Rectangle((int)(WallR1.X + 0.33*WallV1.X +1.66*WallR1.Width), WallR2.Y-20,3*WallR2.Width,WallR2.Height), Color.Black);
            //These are the 2nd walls
            _spriteBatch.Draw(Wall, new Rectangle((int)(WallR1.X + 0.5*WallV1.X -0.75*WallR1.Width), WallR1.Y-10, 2*WallR1.Width, WallR1.Height), Color.Green);
            _spriteBatch.Draw(Wall, new Rectangle((int)(WallR1.X + 0.5*WallV1.X + 1.75*WallR1.Width), WallR2.Y-10, 2*WallR2.Width, WallR2.Height), Color.Green);
            //These are the front walls
            _spriteBatch.Draw(Wall, new Rectangle((int)(WallR1.X + WallV1.X), WallR1.Y, WallR1.Width, WallR1.Height), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle((int)(WallR1.X + WallV1.X + 2*WallR1.Width), WallR2.Y, WallR2.Width, WallR2.Height), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(300,150,70,70), Color.Purple);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
