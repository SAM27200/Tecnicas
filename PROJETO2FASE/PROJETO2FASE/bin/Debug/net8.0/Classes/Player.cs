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
        public Vector2 size
        {
            get { return new Vector2(texture.Width, texture.Height); }
        }
        private Vector2 velocidade;
        private Texture2D texture;
        private bool pousado;
        private KeyboardState previousKeyState;
        private float moveSpeed = 200f;
        private float gravidade = 900f;
        private float jumpSpeed = -450f;
        public int vida = 100;
        public Rectangle hitbox // criei a hitbox no update, e coloquei isto aqui para poder usar com os inimigos
        {
            get
            {
                return new Rectangle((int)posicao.X, (int)posicao.Y, texture.Width, texture.Height);
            }
        }

        // ataque fisico
        private bool atk = false;
        private float tempoatk = 0.4f;   // tempo de animação  (depois ajustar)
        private float tempoDecorrido =0f;  // tempo de ataque decorrido 
        public Rectangle atkHitbox { get; private set; } = Rectangle.Empty;


        // projetil
        public List<Projetil> projeteis = new List<Projetil>();
        public Texture2D projetilTexture;

        public Player(Texture2D texture, Vector2 posInicial)
        {
            this.texture = texture;
            posicao = posInicial;
            velocidade = Vector2.Zero;
            pousado = false;
            previousKeyState = Keyboard.GetState(); // Inicializar
        }

        public void Update(GameTime gameTime, List<Rectangle> plataformas)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // delta time, é basicamento o tempo que passa desde o ultimo frame/update, dá uma melhor otimização para todos os dispositivos
            KeyboardState keyState = Keyboard.GetState();

            // Movimento horizontal
            if (keyState.IsKeyDown(Keys.A))
                velocidade.X = -moveSpeed;
            else if (keyState.IsKeyDown(Keys.D))
                velocidade.X = moveSpeed;
            else
                velocidade.X = 0;

            // Gravidade
            velocidade.Y += gravidade * dt; // a velocidade vertical aumenta conforme a gravidade 
            // Movimento e colisão horizontal
            posicao.X += velocidade.X * dt;
            Rectangle playerHitbox = new Rectangle((int)posicao.X, (int)posicao.Y, texture.Width, texture.Height); // definicção da hitbox do player

            foreach (var platform in plataformas)
            {
                if (playerHitbox.Intersects(platform))
                {
                    if (velocidade.X > 0)
                        posicao.X = platform.Left - texture.Width; // impede a colisao do jogador ,ex: a largura do objeto é 100 e a do player 20, vai ter uma colisao no x = 80
                    else if (velocidade.X < 0)
                        posicao.X = platform.Right;

                    velocidade.X = 0;
                    playerHitbox.X = (int)posicao.X;
                }
            }

            // Movimento e colisão vertical
            posicao.Y += velocidade.Y * dt;
            playerHitbox.Y = (int)posicao.Y;
            pousado = false;

            foreach (var plataforma in plataformas)
            {
                if (playerHitbox.Intersects(plataforma))
                {
                    if (velocidade.Y > 0)
                    {
                        posicao.Y = plataforma.Top - texture.Height;
                        pousado = true;
                    }
                    else if (velocidade.Y < 0)
                    {
                        posicao.Y = plataforma.Bottom;
                    }

                    velocidade.Y = 0;
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
                velocidade.Y = jumpSpeed;
                pousado = false;
                tempoDesdeChao = CoyoteTimeMax; // impede novo salto até tocar no chão
            }

            // logica de ataque fisico
            if(keyState.IsKeyDown(Keys.E) && previousKeyState.IsKeyUp(Keys.E) && !atk)
            {
                atk = true;
                tempoDecorrido = tempoatk;
                int largura = 20;             // largura e altura da hitbox
                int altura = texture.Height;
                int frente;             // para criar a hitbox do ataque a frente do player
                if (velocidade.X >= 0)
                {
                    frente = texture.Width; // ataque para a direita
                }
                else
                {
                    frente = -largura;   // ataque para a esquerda , o - é necessário para nao ficar distante do jogador
                }
                atkHitbox = new Rectangle ((int)posicao.X + frente, (int)posicao.Y, largura, altura);
            }
            if (atk == true)
            {
                tempoDecorrido -= dt;             
                if (tempoDecorrido <= 0)
                {
                    atk = false;
                    atkHitbox = Rectangle.Empty; // torna a hitbox vazia
                }

            }

            //disparo 
            if (keyState.IsKeyDown(Keys.Q) && previousKeyState.IsKeyUp(Keys.Q))
            {
                Vector2 direcao;
                if (velocidade.X >= 0)
                {
                    direcao = new Vector2(1, 0); //para a direita
                }
                else
                {
                    direcao = new Vector2(-1, 0); //para a esquerda
                }
             // Aqui podem ajustar os valores para os projeteis sairem conforme a sprite
                float ajusteX = 20f; // para a direita - valores positivos , para a esquerda - valores negativos
                float ajusteY = 10f; // para baixo - valores positivos , para cima - valores negativos
                if (direcao.X < 0)
                {
                    ajusteX = -texture.Width + ajusteX; // coloca o projetil a esquerda caso ele esteja a ir para a esquerda obvio
                }
                Vector2 disparo = new Vector2(posicao.X + ajusteX,posicao.Y + ajusteY);
                Projetil novo = new Projetil(projetilTexture, disparo, direcao);
                projeteis.Add(novo);
            }

            previousKeyState = keyState;
        }
        public void levarDano(int dano) // o player leva dano
        {
            vida -= dano;

        }
        public void ataqueFisico(InimigoATK inimigo)
        {
            if (!atkHitbox.IsEmpty && atkHitbox.Intersects(inimigo.hitbox))
            {
                inimigo.vida -= 50;
            }
        }

        public void ataqueEspecial(InimigoSPATK inimigo)
        {
            List<Projetil> despawn = new List<Projetil>(); // remove os projeteis assim que eles acertam na hitbox do inimigo
            foreach (var projetil in projeteis)
            {
                if (projetil.hitbox.Intersects(inimigo.hitbox))
                {
                    inimigo.vida -= 20;
                    despawn.Add(projetil);
                }
            }
            foreach (var projetil in despawn)
            {
                projeteis.Remove(projetil);
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
