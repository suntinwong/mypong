using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WongPong {
    //Ball class
    public class Ball {

        public Texture2D texture; //ball's texture
        public Vector2 position; //position of ball in the game world
        public Rectangle boundingBox; //bounding box used for collision
        public Vector2 velocity; //velocity of ball (x/y movement)
        public int directionRight; //if ball is suppose to move right
        public bool isVisible;    //if the ball is visible and to be rendered
        private Vector2 maxVelocity; //max velocity of the ball (x/y movement)

        //Constructor
        public Ball() {
            //Seed random number
            Random rand = new Random();
        
            //set other stuff
            position = new Vector2(Defualt.Default._W / 5, Defualt.Default._H/2);
            velocity = new Vector2(20, rand.Next(2,4) );
            maxVelocity = new Vector2(10, 10);
            isVisible = true;
            directionRight = 1;
            
        }

        //Load Content Method
        public void LoadContent(ContentManager content){

            //set the texture & bounding box
            texture = content.Load<Texture2D>("Artwork/ball"); 
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); 
        }

        //update method
        public void Update(GameTime gametime,ContentManager Content, bool pause) {

            if (pause) return;

            //Move the ball && update bounding box, degrade X velocity
            position.X += velocity.X; position.Y += velocity.Y;
            boundingBox.X = (int)position.X; boundingBox.Y = (int)position.Y;
            if (directionRight == 1) velocity.X -= 0.085f;
            else velocity.X += 0.085f;

            //Check wall coliisions and act accordingly
            if (position.X > Defualt.Default._W - texture.Width || position.X < 0) { velocity.X *= -1; }
            if (position.Y > Defualt.Default._H - texture.Height || position.Y < 0) { velocity.Y *= -1; }

            //set limits on the velocity
            if (velocity.X > maxVelocity.X) velocity.X = maxVelocity.X;
            else if (velocity.X < -1 * maxVelocity.X) velocity.X = -1 * maxVelocity.X;
            if (velocity.Y > maxVelocity.Y) velocity.Y = maxVelocity.Y;
            else if (velocity.Y < -1 * maxVelocity.Y) velocity.Y = -1 * maxVelocity.Y;
        }

        //draw method
        public void Draw(SpriteBatch spritebatch) {
            if(isVisible) spritebatch.Draw(texture, position, Color.White);
        }
    }

    
}
