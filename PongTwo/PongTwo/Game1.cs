using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace PongTwo
{
     /// <summary>
     /// This is the main type for your game
     /// </summary>
     public class Game1 : Microsoft.Xna.Framework.Game
     {
          public enum GameState { Start, InGame, GameOver, AI };
          enum PlayerCount { One, Two };

          GameState currentGS = GameState.Start;

          public Random random { get; private set; }

          bool up_pressed = false;
          bool down_pressed = false;
          bool up_pressed2 = false;
          bool down_pressed2 = false;
          int temp;

          string winner;
          GraphicsDeviceManager graphics;
          SpriteBatch spriteBatch;


          SoundEffect paddlehit;
          SoundEffect fanfare;
          SoundEffect loser;
          SoundEffect boo;
          SoundEffect applause;
          SoundEffect boxing_bell;

          Texture2D texture;
          Texture2D texture2;
          Texture2D ball;
          Texture2D arrow;
          Texture2D arrow_down;
          Texture2D arrow_pressed;
          Texture2D arrow_down_pressed;
          SpriteFont scoreFont;
          SpriteFont InGameFont;

          int scoreOrange = 0;
          int scoreRed = 0;
          int scoreLimit = 10;
          int centerx, centery;

          Point ringsFrameSize = new Point(20, 100);
          Point ringsCurrentFrame = new Point(0, 0);
          //Point ringsSheetSize = new Point(6, 8);

          Point rings2FrameSize = new Point(20, 100);
          Point rings2CurrentFrame = new Point(0, 0);
          //Point rings2SheetSize = new Point(6, 8);

          Point skullFrameSize = new Point(42, 42);
          Point skullCurrentFrame = new Point(0, 0);
          //Point skullSheetSize = new Point(6, 8);

          Vector2 pos1 = new Vector2(100, 250);
          Vector2 pos2 = new Vector2(650, 250);
          Vector2 pos3 = new Vector2(400, 250);

          Color fontcolor1p = Color.AntiqueWhite;
          Color fontcolor2p = Color.AntiqueWhite;
          Color fontcolor_score_red = Color.AntiqueWhite;
          Color fontcolor_score_orange = Color.AntiqueWhite;

          bool isColliding = false;

          float speed1 = 4f;
          float speed2 = 4f;
          float speed3 = 2f;

          public Game1()
          {
               graphics = new GraphicsDeviceManager(this);
               graphics.IsFullScreen = true; 
               Content.RootDirectory = "Content";

               // Frame rate is 30 fps by default for Windows Phone.
               TargetElapsedTime = TimeSpan.FromTicks(288777);

               // set up touch gesture support: make vertical drag and flick the
               // gestures that we're interested in.
               TouchPanel.EnabledGestures = GestureType.DoubleTap | GestureType.Hold;

               random = new Random();

               centerx = Window.ClientBounds.Height / 2;
               centery = Window.ClientBounds.Width / 2;


          }

          /// <summary>
          /// Allows the game to perform any initialization it needs to before starting to run.
          /// This is where it can query for any required services and load any non-graphic
          /// related content.  Calling base.Initialize will enumerate through any components
          /// and initialize them as well.
          /// </summary>
          protected override void Initialize()
          {
               // TODO: Add your initialization logic here
               base.Initialize();
          }

          /// <summary>
          /// LoadContent will be called once per game and is the place to load
          /// all of your content.
          /// </summary>
          protected override void LoadContent()
          {
               // Create a new SpriteBatch, which can be used to draw textures.
               spriteBatch = new SpriteBatch(GraphicsDevice);
               scoreFont = Content.Load<SpriteFont>(@"fonts\score");
               InGameFont = Content.Load<SpriteFont>(@"fonts\InGame");
               texture = Content.Load<Texture2D>(@"Images/RedRectangle");
               texture2 = Content.Load<Texture2D>(@"Images/OrangeRectangle");
               ball = Content.Load<Texture2D>(@"Images/WhiteBall");
               arrow = Content.Load<Texture2D>(@"Images/arrow");
               arrow_down = Content.Load<Texture2D>(@"Images/arrow_down");
               arrow_pressed = Content.Load<Texture2D>(@"Images/arrow_pressed");
               arrow_down_pressed = Content.Load<Texture2D>(@"Images/arrow_down_pressed");
               paddlehit = Content.Load<SoundEffect>(@"Audio\blip");
               fanfare = Content.Load<SoundEffect>(@"Audio\fanfare");
               loser = Content.Load<SoundEffect>(@"Audio\loser");
               boo = Content.Load<SoundEffect>(@"Audio\boo");
               applause = Content.Load<SoundEffect>(@"Audio\applause");
               boxing_bell = Content.Load<SoundEffect>(@"Audio\boxing_bell");
               
               // TODO: use this.Content to load your game content here
          }

          /// <summary>
          /// UnloadContent will be called once per game and is the place to unload
          /// all content.
          /// </summary>
          protected override void UnloadContent()
          {
               // TODO: Unload any non ContentManager content here
          }


          private void comenzar()
          {
               fontcolor1p = Color.AntiqueWhite;
               fontcolor2p = Color.AntiqueWhite;

               fontcolor_score_red = Color.AntiqueWhite;
               fontcolor_score_orange = Color.AntiqueWhite;

               speed1 = 4f;
               speed2 = 4f;
               scoreOrange = 0;
               scoreRed = 0;

               pos3.X = centerx - 20;
               pos3.Y = centery;
               speed2 *= -1;

               pos1.Y = centery;
               pos2.Y = centery;

          }

          /// <summary>
          /// Allows the game to run logic such as updating the world,
          /// checking for collisions, gathering input, and playing audio.
          /// </summary>
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Update(GameTime gameTime)
          {
               // Allows the game to exit

               switch (currentGS)
               {
                    case GameState.Start:
                         {

                              if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                                   this.Exit();

                              comenzar();

                              // Did the user press the attack button on the touch panel?
                              TouchCollection touchCollection = TouchPanel.GetState();
                              foreach (TouchLocation touchLocation in touchCollection)
                              {
                                   if (touchLocation.Position.X < 380)
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)
                                        {

                                             fontcolor1p = Color.DarkRed;

                                             while (TouchPanel.IsGestureAvailable)
                                             {
                                                  GestureSample gs = TouchPanel.ReadGesture();
                                                  switch (gs.GestureType)
                                                  {
                                                            
                                                       case GestureType.DoubleTap:
                                                       case GestureType.Hold:
                                                            fontcolor1p = Color.DarkRed;
                                                            boxing_bell.Play();
                                                            currentGS = GameState.AI;
                                                            break;
                                                  }
                                             }
                                        }

                                   if (touchLocation.Position.X > 380)
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)
                                        {

                                             fontcolor2p = Color.DarkRed;
                                             while (TouchPanel.IsGestureAvailable)
                                             {
                                                  GestureSample gs = TouchPanel.ReadGesture();
                                                  switch (gs.GestureType)
                                                  {
                                                       case GestureType.DoubleTap:
                                                       case GestureType.Hold:
                                                            fontcolor2p = Color.DarkRed;
                                                            boxing_bell.Play();
                                                            currentGS = GameState.InGame;
                                                            break;
                                                  }
                                             }
                                        }
                              }
                              break;
                         }

                    case GameState.GameOver:
                         {
                              if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                              currentGS = GameState.Start;
                              
                              speed1 = 4f;
                              speed2 = 4f;
                              scoreOrange = 0;
                              scoreRed = 0;


                                // Did the user press the attack button on the touch panel?
                              TouchCollection touchCollection = TouchPanel.GetState();
                              foreach (TouchLocation touchLocation in touchCollection)
                              {
                                   if (touchLocation.Position.X > Window.ClientBounds.Height / 2 - 100 &&
                                        touchLocation.Position.X < Window.ClientBounds.Height / 2 + 100
                              && (touchLocation.Position.Y > Window.ClientBounds.Width / 2 - 100)
                                        && (touchLocation.Position.Y < Window.ClientBounds.Width / 2 + 100) )
                                                  while (TouchPanel.IsGestureAvailable)
                                                  {
                                                       GestureSample gs = TouchPanel.ReadGesture();
                                                       switch (gs.GestureType)
                                                       {
                                                            case GestureType.DoubleTap:
                                                                 currentGS = GameState.Start;
                                                                 break;

                                                            case GestureType.Hold:
                                                                currentGS = GameState.Start;
                                                                 break;
                                                       }
                                                  }
                              }

                              break;
                         }

                    case GameState.InGame:
                         {
                              if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                                    currentGS  = GameState.Start;

                              pos3.Y += speed1;
                              if (pos3.Y > Window.ClientBounds.Width - skullFrameSize.X - 10 || pos3.Y < 10)
                                   speed1 *= -1;

                              pos3.X += speed2;
                              if (pos3.X > 645 || pos3.X < 99)
                              {
                                   if (random.Next(100) > 50)
                                   {
                                        speed1 = random.Next(7, 12);
                                   }
                                   else
                                   {
                                        speed1 = random.Next(-12, -7);
                                   }

                                   if (random.Next(100) > 50)
                                   {
                                        speed2 = random.Next(7, 12);
                                   }
                                   else
                                   {
                                        speed2 = random.Next(-12, -7);
                                   }
                                   if (pos3.X < 100)
                                   {
                                        scoreOrange++;
                                        fontcolor_score_orange = Color.Yellow;
                                        fontcolor_score_red = Color.AntiqueWhite;
                                        if (random.Next(100) > 50)
                                             loser.Play();
                                        else fanfare.Play();

                                   }
                                   else
                                   {
                                        scoreRed++;
                                        fontcolor_score_red = Color.Yellow;
                                        fontcolor_score_orange = Color.AntiqueWhite;
                                        if (random.Next(100) > 50)
                                             loser.Play();
                                        else fanfare.Play();
                                   }

                                   pos3.X = centerx - 20;
                                   pos3.Y = centery;
                                   speed2 *= -1;

                                   if (scoreRed == scoreLimit)
                                   {
                                        winner = "Red";
                                        currentGS = GameState.GameOver;
                                   }
                                   else if (scoreOrange == scoreLimit)
                                   {
                                        winner = "Orange";
                                        currentGS = GameState.GameOver;
                                   }
                              }

                              up_pressed = false;
                              down_pressed = false;
                              up_pressed2 = false;
                              down_pressed2 = false;

                              // Did the user press the attack button on the touch panel?
                              TouchCollection touchCollection = TouchPanel.GetState();
                              foreach (TouchLocation touchLocation in touchCollection)
                              {
                                   if (touchLocation.Position.X < Window.ClientBounds.Width / 2
                              && (touchLocation.Position.Y > Window.ClientBounds.Height / 2)

                              )
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)
                                             if (pos1.Y < Window.ClientBounds.Width - 110)
                                             {

                                             up_pressed = true;
                                             pos1.Y++;
                                             pos1.Y++;
                                             pos1.Y++;
                                             pos1.Y++;
                                             pos1.Y++;

                                        }

                                   if (touchLocation.Position.X < Window.ClientBounds.Width / 2
                                  && (touchLocation.Position.Y < Window.ClientBounds.Height / 2)

                                  )
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)
                                        if (pos1.Y > 50)
                                             {
                                             down_pressed = true;
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                        }



                                   if (touchLocation.Position.X > Window.ClientBounds.Width / 2
                          && (touchLocation.Position.Y > Window.ClientBounds.Height / 2))
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)                                   
                                             if (pos2.Y < Window.ClientBounds.Width - 110)
                                             {
                                                  up_pressed2 = true;
                                                  pos2.Y++;
                                                  pos2.Y++;
                                                  pos2.Y++;
                                                  pos2.Y++;
                                                  pos2.Y++;
                                             }

                                   if (touchLocation.Position.X > Window.ClientBounds.Width / 2
                                  && (touchLocation.Position.Y < Window.ClientBounds.Height / 2))
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)
                                             if (pos2.Y > 50)
                                             {
                                             down_pressed2 = true;
                                             pos2.Y--;
                                             pos2.Y--;
                                             pos2.Y--;
                                             pos2.Y--;
                                             pos2.Y--;
                                        }
                              }

                              if (pos3.X < 500 && pos3.X > 300)
                              {
                                   isColliding = false;
                              }

                              if (Collide() && !isColliding)
                              {
                                   Collading();
                              }
                         }
                         break;

                    case GameState.AI:
                         {
                              if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                                   currentGS = GameState.Start;

                              pos3.Y += speed1;
                              if (pos3.Y > Window.ClientBounds.Width - skullFrameSize.X - 10 || pos3.Y < 10)
                                   speed1 *= -1;

                              pos3.X += speed2;
                              if (pos3.X > 645 || pos3.X < 99)
                              {
                                   if (random.Next(100) > 50)
                                   {
                                        speed1 = random.Next(7, 12);
                                   }
                                   else
                                   {
                                        speed1 = random.Next(-12, -7);
                                   }

                                   if (random.Next(100) > 50)
                                   {
                                        speed2 = random.Next(7, 12);
                                   }
                                   else
                                   {
                                        speed2 = random.Next(-12, -7);
                                   }

                                   if (pos3.X < 100)
                                   {
                                        loser.Play();
                                        scoreOrange++;
                                        fontcolor_score_orange = Color.Yellow;
                                        fontcolor_score_red = Color.AntiqueWhite;
                                        
                                   }
                                   else
                                   {
                                        fanfare.Play();
                                        scoreRed++;
                                        fontcolor_score_red = Color.Yellow;
                                        fontcolor_score_orange = Color.AntiqueWhite;
                                   }

                                   pos3.X = centerx - 20;
                                   pos3.Y = centery;
                                   speed2 *= -1;

                                   if (scoreRed == scoreLimit)
                                   {
                                        winner = "You";
                                        applause.Play();
                                        currentGS = GameState.GameOver;
                                   }
                                   else if (scoreOrange == scoreLimit)
                                   {
                                        winner = "Your Phone";
                                        boo.Play();
                                        currentGS = GameState.GameOver;
                                   }
                              }

                              up_pressed = false;
                              down_pressed = false;

                              // Did the user press the attack button on the touch panel?
                              TouchCollection touchCollection = TouchPanel.GetState();
                              foreach (TouchLocation touchLocation in touchCollection)
                              {

                                   if (touchLocation.Position.X < Window.ClientBounds.Width / 2
                                        && (touchLocation.Position.Y > Window.ClientBounds.Height / 2)
                                        
                                        )
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)
                                        {
                                             if (pos1.Y < Window.ClientBounds.Width - 110)
                                             {
                                                  pos1.Y++;
                                                  pos1.Y++;
                                                  pos1.Y++;
                                                  pos1.Y++;
                                                  pos1.Y++;
                                                  pos1.Y++;
                                                  pos1.Y++;
                                                  up_pressed = true;
                                             }
                                        }

                                   if (touchLocation.Position.X < Window.ClientBounds.Width / 2
                                  && (touchLocation.Position.Y < Window.ClientBounds.Height / 2)
                                        
                                  )
                                        if (touchLocation.State == TouchLocationState.Pressed ||
                                             touchLocation.State == TouchLocationState.Moved)
                                        {

                                             if (pos1.Y > 50)
                                             {
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                             pos1.Y--;
                                             down_pressed = true;
                                             }
                                        }
                              }



                              if (pos3.Y > 50)
                                   if (pos3.Y != pos2.Y && pos3.Y > pos2.Y && pos2.Y < Window.ClientBounds.Width - 120 )
                                   {
                                        pos2.Y++;
                                        pos2.Y++;
                                        pos2.Y++;
                                        pos2.Y++;
                                        pos2.Y++;
                                        pos2.Y++;
                                        pos2.Y++;
                                        pos2.Y++;
                                      

                                   }

                              if (pos3.Y < 420)
                                   if (pos3.Y != pos2.Y && pos3.Y < pos2.Y && pos2.Y > 60)
                                   {
                                        pos2.Y--;
                                        pos2.Y--;
                                        pos2.Y--;
                                        pos2.Y--;
                                        pos2.Y--;
                                        pos2.Y--;
                                        pos2.Y--;
                                        pos2.Y--;
                                       
                                   }

                              if (pos3.X < 500 && pos3.X > 300)
                              {
                                   isColliding = false;
                              }

                              if (Collide() && !isColliding)
                              {
                                   Collading();
                              }
                         }
                         break;
               }

               // TODO: Add your update logic here
               base.Update(gameTime);
          }

          private void Collading()
          {
               isColliding = true;
               speed2 *= -1;

               temp = random.Next(100);

               if (temp < 20)
                    speed1 *= -1;

               if (speed1 > 0 && speed1 < 10)
               {
                    speed1 = random.Next(7, 13);
               }
               else if (speed1 < 0 && speed1 > -10)
               {
                    speed1 = random.Next(-12, -7);
               }

               if (speed2 > 0 && speed2 < 10)
               {
                    speed2 = random.Next(7, 13);
               }
               else if (speed2 < 0 && speed2 > -10)
               {
                    speed2 = random.Next(-13, -7);
               }
          }

          /// <summary>
          /// This is called when the game should draw itself.
          /// </summary>
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Draw(GameTime gameTime)
          {
               switch (currentGS)
               {
                    case GameState.Start:
                         {
                              GraphicsDevice.Clear(Color.Black);

                              // Draw text for intro splash screen
                              spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                              string text = "Just Another Pong Clone";

                              spriteBatch.DrawString(scoreFont, text,
                                  new Vector2((Window.ClientBounds.Height / 2)
                                  - (scoreFont.MeasureString(text).X / 2),
                                 90),
                                  Color.AntiqueWhite);

                              text = "Hold or Double Tap to Begin";
                              spriteBatch.DrawString(scoreFont, text,
                                  new Vector2((Window.ClientBounds.Height / 2)
                                  - (scoreFont.MeasureString(text).X / 2), 150),
                                  Color.AntiqueWhite);

                              text = "Single \nPlayer";
                              spriteBatch.DrawString(scoreFont, text,
                                  new Vector2(150,
                                  ((Window.ClientBounds.Width / 3) * 2) - (scoreFont.MeasureString(text).Y / 2)),
                                  fontcolor1p);

                              text = "Multiplayer";
                              spriteBatch.DrawString(scoreFont, text,
                                  new Vector2(420,
                                  ((Window.ClientBounds.Width / 3) * 2) - scoreFont.MeasureString(text).Y / 2),
                                  fontcolor2p);

                              spriteBatch.End();
                              break;
                         }

                    case GameState.GameOver:
                         {
                              GraphicsDevice.Clear(Color.Black);
                              spriteBatch.Begin();
                              string gameover = "Game Over! ";
                              spriteBatch.DrawString(scoreFont, gameover,
                                  new Vector2((Window.ClientBounds.Height / 2)
                                  - (scoreFont.MeasureString(gameover).X / 2),
                                  (Window.ClientBounds.Width / 2)
                                  - (scoreFont.MeasureString(gameover).Y / 2)),
                                  Color.AntiqueWhite);

                              gameover = winner + " Won";
                              spriteBatch.DrawString(scoreFont, gameover,
                                  new Vector2((Window.ClientBounds.Height / 2)
                                  - (scoreFont.MeasureString(gameover).X / 2 + 20),
                                  (Window.ClientBounds.Width / 2)
                                  - (scoreFont.MeasureString(gameover).Y / 2) + 50),
                                  Color.AntiqueWhite);

                              spriteBatch.End();
                              break;
                         }

                    case GameState.InGame:
                         {
                              GraphicsDevice.Clear(Color.Black);
                              // TODO: Add your drawing code here
                              spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                              DrawBallAndPaddles();
                              DrawP2Arrows();
                              DrawText();
                              spriteBatch.End();
                              break;
                         }


                    case GameState.AI:
                         {
                              GraphicsDevice.Clear(Color.Black);

                              // TODO: Add your drawing code here
                              spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                              DrawBallAndPaddles();
                              DrawP1Arrows();
                              DrawText();
                              spriteBatch.End();
                              break;
                         }
               }

               base.Draw(gameTime);
          }

          private void DrawBallAndPaddles()
          {
               spriteBatch.Draw(texture,
                                  pos1,
                                  new Rectangle(ringsCurrentFrame.X * ringsFrameSize.X,
                                  ringsCurrentFrame.Y * ringsFrameSize.Y,
                                  ringsFrameSize.X,
                                  ringsFrameSize.Y),
                                  Color.White,
                                  0,
                                  Vector2.Zero,
                                  1,
                                  SpriteEffects.None,
                                  0);

               spriteBatch.Draw(texture2,
                   pos2,
                  new Rectangle(rings2CurrentFrame.X * rings2FrameSize.X,
                   rings2CurrentFrame.Y * rings2FrameSize.Y,
                   rings2FrameSize.X,
                   rings2FrameSize.Y),
                   Color.White,
                   0,
                   Vector2.Zero,
                   1,
                   SpriteEffects.None,
                   0);

               spriteBatch.Draw(ball,
                      pos3,
                      new Rectangle(skullCurrentFrame.X * skullFrameSize.X,
                      skullCurrentFrame.Y * skullFrameSize.Y,
                      skullFrameSize.X,
                      skullFrameSize.Y),
                      Color.White,
                      0,
                      Vector2.Zero,
                    1,
                      SpriteEffects.None,
                      0);

          }

          private void DrawP1Arrows()
          {
               if (down_pressed)
               {
                    spriteBatch.Draw(arrow_pressed,
                   new Vector2(45, 50),
                   null,
                   Color.White,
                   0,
                   Vector2.Zero,
                   (float)0.9,
                   SpriteEffects.None,
                   0);
               }
               else
               {
                    spriteBatch.Draw(arrow,
                       new Vector2(45, 50),
                        null,
                       Color.White,
                       0,
                       Vector2.Zero,
                      (float)0.9,
                       SpriteEffects.None,
                       0);
               }

               if (up_pressed)
               {
                    spriteBatch.Draw(arrow_down_pressed,
                new Vector2(45, Window.ClientBounds.Width - 60),
                 null,
                Color.White,
                0,
                Vector2.Zero,
                (float)0.9,
                SpriteEffects.None,
                0);

               }
               else
               {


                    spriteBatch.Draw(arrow_down,
                    new Vector2(45, Window.ClientBounds.Width - 60),
                     null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    (float)0.9,
                    SpriteEffects.None,
                    0);

               }

          }

          private void DrawP2Arrows()
          {

               DrawP1Arrows();

               if (down_pressed2)
               {
                    spriteBatch.Draw(arrow_pressed,
                    new Vector2(Window.ClientBounds.Height - 50, 45),
                     null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    (float)0.9,
                    SpriteEffects.None,
                    0);

               }
               else
               {

                    spriteBatch.Draw(arrow,
                    new Vector2(Window.ClientBounds.Height - 50, 45),
                     null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    (float)0.9,
                    SpriteEffects.None,
                    0);
               }


               if (up_pressed2)
               {
                    spriteBatch.Draw(arrow_down_pressed,
                                                  new Vector2(Window.ClientBounds.Height - 50, Window.ClientBounds.Width - 60),
                                                   null,
                                                  Color.White,
                                                  0,
                                                  Vector2.Zero,
                                                  (float)0.9,
                                                  SpriteEffects.None,
                                                  0);
               }
               else
               {
                    spriteBatch.Draw(arrow_down,
                    new Vector2(Window.ClientBounds.Height - 50, Window.ClientBounds.Width - 60),
                     null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    (float)0.9,
                    SpriteEffects.None,
                    0);
               }


          }

          private void DrawText()
          {
               spriteBatch.DrawString(InGameFont, scoreOrange.ToString(),
                                new Vector2(550, 125),
                                fontcolor_score_orange);

               spriteBatch.DrawString(InGameFont, scoreRed.ToString(),
                   new Vector2(200, 125),
                   fontcolor_score_red);

          }

          protected bool Collide()
          {
               Rectangle ringsRect = new Rectangle(
                   (int)pos1.X,
                   (int)pos1.Y,
                   ringsFrameSize.X,
                   ringsFrameSize.Y);

               Rectangle rings2Rect = new Rectangle(
                  (int)pos2.X,
                  (int)pos2.Y,
                  rings2FrameSize.X,
                  rings2FrameSize.Y);

               Rectangle skullRect = new Rectangle(
                   (int)pos3.X,
                   (int)pos3.Y,
                   skullFrameSize.X,
                   skullFrameSize.Y);

               if (ringsRect.Intersects(skullRect))
               {
                    paddlehit.Play();
                    return true;
               }
               if (rings2Rect.Intersects(skullRect))
               {
                    paddlehit.Play();
                    return true;
               }
               return false;
          }
     }
}
