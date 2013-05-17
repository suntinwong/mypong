using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;
using KinectTracking;

namespace WongPong {

    //Ball class
    public class Player {

        public Texture2D texture;     //Player's texture
        public Vector2 position;      //position of Player in the game world
        public Rectangle boundingBox; //bounding box used for collision
        public int moveSpeed;        //Player's move max speed
        public int velocity;        //player's current velocity
        public int score;            //player's score
        private int type;            //player's type # (player 1 or player 2?)
        private Kinect kinect;

        //Constructor
        public Player(int newtype = 1) {

            //set important attributes
            moveSpeed = 12;
            
            //other attributes
            velocity = 0;
            type = newtype;
            score = 0;
            kinect = new Kinect();  //Make kinect object

        }

        //Load Content Method
        public void LoadContent(ContentManager content) {

            //set the texture & bounding box
            texture = content.Load<Texture2D>("Artwork/player");
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //set initial position
            if (type == 1) position = new Vector2(15, Defualt.Default._H / 2 - texture.Height/2);
            else if (type == 2) position = new Vector2(Defualt.Default._W - texture.Width - 15, Defualt.Default._H / 2 - texture.Height/2);

            //initialize kinect stuff if needed
            if (Defualt.Default.UsingKinect) {
                kinect = new Kinect();
                kinect.initialize();
            }
        }

        //Update method
        public void Update(GameTime gametime) {

            //update boundingBox & other thigns
            boundingBox.X = (int)position.X; boundingBox.Y = (int)position.Y;
            bool moveUp = false;
            bool moveDown = false;
            velocity = 0;

            //When we're using mice and keyboard (no kinect controls)
            if (!Defualt.Default.UsingKinect) {
                KeyboardState keystate = Keyboard.GetState();
                if (type == 1 && keystate.IsKeyDown(Keys.W)) moveUp = true;
                if (type == 1 && keystate.IsKeyDown(Keys.S)) moveDown = true;
                if (type == 2 && keystate.IsKeyDown(Keys.Up)) moveUp = true;
                if (type == 2 && keystate.IsKeyDown(Keys.Down)) moveDown = true; 
            }

            //When kinect is enabled (only use kinect controls)
            else {

                //when player isnt on the screen
                if (kinect.player == null) return;
            }

            //Move player if applicable & keep them on screen
            if (moveUp)  {position.Y -= moveSpeed; velocity = moveSpeed * -1; }
            if (moveDown) {position.Y += moveSpeed; velocity = moveSpeed;}
            if (position.Y <= 0) position.Y = 0;
            if (position.Y >= Defualt.Default._H - texture.Height) position.Y = Defualt.Default._H - texture.Height;
        }

        //draw method
        public void Draw(SpriteBatch spritebatch,bool hit = false) {
            if (!hit) spritebatch.Draw(texture, position, Color.Crimson);
            else spritebatch.Draw(texture, position, Color.Gold);
        }
    }
}
