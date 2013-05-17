using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WongPong {
    public class Ball {

        public Texture2D texture; //ball's texture
        private Texture2D particleTexture; //particles' texture
        public Vector2 position; //position of ball in the game world
        public Rectangle boundingBox; //bounding box used for collision
        public Vector2 velocity; //velocity of ball (x/y movement)
        public int directionRight; //if ball is suppose to move right
        public bool isVisible;    //if the ball is visible and to be rendered
        private Vector2 maxVelocity; //max velocity of the ball (x/y movement)
        public int maxParticleLife; //life of each generated particles
        public List<Particle> particles;

        //Constructor
        public Ball() {
            Random rand = new Random();  //Seed random number

            //Set important attributes
            maxParticleLife = 20;
            position = new Vector2(Defualt.Default._W / 5, Defualt.Default._H / 2);
            velocity = new Vector2(5, 0);
            maxVelocity = new Vector2(12, 12);
            
            //set other stuff
            isVisible = true;
            particles = new List<Particle>();
            
        }

        //Load Content Method
        public void LoadContent(ContentManager content){

            //set the texture & bounding box
            texture = content.Load<Texture2D>("Artwork/ball");
            particleTexture = content.Load<Texture2D>("Artwork/ball_particle");
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); 
        }

        //update method
        public void Update(GameTime gametime,ContentManager Content, bool pause) {

            //Get out if game is paused
            if (pause) return;

            //Update curent particles
            for (int i = 0; i < particles.Count(); i++) {
                particles[i].Update(gametime);
                if (particles[i].life > maxParticleLife) particles.RemoveAt(i);
            }

            //Dont update if not visible
            if (!isVisible) return;

            //Make particle trail
            Random rand = new Random();
            particles.Add(new Particle(particleTexture,
                (int)position.X + texture.Width / 2 + rand.Next(-4, 4),
                (int)position.Y + texture.Width / 2 + rand.Next(-4, 4),
                new Vector2(rand.Next(-1, 1), rand.Next(-1, 1)),
                Color.Yellow));

            //Move the ball && update bounding box, degrade X velocity
            position.X += velocity.X; position.Y += velocity.Y;
            boundingBox.X = (int)position.X; boundingBox.Y = (int)position.Y;

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

            //draw the ball
            if(isVisible) spritebatch.Draw(texture, position, Color.Yellow);

            //draw all particles
            for (int i = 0; i < particles.Count(); i++) particles[i].Draw(spritebatch);
        }

        //destroy the ball at current location, make particle effect
        public void Kill() {
            
            //Make explision
            Random rand = new Random();
            for (int i = 0; i < 50; i++) {
                Particle p = new Particle(particleTexture,
                    (int)position.X,(int)position.Y,
                    new Vector2(rand.Next(-1000,1000)/100f,rand.Next(-1000,1000)/100f),
                    Color.Orange);
                p.life = - 200;
                particles.Add(p);
            }

            //set attributes
            isVisible = false;
            velocity = new Vector2(0, 0);
            position = new Vector2(Defualt.Default._W / 2, Defualt.Default._H / 2);
            boundingBox.X = (int)position.X; boundingBox.Y = (int)position.Y;

        }

        //reset the ball
        public void Reset() {
            particles.Clear();
            isVisible = true;
            position = new Vector2(Defualt.Default._W / 5, Defualt.Default._H / 2);
            velocity = new Vector2(5, 0);
        }
    }

  

    
}
