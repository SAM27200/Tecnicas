using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJETO2FASE.Classes
{
    public class InimigoATK
    {
        public Vector2 posicao;
        public int dano = 45;
        public int vida = 50;
        private float velocidade = 100f;
        private float zonaATK = 100f;
        private Texture2D texture;
        public Rectangle hitbox;
        private float cooldown = 1.5f;
        private float cooldownDecorr = 0f;

        public InimigoATK(Vector2 posInicial, Texture2D textura)
        {
            posicao = posInicial;
            texture = textura;
        }

        public void Update(GameTime gameTime, Player player, List<Rectangle> plataformas)
        {
            if ( vida > 0)
            {
                hitbox = new Rectangle((int)posicao.X, (int)posicao.Y, texture.Width, texture.Height); //faltava atualizar a posicao da hitbox e o tempo do cooldown
                cooldownDecorr += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 direcao = player.posicao - posicao;
                float distancia = direcao.Length();

                foreach (var plataforma in plataformas)
                {
                    if (hitbox.Intersects(plataforma))
                    {
                        posicao.Y = plataforma.Top - texture.Height; 
                        break;
                    }
                }

                if (distancia < zonaATK)
                {
                    direcao.Normalize();                       // para o inimigo vir atras do player
                    posicao += direcao * velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
              if(hitbox.Intersects(player.hitbox) && cooldownDecorr >= cooldown)
                {
                    player.levarDano(45);          // o player morrer em 3 ataques
                    cooldownDecorr = 0;
                }
              if (player.atkHitbox.Intersects(this.hitbox))
               {
                    vida -= 50; 
               }
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (vida > 0)
            {
                spriteBatch.Draw(texture, posicao, Color.White);
            }
        }






    }


}
