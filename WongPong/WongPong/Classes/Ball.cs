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

        //Constructor
        public Ball() {
            //Seed random number
            Random rand = new Random();
            

            //set the starting position & starting velocity
            position = new Vector2(Defualt.Default._W / 2, Defualt.Default._H/2);
            velocity = new Vector2(rand.Next(-30, 30)/10, rand.Next(10,40)/10 );
            
        }

        //Load Content Method
        public void LoadContent(ContentManager content){

            //set the texture & bounding box
            texture = content.Load<Texture2D>("Artwork/ball"); 
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); 
        }

        //update method
        public void Update(GameTime gametime) {

            //Move the ball
            position.X += velocity.X; position.Y += velocity.Y;

            //Check wall coliisions and act accordingly
            if (position.X > Defualt.Default._W - texture.Width|| position.X < 0) {velocity.X *= -1};
            if (position.Y > Defualt.Default._H - texture.Height || position.Y < 0) {velocity.Y *= -1};
        }

        //draw method
        public void Draw(SpriteBatch spritebatch) {
            spritebatch.Draw(texture, position, Color.White);
        }
    }

    
}
