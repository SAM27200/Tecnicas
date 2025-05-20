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
                new Rectangle(0, 400, 800, 50),       // coordenas da plataforma 1º - eixo do x 2º eixo do y 3º comprimento 4º altura AVISO: AS COORDENAS FUNCIONAM AO CONTRARIO!!!
                new Rectangle(300, 300, 200, 30),
                new Rectangle(600, 200, 150, 30),
    /*ew Rectangle(100, 250, 150, 25),    
    new Rectangle(500, 150, 120, 25),    
    new Rectangle(800, 350, 200, 30),   
    new Rectangle(1100, 300, 180, 30),  
    new Rectangle(1350, 250, 150, 30),   
    new Rectangle(1600, 400, 300, 50)*/  //estes aqui sao apenas uns testes
            };

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var plataforma in plataformas)
                spriteBatch.Draw(texture, plataforma, Color.White);
        }
    }
}