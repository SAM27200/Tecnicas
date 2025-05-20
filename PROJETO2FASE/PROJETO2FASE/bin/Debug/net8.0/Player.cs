using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PROJETO2FASE
{
    public class Player
    {
        private float tempoDesdeChao;
        private const float CoyoteTimeMax = 0.1f; // 100ms de tolerância

        public Vector2 Position;
        private Vector2 Velocity;
        private Texture2D texture;
        private bool pousado;
        private KeyboardState previousKeyState;

        private const float MoveSpeed = 200f;
        private const float Gravity = 900f;
        private const float JumpVelocity = -450f;

        public Player(Texture2D texture, Vector2 startPos)
        {
            this.texture = texture;
            Position = startPos;
            Velocity = Vector2.Zero;
            pousado = false;
            previousKeyState = Keyboard.GetState(); // Inicializar
        }

        public void Update(GameTime gameTime, List<Rectangle> platforms)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyState = Keyboard.GetState();

            // Movimento horizontal
            if (keyState.IsKeyDown(Keys.A))
                Velocity.X = -MoveSpeed;
            else if (keyState.IsKeyDown(Keys.D))
                Velocity.X = MoveSpeed;
            else
                Velocity.X = 0;

            // Gravidade
            Velocity.Y += Gravity * dt;

            // Movimento e colisão horizontal
            Position.X += Velocity.X * dt;
            Rectangle playerRect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);

            foreach (var platform in platforms)
            {
                if (playerRect.Intersects(platform))
                {
                    if (Velocity.X > 0)
                        Position.X = platform.Left - texture.Width;
                    else if (Velocity.X < 0)
                        Position.X = platform.Right;

                    Velocity.X = 0;
                    playerRect.X = (int)Position.X;
                }
            }

            // Movimento e colisão vertical
            Position.Y += Velocity.Y * dt;
            playerRect.Y = (int)Position.Y;
            pousado = false;

            foreach (var platform in platforms)
            {
                if (playerRect.Intersects(platform))
                {
                    if (Velocity.Y > 0)
                    {
                        Position.Y = platform.Top - texture.Height;
                        pousado = true;
                    }
                    else if (Velocity.Y < 0)
                    {
                        Position.Y = platform.Bottom;
                    }

                    Velocity.Y = 0;
                    playerRect.Y = (int)Position.Y;
                }
            }

            // Atualizar tempo desde o chão
            if (pousado)
                tempoDesdeChao = 0f;
            else
                tempoDesdeChao += dt;

            // Verificar salto com coyote time
            bool jumpPressed =
                (keyState.IsKeyDown(Keys.Space) && previousKeyState.IsKeyUp(Keys.Space)) ||
                (keyState.IsKeyDown(Keys.W) && previousKeyState.IsKeyUp(Keys.W));

            if (jumpPressed && tempoDesdeChao < CoyoteTimeMax)
            {
                Velocity.Y = JumpVelocity;
                pousado = false;
                tempoDesdeChao = CoyoteTimeMax; // impede novo salto até tocar no chão
            }

            previousKeyState = keyState;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
