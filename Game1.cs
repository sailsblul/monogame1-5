using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace monogame1_5
{
    public class Game1 : Game
    {
        Random gen = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        enum Screen
        {
            Intro,
            Main,
            Win,
            Lose
        }
        Screen screen;
        SpriteFont titleFont;
        SpriteFont mainFont;
        Texture2D titleTexture;
        Texture2D cave;
        Texture2D winScreen;
        Texture2D[] steve = new Texture2D[3];
        Rectangle steveRect;
        int steveState = 1;
        Texture2D[] mob = new Texture2D[3];
        int mobChoice;
        Rectangle mobRect;
        bool spawned = false;
        float timeStamp;
        float timeLimit;
        SoundEffect creeperSound;
        SoundEffect spiderSound;
        SoundEffect zombieSound;
        SoundEffect[] deathSounds = new SoundEffect[3];
        SoundEffect explosion;
        Texture2D dab;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1440;
            _graphics.PreferredBackBufferHeight = 810;
            _graphics.ApplyChanges();
            Window.Title = "minceraft";
            screen = Screen.Intro;
            steveRect = new Rectangle(100, 300, 243, 300);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            titleFont = Content.Load<SpriteFont>("titlefont");
            mainFont = Content.Load<SpriteFont>("otherfont");
            titleTexture = Content.Load<Texture2D>("mctitle");
            cave = Content.Load<Texture2D>("cave");
            winScreen = Content.Load<Texture2D>("win");
            steve[0] = Content.Load<Texture2D>("steveleft");
            steve[1] = Content.Load<Texture2D>("steveright");
            steve[2] = Content.Load<Texture2D>("stevesword");
            mob[0] = Content.Load<Texture2D>("creeper");
            mob[1] = Content.Load<Texture2D>("spider");
            mob[2] = Content.Load<Texture2D>("zombie");
            creeperSound = Content.Load<SoundEffect>("Creeper_fuse");
            spiderSound = Content.Load<SoundEffect>("Spider_idle4");
            zombieSound = Content.Load<SoundEffect>("zombiesound");
            deathSounds[0] = Content.Load<SoundEffect>("Creeper_death");
            deathSounds[1] = Content.Load<SoundEffect>("Spider_death");
            deathSounds[2] = Content.Load<SoundEffect>("zombiedeath");
            explosion = Content.Load<SoundEffect>("explosion");
            dab = Content.Load<Texture2D>("dab");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (screen == Screen.Intro)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    screen = Screen.Main;
                    _graphics.PreferredBackBufferHeight = 880;
                    _graphics.ApplyChanges();
                }
            }
            else if (screen == Screen.Main)
            {
                if (spawned)
                {
                    if (mobRect.Intersects(steveRect))
                    {
                        if (steveState == 2)
                        {
                            deathSounds[mobChoice].Play();
                            screen = Screen.Lose;
                        }
                        else
                        {
                            if (mobChoice == 0)
                                explosion.Play();
                            screen = Screen.Win;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        mobRect.X += 4;
                        if (mobRect.Right > _graphics.PreferredBackBufferWidth)
                            mobRect.X = _graphics.PreferredBackBufferWidth - mobRect.Width;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        mobRect.X -= 4;
                        if (mobRect.X < 0)
                            mobRect.X = 0;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        mobRect.Y -= 4;
                        if (mobRect.Y < 0)
                            mobRect.Y = 0;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        mobRect.Y += 4;
                        if (mobRect.Bottom > cave.Height)
                            mobRect.Y = cave.Height - mobRect.Height;
                    }

                    if (gameTime.TotalGameTime.TotalSeconds - timeStamp >= timeLimit)
                        steveState = 2;
                    
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Z))
                    {
                        mobChoice = 2;
                        spawned = true;
                        mobRect = new Rectangle(0, 0, 193, 300);
                        timeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                        timeLimit = gen.Next(3, 10);
                        zombieSound.Play();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.X))
                    {
                        mobChoice = 1;
                        spawned = true;
                        mobRect = new Rectangle(0, 0, 230, 180);
                        timeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                        timeLimit = gen.Next(3, 10);
                        spiderSound.Play();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        mobChoice = 0;
                        spawned = true;
                        mobRect = new Rectangle(0, 0, 177, 310);
                        timeStamp = (float)gameTime.TotalGameTime.TotalSeconds;
                        timeLimit = gen.Next(3, 10);
                        creeperSound.Play();
                    }
                }
                if (steveState == 0)
                {
                    steveRect.X -= 3;
                    int turn = gen.Next(steveRect.Left);
                    if (turn == 0)
                        steveState = 1;
                }
                else if (steveState == 1)
                {
                    steveRect.X += 3;
                    int turn = gen.Next(_graphics.PreferredBackBufferWidth - steveRect.Right + 1);
                    if (turn == 0)
                        steveState = 0;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PapayaWhip);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(titleTexture, new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(titleFont, "MINECRAFT", new Vector2(360, 50), Color.Gray);
                _spriteBatch.DrawString(mainFont, "bad guy edition", new Vector2(520, 190), Color.LightGray);
                _spriteBatch.DrawString(mainFont, "press enter to start", new Vector2(20, 600), Color.White);
            }
            else if (screen == Screen.Main)
            {
                _spriteBatch.Draw(cave, new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(mainFont, "look at him hes just minin for diamonds", new Vector2(30, 10), Color.Red);
                if (spawned)
                {
                    _spriteBatch.DrawString(mainFont, "Quick! get him before he sees you (arrow keys)", new Vector2(10, 840), Color.Blue);
                    _spriteBatch.Draw(mob[mobChoice], mobRect, Color.White);
                }
                else
                    _spriteBatch.DrawString(mainFont, "Press Z to zombie   Press X to spider   Press C to creeper", new Vector2(10, 840), Color.Blue);
                _spriteBatch.Draw(steve[steveState], steveRect, Color.White);
            }
            else if (screen == Screen.Win)
            {
                _spriteBatch.Draw(winScreen, new Rectangle(0, 0, 1440, 810), Color.White);
                _spriteBatch.DrawString(mainFont, "lmao Gottem !!!!!!!!", new Vector2(10, 820), Color.Blue);
                _spriteBatch.DrawString(mainFont, "nooo steve died! :(", new Vector2(30, 10), Color.Red);
            }
            else
            {
                _spriteBatch.DrawString(mainFont, "haha looser.!", new Vector2(30, 10), Color.Red);
                _spriteBatch.DrawString(mainFont, "he killed you...", new Vector2(10, 820), Color.Blue);
                _spriteBatch.Draw(dab, new Rectangle(200, 200, 300, 300), Color.White);
                _spriteBatch.Draw(dab, new Rectangle(600, 200, 300, 300), Color.White);
                _spriteBatch.Draw(dab, new Rectangle(1000, 200, 300, 300), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
