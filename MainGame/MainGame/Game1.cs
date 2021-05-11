using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace MainGame
{
    enum GameState
    {
        Title,
        Controls,
        Normal,
        Hard,
        Speedrun,
        GameOver,
        Win,
        Endless
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // fields
        private GameState currentState;
        private bool prevSpeedrun;
        private bool prevEndless;
        private string PlayerName;
        private List<string> Scores;
        private Player player;
        private SpriteFont backLayer;
        private SpriteFont frontLayer;
        private SpriteFont Subtitle;
        private SpriteFont Normal;
        private Texture2D GiraffeSprite;
        private Texture2D GiraffeSpriteWalk;
        private Texture2D viewCone;
        private Texture2D finishLine;
        private List<Color> Colors;
        private KeyboardState KBS;
        private KeyboardState prevKBS;
        private int c;
        private int playTime;
        private int shift;
        private Level level;

        private Rectangle GiraffeRectangle;

        private Texture2D GuardSprite;
        private Rectangle GuardRectangle;
        private Guard guard1;


        private Texture2D background;

        //This is for the game over screen
        private Texture2D deadGiraffeSprite;
        private Rectangle deadGiraffeRectangle;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playTime = 0;
            c = 0;
            shift = 0;
            Colors = new List<Color>(6) { Color.Red, Color.DarkOrange, Color.Yellow, Color.Green, Color.Blue, Color.Violet };
            this.prevKBS = new KeyboardState();
            prevSpeedrun = false;
            prevEndless = false;
            Scores = new List<string> { };
            PlayerName = "";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // for title screen
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.backLayer = this.Content.Load<SpriteFont>("File");
            this.frontLayer = this.Content.Load<SpriteFont>("FrontLayer");
            this.Subtitle = this.Content.Load<SpriteFont>("Subtitle");
            this.Normal = this.Content.Load<SpriteFont>("Normal");
            
            // for in game
            this.viewCone = this.Content.Load<Texture2D>("unnamed");
            this.finishLine = this.Content.Load<Texture2D>("1227835");
            this.GiraffeSprite = this.Content.Load<Texture2D>("GiraffeStatic");
            this.GiraffeSpriteWalk = this.Content.Load<Texture2D>("GiraffeWalk");

            //Background
            this.background = this.Content.Load<Texture2D>("roadLong");

            //Guards
            this.GuardSprite = this.Content.Load<Texture2D>("BrandNewGuard");
            GuardRectangle = new Rectangle(750, 305, (int)GuardSprite.Width / 2, (int)GuardSprite.Height / 2);
            guard1 = new Guard(GuardRectangle, this.GuardSprite, 3, this.viewCone);

            //Dead Giraffe
            deadGiraffeSprite = this.Content.Load<Texture2D>("GiraffeDead");
            deadGiraffeRectangle = new Rectangle(500, 200, deadGiraffeSprite.Width / 4, deadGiraffeSprite.Height / 4);

            currentState = GameState.Title;
            this.level = new Level(GameState.Normal, 0, background, new Rectangle(0, 100, (int)background.Width / 2, (int)background.Height / 2), guard1);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KBS = Keyboard.GetState();

            // TODO: Add your update logic here

            switch (currentState)
            {
                case GameState.Title:
                    if (c % 30 == 0)
                    {
                        shift++;
                    }
                    Scores.Clear();
                    c++;
                    //This handles the switches between states
                    //But first it handles initialization logic
                    if (SingleKeyPress(Keys.N, KBS, prevKBS))
                    {
                        currentState = GameState.Normal;
                        DrawPlayer();
                        //Attempting first level creation
                        this.level.GameState = GameState.Normal;
                        this.level.Reset();
                    }
                    else if (SingleKeyPress(Keys.H, KBS, prevKBS))
                    {
                        currentState = GameState.Hard;
                        DrawPlayer();
                        //Attempting first level creation
                        this.level.GameState = GameState.Hard;
                        this.level.Reset();

                    }
                    else if (SingleKeyPress(Keys.S, KBS, prevKBS))
                    {
                        currentState = GameState.Speedrun;
                        DrawPlayer();
                        //Attempting first level creation
                        this.level.GameState = GameState.Normal;
                        this.level.Reset();
                    }else if (SingleKeyPress(Keys.E,KBS,prevKBS))
                    {
                        currentState = GameState.Endless;
                        DrawPlayer();
                        this.level.GameState = GameState.Normal;
                        this.level.Reset();
                    }
                    else if (SingleKeyPress(Keys.C, KBS, prevKBS))
                    {
                        currentState = GameState.Controls;
                    }
                    break;

                    // state logic for the controls screem
                case GameState.Controls:
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        currentState = GameState.Title;
                    }
                    break;

                //This Section handless the state logic
                //For the Normal, Hard and speedrun Modes
                case GameState.Normal:
                    //Playtime recording
                    playTime += 1;
                    //Increasing speed 
                    level.Move(2 + level.Num*2);

                    // player movement
                    player.Update(gameTime);

                    // temporary for now, until we can get the full game working
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        currentState = GameState.Title;
                    }

                    if (level.Win(player) && level.Num >= 5)
                    {
                        currentState = GameState.Win;
                    }else if (level.Win(player))
                    {
                        level.Next();
                    }

                    if (level.Collision(player))
                    {
                        currentState = GameState.GameOver;
                    }
                    break;

                case GameState.Hard:
                    //Playtime recording 
                    playTime += 1;
                    //faster movement and stronger scaling
                    level.Move(5 + level.Num*3);

                    // player movement
                    player.Update(gameTime);


                    // Switches between gamestates
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        currentState = GameState.Title;
                    }

                    if (level.Win(player) && level.Num >= 8)
                    {
                        currentState = GameState.Win;
                    }else if (level.Win(player))
                    {
                        level.Next();
                    }
                    if (level.Collision(player))
                    {
                        currentState = GameState.GameOver;
                    }
                    break;

                case GameState.Speedrun:
                    playTime += 1;
                    //Scaling like normal mode
                    level.Move(2 + level.Num*2);

                    // player movement
                    player.Update(gameTime);


                    // temporary for now, until we can get the full game working
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        currentState = GameState.Title;
                    }

                    //Recording speedrun wins
                    if (level.Win(player) && level.Num >= 5)
                    {

                        currentState = GameState.Win;
                        prevSpeedrun = true;
                        Scores.Clear();
                        //Read the speedrun scores from file (accurate to one hundredth of a second)

                            StreamReader ScoreReader = new StreamReader("Scores.txt");
                            string line;
                            string[] split;
                            int l = 0;
                        bool added = false;
                            while((line = ScoreReader.ReadLine()) != null)
                            {
                            //Add them (in order) into the "scores" list of strings in proper format
                                l++;
                                split = line.Split(',',':','.');
                                double time = double.Parse(split[1]) + double.Parse(split[2])*0.01;
                                if(Math.Round((double)playTime/60,2 ) < time)
                                {
                                    Scores.Add(l + "." + "☺" +  " : " + Math.Round((double)playTime / 60, 2) + "s");
                                    l++;
                                added = true;
                                }
                                Scores.Add(l + "." + split[0].ToUpper() + " : " + time + "s");
                            }
                            ScoreReader.Close();
                            if (!added)
                            {
                                Scores.Add((l+1) + "." + "☺" + " : " + Math.Round((double)playTime / 60, 2) + "s");
                            }
                            //Now the scores list contains all the scores
                            //Close the reader
                            ScoreReader.Close();






                    }else if (level.Win(player))
                    {
                        level.Next();
                    }
                    if (level.Collision(player))
                    {
                        currentState = GameState.GameOver;
                        prevSpeedrun = true;
                    }
                    break;

                case GameState.Endless:
                    playTime += 1;
                    //Player movement
                    level.Move(2 + level.Num * 2);
                    player.Update(gameTime);

                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        currentState = GameState.Title;
                    }
                    //Since endless has no win condition, winning will always cause a level.next()
                    if (level.Win(player))
                    {
                        level.Next();

                    }
                    //The only end is if you die, in which case we record your score(# of levels cleared)
                    else if (level.Collision(player))
                    {
                        currentState = GameState.GameOver;
                        prevEndless = true;

                        Scores.Clear();
                        //Read the endless scores from file
                        StreamReader ScoreReader = new StreamReader("EndlessScores.txt");
                        string line;
                        string[] split;
                        int l = 0;
                        bool added = false;
                        while ((line = ScoreReader.ReadLine()) != null)
                        {
                            l++;
                            split = line.Split(',', ':', '.');
                            double highestLevel = int.Parse(split[1]);
                            if (level.Num > highestLevel)
                            {
                                Scores.Add(l + "." + "☻" + " : " + level.Num);
                                l++;
                                added = true;
                            }
                            Scores.Add(l + "." + split[0].ToUpper() + " : " + highestLevel);
                        }
                        ScoreReader.Close();
                        if (!added)
                        {
                            Scores.Add((l + 1) + "." + "☻" + " : " + level.Num);
                        }
                        //Now the scores list contains all the scores
                        ScoreReader.Close();

                    }



                    break;

                case GameState.GameOver:
                    c++;
                    if (prevEndless)
                    {
                        //If it was endless, start asking for the player name
                        PlayerName = AddLetter(PlayerName);
                    }
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS) && prevEndless)
                    {
                        currentState = GameState.Title;
                        //Now write to file
                        StreamWriter ScoreWriter = new StreamWriter("EndlessScores.txt");
                        foreach (string n in Scores)
                        {

                            string[] split = n.Split(',', ':', '.', 's');
                            if (split[1].Trim() != "☻")
                            {
                                ScoreWriter.WriteLine(split[1].Trim() + ":" + split[2].Trim());
                            }
                            else
                            {
                                ScoreWriter.WriteLine(PlayerName.Trim().ToUpper() + ":" + split[2].Trim());
                            }
                        }
                        ScoreWriter.Close();
                        PlayerName = "";
                    }
                    else if(SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        currentState = GameState.Title;
                    }
                    break;
                case GameState.Win:
                    c++;
                    if (prevSpeedrun)
                    {
                        //If it was speedrun and you won, allow the player to enter their name
                        PlayerName = AddLetter(PlayerName);
                    }
                    if (SingleKeyPress(Keys.Enter, KBS, prevKBS) && prevSpeedrun)
                    {
                        //Once they've entered it, write the new order of scores to the file
                            StreamWriter ScoreWriter = new StreamWriter("Scores.txt");
                            foreach (string n in Scores)
                            {

                                string[] split = n.Split(',', ':', '.','s');
                                if (split[1].Trim() != "☺")
                                {
                                    ScoreWriter.WriteLine(split[1].Trim() + ":" + split[2].Trim());
                                }
                                else
                                {
                                    ScoreWriter.WriteLine(PlayerName.Trim().ToUpper() + ":" + split[2].Trim());
                                }
                            }
                            ScoreWriter.Close();
                        //And return to title
                        currentState = GameState.Title;
                        PlayerName = "";
                    }else if(SingleKeyPress(Keys.Enter, KBS, prevKBS))
                    {
                        //If it wasnt speedrun, just go to title
                        currentState = GameState.Title;
                    }
                    break;

                default:
                    break;
            }
            prevKBS = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            _spriteBatch.Begin();

            switch (currentState)
            {
                case GameState.Title:
                    GraphicsDevice.Clear(Color.Black);
                    playTime = 0;
                    // code for the special rainbow title screen
                    _spriteBatch.DrawString(frontLayer, "Giraffe Noise 2", new Vector2(35, 7), Color.OrangeRed);
                    _spriteBatch.DrawString(Subtitle, "Choose Mode: ", new Vector2(35, 187), Color.Green);
                    _spriteBatch.DrawString(Normal, "Normal - Press 'N'\nHard - Press 'H'", new Vector2(35, 237), Color.Gray);
                    _spriteBatch.DrawString(frontLayer, "B", new Vector2(35, 87 + (15 * (float)Math.Sin(c / 6))), Colors.ToArray()[(shift) % Colors.Count]);
                    _spriteBatch.DrawString(frontLayer, "E", new Vector2(95, 87 + (15 * (float)Math.Sin(c / 6 + 30))), Colors.ToArray()[(1 + shift) % Colors.Count]);
                    _spriteBatch.DrawString(frontLayer, "G", new Vector2(155, 87 + (15 * (float)Math.Sin(c / 6 + 60))), Colors.ToArray()[(2 + shift) % Colors.Count]);
                    _spriteBatch.DrawString(frontLayer, "I", new Vector2(235, 87 + (15 * (float)Math.Sin(c / 6 + 90))), Colors.ToArray()[(3 + shift) % Colors.Count]);
                    _spriteBatch.DrawString(frontLayer, "N", new Vector2(275, 87 + (15 * (float)Math.Sin(c / 6 + 120))), Colors.ToArray()[(4 + shift) % Colors.Count]);
                    _spriteBatch.DrawString(frontLayer, "?", new Vector2(345, 87 + (15 * (float)Math.Sin(c / 6 + 150))), Colors.ToArray()[(5 + shift) % Colors.Count]);
                    _spriteBatch.DrawString(Normal, "Speedrun - Press 'S'\nEndless - Press 'E'\nHow to Play - Press 'C'", new Vector2(35, 307), Color.Gray);

                    break;

                case GameState.Controls:
                    GraphicsDevice.Clear(Color.Black);
                    _spriteBatch.DrawString(frontLayer, "Goal and Controls:", new Vector2(10,10), Color.White);

                    _spriteBatch.DrawString(Normal, "You play as a giraffe attempting to evade the cops, " +
                        "\ndodge them as you race through the streets of NYC!",
                        new Vector2(10, 100), Color.LightPink);

                    _spriteBatch.DrawString(Normal, "Up - 'W'          Down - S" +
                        "\nLeft - 'A'          Right - 'D'", new Vector2(10, 250), Color.CornflowerBlue);

                    _spriteBatch.DrawString(Normal, "Press 'ENTER' to return to main menu", new Vector2(10, 420), Color.Gray);
                    break;

                case GameState.Normal:
                    level.Draw(_spriteBatch,Normal,finishLine,this.Subtitle);
                    //On normal and hard mode, tell the player how many guards there are
                    _spriteBatch.DrawString(
                        frontLayer, 
                        string.Format("Total Guards: {0}",level.Guards.Count), 
                        new Vector2(35, 7),
                        Color.OrangeRed
                        );

                    _spriteBatch.DrawString(Normal, string.Format("To exit, press enter"), new Vector2(35, 435), Color.Yellow);
                    player.Draw(_spriteBatch);
                    
                    break;

                case GameState.Hard:
                    level.Draw(_spriteBatch, Normal, finishLine, this.Subtitle);
                    //On normal and hard mode, tell the player how many guards there are
                    _spriteBatch.DrawString(
                        frontLayer,
                        string.Format("Total Guards: {0}", level.Guards.Count),
                        new Vector2(35, 7),
                        Color.OrangeRed
                        );

                    _spriteBatch.DrawString(Normal, string.Format("To exit, press enter"), new Vector2(35, 435), Color.Yellow);
                    player.Draw(_spriteBatch);

                    break;

                case GameState.Speedrun:
                    level.Draw(_spriteBatch, Normal, finishLine, this.Subtitle);
                    //on speedrun mode, display the total time
                    _spriteBatch.DrawString(
                        frontLayer,
                        string.Format("Total Time: {0}", Math.Round((double)playTime / 60, 2)),
                        new Vector2(35, 7),
                        Color.OrangeRed
                        );

                    _spriteBatch.DrawString(Normal, string.Format("To exit, press enter"), new Vector2(35, 435), Color.Yellow);
                    player.Draw(_spriteBatch);
                    break;
                case GameState.Endless:
                    level.Draw(_spriteBatch, Normal, finishLine, this.Subtitle);
                    //On endless mode, display the levels cleared
                        _spriteBatch.DrawString(
                        frontLayer,
                        string.Format("Levels Cleared: {0}", level.Num),
                        new Vector2(35, 7),
                        Color.OrangeRed
                        );
                    _spriteBatch.DrawString(Normal, string.Format("To exit, press enter"), new Vector2(35, 435), Color.Yellow);
                    player.Draw(_spriteBatch);
                    break;

                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.Black);
                    _spriteBatch.DrawString(frontLayer, "GAME OVER", new Vector2(35, 7), Color.OrangeRed);
                    _spriteBatch.DrawString(Normal, "To exit, press enter", new Vector2(35, 307), Color.Yellow);
                    player.Orientation = c;
                    player.Draw(_spriteBatch);
                    //Altered version of the speedrun score display, color must be white because gameover has a black background
                    //And since the "Game Over" is larger than the "YOU WIN!" string, the list has to be shifted slightly right
                    //and downward
                    if (prevEndless)
                    {
                        //Allow the player to see the real time entering of their name for the record
                        _spriteBatch.DrawString(Normal, string.Format("{0}:{1}", PlayerName.ToUpper(), level.Num), new Vector2(35, 177), Color.White);

                        for (int i = 0; i < Scores.Count; i++)
                        {
                            if (i < 10)
                            {
                                string[] split = Scores[i].Split(',', ':','.');
                                if (split[1].Trim() != "☻")
                                {
                                    _spriteBatch.DrawString(Normal, Scores[i], new Vector2(335, 80 + 30 * i), Color.White);
                                }
                                else
                                {
                                    _spriteBatch.DrawString(Normal, "Your Score HERE!", new Vector2(350, 80 + 30 * i), Color.White);
                                }
                            }
                        }

                    }
                    break;
                case GameState.Win:
                    GraphicsDevice.Clear(Color.White);
                    _spriteBatch.DrawString(frontLayer, "You WIN!", new Vector2(35, 7), Color.GreenYellow);
                    _spriteBatch.DrawString(Normal, "To exit, press enter", new Vector2(35, 307), Color.Black);
                    //If this was speedrun, display and check the scores
                    if (prevSpeedrun)
                    {
                        //Allow the player to see the real time entering of their name for the record
                        _spriteBatch.DrawString(Normal, string.Format("{0}:{1}",PlayerName.ToUpper(),Math.Round((double)playTime/60,2)), new Vector2(35, 157), Color.Black);

                        for(int i = 0; i < Scores.Count; i++)
                        {
                            if (i < 10)
                            {
                                string[] split = Scores[i].Split(',', ':', 's', '.');
                                if (split[1].Trim() != "☺")
                                {
                                    _spriteBatch.DrawString(Normal, Scores[i], new Vector2(335, 30 + 30 * i), Color.Black);
                                }
                                else
                                {
                                    _spriteBatch.DrawString(Normal, "Your Score HERE!", new Vector2(335, 30 + 30 * i), Color.Black);
                                }
                            }
                        }

                    }
                    player.Orientation = c;
                    player.Draw(_spriteBatch);
                    break;

                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // key press helper methods
        protected bool SingleKeyPress(Keys key, KeyboardState kbs, KeyboardState prevkbs)
        {
            return (kbs.IsKeyUp(key) && prevkbs.IsKeyDown(key));
        }

        protected bool KeyHold(Keys key, KeyboardState kbs, KeyboardState prevkbs)
        {
            return (kbs.IsKeyDown(key) && prevkbs.IsKeyDown(key));
        }

        protected bool SingleMousePress(ButtonState b, MouseState ms, MouseState prevms)
        {
            return (ms.LeftButton != b && prevms.LeftButton == b);
        }

        //This method checks for if the player makes contact with any guards
        private void Contact()
        {
            if (player.Y == guard1.Y)
            {
                if (player.X > guard1.X)
                {
                    if (player.X -guard1.X < 15)
                    {
                        currentState = GameState.GameOver;
                    }
                }

                if (guard1.X > player.X)
                {
                    if (guard1.X - player.X < 100)
                    {
                        currentState = GameState.GameOver;
                    }
                }

                if (player.X == guard1.X)
                {
                    currentState = GameState.GameOver;
                }


            }
            /*
            if (player.Position.Intersects(guard1.Position) == true)
            {
                currentState = GameState.GameOver;
            }
            */

            //For Andy, the old giraffe sprite was divided by 4 and the guard sprite divided by 2
        }

        // helper method for gamestate switching
        protected void DrawPlayer()
        {
            GiraffeRectangle = new Rectangle(100, 200, (int)GiraffeSprite.Width / 4, (int)GiraffeSprite.Height / 4);
            this.player = new Player(GiraffeRectangle, this.GiraffeSprite);
        }

        public string AddLetter(string n)
        {
            if (SingleKeyPress(Keys.A, KBS, prevKBS))
            {
                return n + "a";
            }
            if (SingleKeyPress(Keys.B, KBS, prevKBS))
            {
                return n + "b";
            }
            if (SingleKeyPress(Keys.C, KBS, prevKBS))
            {
                return n + "c";
            }
            if (SingleKeyPress(Keys.D, KBS, prevKBS))
            {
                return n + "d";
            }
            if (SingleKeyPress(Keys.E, KBS, prevKBS))
            {
                return n + "e";
            }
            if (SingleKeyPress(Keys.F, KBS, prevKBS))
            {
                return n + "f";
            }
            if (SingleKeyPress(Keys.G, KBS, prevKBS))
            {
                return n + "g";
            }
            if (SingleKeyPress(Keys.H, KBS, prevKBS))
            {
                return n + "h";
            }
            if (SingleKeyPress(Keys.I, KBS, prevKBS))
            {
                return n + "i";
            }
            if (SingleKeyPress(Keys.J, KBS, prevKBS))
            {
                return n + "j";
            }
            if (SingleKeyPress(Keys.K, KBS, prevKBS))
            {
                return n + "k";
            }
            if (SingleKeyPress(Keys.L, KBS, prevKBS))
            {
                return n + "l";
            }
            if (SingleKeyPress(Keys.M, KBS, prevKBS))
            {
                return n + "m";
            }
            if (SingleKeyPress(Keys.N, KBS, prevKBS))
            {
                return n + "n";
            }
            if (SingleKeyPress(Keys.O, KBS, prevKBS))
            {
                return n + "o";
            }
            if (SingleKeyPress(Keys.P, KBS, prevKBS))
            {
                return n + "p";
            }
            if (SingleKeyPress(Keys.Q, KBS, prevKBS))
            {
                return n + "q";
            }
            if (SingleKeyPress(Keys.R, KBS, prevKBS))
            {
                return n + "r";
            }
            if (SingleKeyPress(Keys.S, KBS, prevKBS))
            {
                return n + "s";
            }
            if (SingleKeyPress(Keys.T, KBS, prevKBS))
            {
                return n + "t";
            }
            if (SingleKeyPress(Keys.U, KBS, prevKBS))
            {
                return n + "u";
            }
            if (SingleKeyPress(Keys.V, KBS, prevKBS))
            {
                return n + "v";
            }
            if (SingleKeyPress(Keys.W, KBS, prevKBS))
            {
                return n + "w";
            }
            if (SingleKeyPress(Keys.X, KBS, prevKBS))
            {
                return n + "x";
            }
            if (SingleKeyPress(Keys.Y, KBS, prevKBS))
            {
                return n + "y";
            }
            if (SingleKeyPress(Keys.Z, KBS, prevKBS))
            {
                return n + "z";
            }
            if (SingleKeyPress(Keys.Delete, KBS, prevKBS))
            {
                return n.Remove(n.Length-1);
            }
            else
            {
                return n;
            }
        }

    }
}
