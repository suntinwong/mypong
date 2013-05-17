using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WongPong {
  
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Objects
        Player player1 = new Player(1); //make player1
        Player player2 = new Player(2); //make player2
        Ball ball = new Ball();         //make the ball
        HUD hud = new HUD();            //make the Huds up display

        //other axullairy stuff
        bool p1hit = false;
        bool p2hit = false;
        bool ballhit = false;
        bool p2JustScored = false;
        bool p1JustScored = false;

        //timers
        int hittimer = 0;
        int roundtimer = 0;
        int justscoredtimer = 0;
        bool pauseOn = true;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set some basic stuff for my game
            graphics.IsFullScreen = false; //set it to full screen no
            graphics.PreferredBackBufferWidth = Defualt.Default._W; //set the screen dimension width
            graphics.PreferredBackBufferHeight = Defualt.Default._H; //set the screen dimension height
            this.Window.Title = "WongPong"; //set window title
        }

       //Initialize Function
        protected override void Initialize() {

            base.Initialize();
        }

       //Load Content Method
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hud.LoadContent(Content,GraphicsDevice);
            player1.LoadContent(Content);    //load player 1
            player2.LoadContent(Content);   //load player 2
            ball.LoadContent(Content);      //load the ball
        }

        //Unload Content Method
        protected override void UnloadContent() {
             
        }

        //Update Method
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();
            player1.Update(gameTime);    //update the player 1
            player2.Update(gameTime);    //update player 2
            ball.Update(gameTime,Content,pauseOn);       //update the ball
            manage_collisions();         //do collision logic
            Score_Check();              //check & update scores
            hud.Update(gameTime);        //update the hud
            do_animations(gameTime);    //do all game animations

            hittimer++; roundtimer++; justscoredtimer++;
            base.Update(gameTime);      //update the gametime
        }

        //Draw Method
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            //do all drawings here
            spriteBatch.Begin();
            hud.Draw();                     //draw the hud
            player1.Draw(spriteBatch,p1hit); //draw the players
            player2.Draw(spriteBatch,p2hit); //draw the players
            ball.Draw(spriteBatch,ballhit); //draw the ball

            spriteBatch.End(); 
            base.Draw(gameTime);
        }


        ////////////////////////////////////////////////////
        /////////////// Helper functions ///////////////////
        ////////////////////////////////////////////////////

        //function that manages all collisions between objects
        private void manage_collisions() {

            
            Random rand = new Random();
            if (p1hit && hittimer > 10) p1hit = false;
            if (p2hit && hittimer > 10) p2hit = false;
            if (ballhit && hittimer > 10) ballhit = false;
            
            //ball collides with player #1
            if (ball.boundingBox.Intersects(player1.boundingBox)) {
                ball.velocity.X  = rand.Next(4,8); ball.velocity.X += player1.velocity/3;
                if (ball.velocity.Y > 0) ball.velocity.Y -= 2;
                ball.velocity.Y += player1.velocity/3;
                p1hit = true; ballhit = true; hittimer = 0;
                ball.PaddleHit();
            }

            //ball collides with player #2
            if (ball.boundingBox.Intersects(player2.boundingBox)) {
                ball.velocity.X = rand.Next(-8,-4); ball.velocity.X -= player2.velocity / 3;
                if (ball.velocity.Y > 0) ball.velocity.Y-= 2;
                ball.velocity.Y += player2.velocity/3;
                p2hit = true; ballhit = true;  hittimer = 0;
                ball.PaddleHit();
            }
        }

        //function that does all additional animation
        private void do_animations(GameTime gameTime) {

            //Paddle scaling when hit
            if (p2hit & hittimer < 5) player2.scale = linear_tween((float)hittimer / 5f, 1, 1.5f);
            else if (p2hit) player2.scale = linear_tween((float)(hittimer-5)/5f, 1.5f, 1);
            if (p1hit & hittimer < 5) player1.scale = linear_tween((float)hittimer / 5f, 1, 1.5f);
            else if (p1hit) player1.scale = linear_tween((float)(hittimer - 5) / 5f, 1.5f, 1);

            //start of round, scale ball
            if (roundtimer < 25) ball.scale = linear_tween((float)roundtimer / 25f, 10f, .15f);
            else if (roundtimer < 50) ball.scale = linear_tween((float)(roundtimer - 25) / 25f, .15f, 4f);
            else if (roundtimer < 75) ball.scale = linear_tween((float)(roundtimer - 50) / 25f, 4f, 1f);
            else pauseOn = false;

            //When a person scores, make animiation
            if (!ball.isVisible && justscoredtimer < 10) {
                if (p1JustScored) {
                    hud.scale1 = linear_tween((float)justscoredtimer / 10, 1, 1.5f);
                    hud.textColor1 = Color.Green;
                }
            }
            if (!ball.isVisible && justscoredtimer < 20) {
                if (p1JustScored) {
                    hud.scale1 = linear_tween((float)(justscoredtimer-10) / 10, 1.5f, 1f);
                    hud.textColor1 = Color.White;
                }
            }
        }

        //function that checks if ball hits score line
        private void Score_Check() {

            //if ball passes player 1's goal
            if (ball.position.X < 5) {
                ball.Kill();
                hud.p2score++;
                p2JustScored = true;
                justscoredtimer = 0;
            }

            //if ball passes player 2's goal
            if (ball.position.X > Defualt.Default._W - ball.texture.Width - 5) {
                ball.Kill();
                hud.p1score++;
                p1JustScored = true;
                justscoredtimer = 0;
            }

            //if ball has been destroyed (no particles)
            if (ball.particles.Count() == 0 && !pauseOn) {
                ball.Reset();
                if (p2JustScored) { ball.position = new Vector2(Defualt.Default._W * .9f, Defualt.Default._H / 2); p2JustScored = false; ball.velocity.X *= -1; } 
                else if (p1JustScored) { ball.position = new Vector2(Defualt.Default._W * .1f, Defualt.Default._H / 2); p1JustScored = false;  }
                roundtimer = 0;
                pauseOn = true;
            }

        }

        //helper linear tweeining animation
        private float linear_tween(float t, float start, float end) {
            if (t > 1.0f) return end;
            return t * end + (1.0f - t) * start;
        }


    }
}
