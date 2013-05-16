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
        Ball ball = new Ball();


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


            ball.LoadContent(Content); //load the ball
        }

        //Unload Content Method
        protected override void UnloadContent() {
             
        }

        //Update Method
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();
            ball.Update(gameTime); //update the ball
            base.Update(gameTime);
        }

        //Draw Method
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            //do all drawings here
            spriteBatch.Begin(); 

            
            ball.Draw(spriteBatch); //draw the ball

            spriteBatch.End(); 
            base.Draw(gameTime);
        }
    }
}
