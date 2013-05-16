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

        //other axullairy stuff
        bool p1hit = false;
        bool p2hit = false;
        int hittimer = 0;
        bool pauseOn = false;

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
            base.Update(gameTime);      //update the gametime
        }

        //Draw Method
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            //do all drawings here
            spriteBatch.Begin();

            player1.Draw(spriteBatch,p1hit); //draw the players
            player2.Draw(spriteBatch,p2hit); //draw the players
            ball.Draw(spriteBatch); //draw the ball

            spriteBatch.End(); 
            base.Draw(gameTime);
        }


        ////////////////////////////////////////////////////
        /////////////// Helper functions ///////////////////
        ////////////////////////////////////////////////////

        //function that manages all collisions between objects
        private void manage_collisions() {

            hittimer++;

            if (p1hit && hittimer > 10) p1hit = false;
            if (p2hit && hittimer > 10) p2hit = false;
            
            //ball collides with player #1
            if (ball.boundingBox.Intersects(player1.boundingBox)) {
                ball.velocity.X *= -1; ball.velocity.X += 20;
                ball.velocity.Y *= 1;
                ball.directionRight = -1;
                p1hit = true; hittimer = 0;
            }

            //ball collides with player #2
            if (ball.boundingBox.Intersects(player2.boundingBox)) {
                ball.velocity.X *= -1; ball.velocity.X -= 20;
                ball.velocity.Y *= 1;
                ball.directionRight = 1;
                p2hit = true; hittimer = 0;
            }
        }


    }
}
