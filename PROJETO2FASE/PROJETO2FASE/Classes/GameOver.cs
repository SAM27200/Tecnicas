using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PROJETO2FASE.Classes
{
    internal class GameOver
    {
        private SpriteFont Fonte;
        GraphicsDevice Graphicsdevice;

        public GameOver(SpriteFont fonte, GraphicsDevice graphicsdevice)
        {
            Fonte = fonte;  
            Graphicsdevice = graphicsdevice;
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                System.Environment.Exit(0); // sair do jogo no esc
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Fonte, "GAME OVER", new Vector2(100, 100), Color.Black);
        }
    }
}

