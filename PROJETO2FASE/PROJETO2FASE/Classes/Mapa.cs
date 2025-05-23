using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PROJETO2FASE.Classes
{
    internal class Mapa
    {
        public List<Rectangle> plataformas;
        private Texture2D texture;

        public Mapa(GraphicsDevice graphicsDevice, ContentManager content) // adicionei o content manager
        {
            texture = content.Load<Texture2D>("plataforma");  // dei load a imagem de plataforma no content

            plataformas = new List<Rectangle> {
                new Rectangle(-40, 400, 1200, 110),       // coordenas da plataforma 1º - eixo do x 2º eixo do y 3º largura 4º altura AVISO: AS COORDENAS FUNCIONAM AO CONTRARIO!!!
                new Rectangle(300, 300, 200, 30),
                new Rectangle(600, 200, 150, 30),
                new Rectangle(-2480, 400, 2300, 110),
                new Rectangle(-1080, 296, 400, 35),
                new Rectangle(-2480, 196, 1370, 35),
                new Rectangle(900, 200, 600, 30),
                new Rectangle(-2000, 96, 400, 45),
                new Rectangle(-1500, 16, 950, 100),
                new Rectangle(1500, 300, 400, 30),
                new Rectangle(1250, 0, 150, 30),
                new Rectangle(1500, 100, 200, 30),
                new Rectangle(-40, -100, 1200, 110),
                new Rectangle(-400, -50, 180, 70),
            };


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var plataforma in plataformas)
                spriteBatch.Draw(texture, plataforma, Color.White);
        }
    }
}