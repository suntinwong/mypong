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
        public Vector2 position, origin;   //position & center of Player in the game world
        public Rectangle boundingBox; //bounding box used for collision
        public int moveSpeed;        //Player's move max speed
        public int velocity;        //player's current velocity
        public Color color;         //the paddle's color
        private int type;            //player's type # (player 1 or player 2?)
        public float rotationAngle;          //object's rotation
        public float scale;             //object's scale

        //Constructor
        public Player(Color newcolor, int newtype = 1) {

            //set important attributes
            moveSpeed = 12;
            color = newcolor;

            //other attributes
            velocity = 0;
            type = newtype;
            rotationAngle = 0;
            scale = 1.0f;
            
            origin = new Vector2(0,0);
        }

        //Load Content Method
        public void LoadContent(ContentManager content) {

            //texture, set initial position & bounding box
            texture = content.Load<Texture2D>("Artwork/player");
            if (type == 1) position = new Vector2(30, Defualt.Default._H / 2 - texture.Height / 2);
            else if (type == 2) position = new Vector2(Defualt.Default._W - texture.Width - 30, Defualt.Default._H / 2 - texture.Height / 2);
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            origin.X = (texture.Width / 2); origin.Y = (texture.Height / 2);

            
        }

        //Update method
        public void Update(GameTime gametime,Skeleton player) {

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
                if (player == null) return;

                //Make the paddle move to the player's position
                Joint joint = player.Joints[JointType.HandRight];
                Vector2 jointPosition = new Vector2(joint.Position.X, joint.Position.Y);
                double handpos = Math.Abs((jointPosition.Y * 2) * (Defualt.Default._W) - Defualt.Default._H / 2);

                //Figure out if we should move up or down
                if (handpos > position.Y +moveSpeed) moveDown = true;
                else if (handpos < position.Y -moveSpeed) moveUp = true;
            }

            //Move player if applicable & keep them on screen
            if (moveUp) { position.Y -= moveSpeed; velocity = moveSpeed * -1; }
            if (moveDown) { position.Y += moveSpeed; velocity = moveSpeed; }
            if (position.Y <= 0) position.Y = 0;
            if (position.Y >= Defualt.Default._H - texture.Height) position.Y = Defualt.Default._H - texture.Height;
        }

        //draw method
        public void Draw(SpriteBatch spritebatch, bool hit = false) {
            Color c = color;
            if(hit) c = Color.Gold;

            spritebatch.Draw(texture, position + origin, null, c, rotationAngle, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
