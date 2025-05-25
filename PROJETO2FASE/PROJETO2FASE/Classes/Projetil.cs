
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PROJETO2FASE
{
    public class Projetil
    {
        public Vector2 posicao;
        public Vector2 velocidade;
        public Texture2D Texture;
        public Rectangle hitbox => new Rectangle((int)posicao.X, (int)posicao.Y, Texture.Width, Texture.Height); // tem de ser defenida aqui para atuailizar a posicao no update


        public Projetil(Texture2D texture, Vector2 posInicial, Vector2 direcao)
        {
            Texture = texture;
            posicao = posInicial;
            velocidade = direcao * 237f; // para ajustar a velociade é necessário alterar o valor do float
        }
  
     public void Update(GameTime gameTime)
        {
            posicao += velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds;  // Atualiza a posição do projetil para ele se movimentar
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, posicao, Color.White);
        }
    }
}