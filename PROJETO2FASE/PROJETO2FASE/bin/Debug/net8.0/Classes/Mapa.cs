using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PROJETO2FASE.Classes
{
    internal class Mapa
    {
        public List<Rectangle> plataformas;
        private Texture2D texture;

        public Mapa(GraphicsDevice graphicsDevice)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);  // ele carrega um pixel um por um
            texture.SetData(new[] { Color.White });          

            plataformas = new List<Rectangle> {
                new Rectangle(0, 400, 800, 50),       // coordenas da plataforma 1º - eixo do x 2º eixo do y 3º comprimento 4º altura AVISO: AS COORDENAS FUNCIONAM AO CONTRARIO!!!
                new Rectangle(300, 300, 200, 30),
                new Rectangle(600, 200, 150, 30)
    /* new Rectangle(100, 250, 150, 25),    
    new Rectangle(500, 150, 120, 25),    
    new Rectangle(800, 350, 200, 30),   
    new Rectangle(1100, 300, 180, 30),  
    new Rectangle(1350, 250, 150, 30),   
    new Rectangle(1600, 400, 300, 50),*/ // estas aqui no comentário são só um teste
            };

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var plataforma in plataformas)
                spriteBatch.Draw(texture, plataforma, Color.Green);
        }
    }
}