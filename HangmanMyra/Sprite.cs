using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HangmanMyra
{
    internal class Sprite
    {
        public Vector2 position;
        private Texture2D texture;

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White);
        }
    }
}