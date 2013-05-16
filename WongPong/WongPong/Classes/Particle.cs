using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WongPong.Classes {
    public class Particle {

        public Texture2D texture;     //Particle's texture
        public Vector2 position;      //position of Player in the game world
        public Vector2 velocity;      //Particle's velocity

        //particle constructor
        Particle(ContentManager Content, int x, int y) {
            position = new Vector2(x, y);
            texture = Content.Load<Texture2D>("Artwork/ball_particle");
        }

        //Update method
        void Update() {
            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        //Draw method
        void Draw(SpriteBatch spritebatch) {
            spritebatch.Draw(texture, position, Color.Blue);
        }

    }
}
