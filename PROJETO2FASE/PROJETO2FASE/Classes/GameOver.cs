﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        {   // Tela 100% proposital
            spriteBatch.DrawString(Fonte, "GAME OVER", new Vector2(50, 100), Color.Black);
            spriteBatch.DrawString(Fonte, "Não é um bug, é uma feature.", new Vector2(100, 300), Color.Black);

        }
    }
}

