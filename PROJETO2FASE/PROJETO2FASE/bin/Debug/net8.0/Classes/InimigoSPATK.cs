using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace PROJETO2FASE.Classes
{
    public class InimigoSPATK
    {
        public Vector2 posicao;
        public Rectangle hitbox;
        private Texture2D texture;
        public int vida = 50;
        private float velocidade = 110f;
        private float zonaATK = 300f;
        private float cooldown = 2f;
        private float cooldownDecorr = 0f;
        
        public InimigoSPATK(Vector2 posInicial, Texture2D textura)
        {
            posicao = posInicial;
            texture = textura;
        }

        public void Update(GameTime gameTime, Player player, List<Rectangle> plataformas)
        {
            if (vida > 0)
            {
                hitbox = new Rectangle((int)posicao.X, (int)posicao.Y, texture.Width, texture.Height); 
                cooldownDecorr += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 direcao = player.posicao - posicao;
                float distancia = direcao.Length();

                // Colisão com plataformas
                foreach (var plataforma in plataformas)
                {
                    if (hitbox.Intersects(plataforma))
                    {
                        if (posicao.Y + texture.Height <= plataforma.Top + 5) // cima
                        {
                            posicao.Y = plataforma.Top - texture.Height;
                        }
                        else if (posicao.Y >= plataforma.Bottom - 5)     // baixo
                        {
                            posicao.Y = plataforma.Bottom;
                        }
                        else if (posicao.X + texture.Width <= plataforma.Left + 5)  // esquerda
                        {
                            posicao.X = plataforma.Left - texture.Width;
                        }
                        else if (posicao.X >= plataforma.Right - 5)    // direita
                        {
                            posicao.X = plataforma.Right;
                        }

                    }
                }

                // Atacar o player
                if (distancia < zonaATK)
                {
                    direcao.Normalize();                     
                    posicao += direcao * velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds;   // para o inimigo vir atras do player

                }
                if (hitbox.Intersects(player.hitbox) && cooldownDecorr >= cooldown)
                {
                    player.levarDano(30);         
                    cooldownDecorr = 0;
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