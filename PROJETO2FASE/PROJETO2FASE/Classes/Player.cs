using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PROJETO2FASE.Classes
{
    public class Player
    {
        private float tempoDesdeChao;
        private const float CoyoteTimeMax = 0.1f; // 100ms de tolerância

        public Vector2 posicao;
        private Vector2 velocity;
        private Texture2D texture;
        private bool pousado;
        private KeyboardState previousKeyState;

        private const float moveSpeed = 200f;
        private const float gravidade = 900f;
        private const float jumpSpeed = -450f;

        public Player(Texture2D texture, Vector2 startPos)
        {
            this.texture = texture;
            posicao = startPos;
            velocity = Vector2.Zero;
            pousado = false;
            previousKeyState = Keyboard.GetState(); // Inicializar
        }

        public void Update(GameTime gameTime, List<Rectangle> platforms)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // delta time, 
            KeyboardState keyState = Keyboard.GetState();

            // Movimento horizontal
            if (keyState.IsKeyDown(Keys.A))
                velocity.X = -moveSpeed;
            else if (keyState.IsKeyDown(Keys.D))
                velocity.X = moveSpeed;
            else
                velocity.X = 0;

            // Gravidade
            velocity.Y += gravidade * dt;

            // Movimento e colisão horizontal
            posicao.X += velocity.X * dt;
            Rectangle playerHitbox = new Rectangle((int)posicao.X, (int)posicao.Y, texture.Width, texture.Height); // definicção da hitbox do player

            foreach (var platform in platforms)
            {
                if (playerHitbox.Intersects(platform))
                {
                    if (velocity.X > 0)
                        posicao.X = platform.Left - texture.Width; // impede a colisao do jogador ,ex: a largura do objeto é 100 e a do player 20, vai ter uma colisao no x = 80
                    else if (velocity.X < 0)
                        posicao.X = platform.Right;

                    velocity.X = 0;
                    playerHitbox.X = (int)posicao.X;
                }
            }

            // Movimento e colisão vertical
            posicao.Y += velocity.Y * dt;
            playerHitbox.Y = (int)posicao.Y;
            pousado = false;

            foreach (var platform in platforms)
            {
                if (playerHitbox.Intersects(platform))
                {
                    if (velocity.Y > 0)
                    {
                        posicao.Y = platform.Top - texture.Height;
                        pousado = true;
                    }
                    else if (velocity.Y < 0)
                    {
                        posicao.Y = platform.Bottom;
                    }

                    velocity.Y = 0;
                    playerHitbox.Y = (int)posicao.Y;
                }
            }

            // Atualizar tempo desde o chão
            if (pousado)
                tempoDesdeChao = 0f;
            else
                tempoDesdeChao += dt;

            // Verificar salto com coyote time
            bool jumpPressed = keyState.IsKeyDown(Keys.Space) && previousKeyState.IsKeyUp(Keys.Space) || keyState.IsKeyDown(Keys.W) && previousKeyState.IsKeyUp(Keys.W);

            if (jumpPressed && tempoDesdeChao < CoyoteTimeMax)
            {
                velocity.Y = jumpSpeed;
                pousado = false;
                tempoDesdeChao = CoyoteTimeMax; // impede novo salto até tocar no chão
            }

            previousKeyState = keyState;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, posicao, Color.White);
        }
    }
}
