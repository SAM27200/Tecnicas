using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PROJETO2FASE.Classes
{
    public class Player
    {   
        // Característica do player e camera
        private Vector2 velocidade;
        public Vector2 posicao;
        public Vector2 size
        {
            get { return new Vector2(texture.Width, texture.Height); }
        }
        private Texture2D texture;
        private KeyboardState previousKeyState;
        private float tempoDesdeChao;
        private const float CoyoteTimeMax = 0.1f;
        private bool pousado;
        private float moveSpeed = 200f;
        private float gravidade = 900f;
        private float jumpSpeed = -450f;
        public int vida = 100;
        public Rectangle hitbox // a hitbox foi criada dentro do updade, isto foi colocado para conseguir fazer a colisão com inimigos
        {
            get
            {
                return new Rectangle((int)posicao.X, (int)posicao.Y, texture.Width, texture.Height);
            }
        }

        private bool atk = false;
        private float tempoatk = 0.4f;   
        private float tempoDecorrido =0f;  
        public Rectangle atkHitbox { get; private set; } = Rectangle.Empty;

        public List<Projetil> projeteis = new List<Projetil>();
        public Texture2D projetilTexture;

        public Player(Texture2D texture, Vector2 posInicial)
        {
            this.texture = texture;
            posicao = posInicial;
            velocidade = Vector2.Zero;
            pousado = false;
            previousKeyState = Keyboard.GetState(); 
        }

        public void Update(GameTime gameTime, List<Rectangle> plataformas)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            KeyboardState keyState = Keyboard.GetState();

            // Movimento horizontal
            if (keyState.IsKeyDown(Keys.A))
                velocidade.X = -moveSpeed;
            else if (keyState.IsKeyDown(Keys.D))
                velocidade.X = moveSpeed;
            else
                velocidade.X = 0;

            // Gravidade
            velocidade.Y += gravidade * dt; 
            posicao.X += velocidade.X * dt;
            Rectangle playerHitbox = new Rectangle((int)posicao.X, (int)posicao.Y, texture.Width, texture.Height); 

            // Colisão horizontal
            foreach (var platform in plataformas)
            {
                if (playerHitbox.Intersects(platform))
                {
                    if (velocidade.X > 0)
                        posicao.X = platform.Left - texture.Width; 
                    else if (velocidade.X < 0)
                        posicao.X = platform.Right;

                    velocidade.X = 0;
                    playerHitbox.X = (int)posicao.X;
                }
            }

            // Colisão vertical
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

            // Impedir o player de voar
            if (pousado)
                tempoDesdeChao = 0f;
            else
                tempoDesdeChao += dt;

            bool jumpPressed = keyState.IsKeyDown(Keys.Space) && previousKeyState.IsKeyUp(Keys.Space) || keyState.IsKeyDown(Keys.W) && previousKeyState.IsKeyUp(Keys.W);

            if (jumpPressed && tempoDesdeChao < CoyoteTimeMax)
            {
                velocidade.Y = jumpSpeed;
                pousado = false;
                tempoDesdeChao = CoyoteTimeMax; // impede novo salto até tocar no chão
            }

            // Ataque físico
            if(keyState.IsKeyDown(Keys.E) && previousKeyState.IsKeyUp(Keys.E) && !atk)
            {
                atk = true;
                tempoDecorrido = tempoatk;
                int largura = 20;            // tamanho da hitbox do ataque do player, ajustar se necessário
                int altura = texture.Height;
                int frente;                  // cria a hitbox a frente do player
                if (velocidade.X >= 0)
                {
                    frente = texture.Width; // ataque para a direita
                }
                else
                {
                    frente = -largura;      // ataque para a esquerda
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

            // Projeteis
            if (keyState.IsKeyDown(Keys.F) && previousKeyState.IsKeyUp(Keys.F))
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
                    ajusteX = -texture.Width + ajusteX;
                }
                Vector2 disparo = new Vector2(posicao.X + ajusteX,posicao.Y + ajusteY);
                Projetil novo = new Projetil(projetilTexture, disparo, direcao);
                projeteis.Add(novo);
            }

            previousKeyState = keyState;
        }
        // O player leva dano
        public void levarDano(int dano) 
        {
            vida -= dano;

        }

        // Dano de ataque físico
        public void ataqueFisico(InimigoATK inimigo)
        {
            if (atkHitbox.IsEmpty == false && atkHitbox.Intersects(inimigo.hitbox))
            {
                inimigo.vida -= 50;
            }
        }

        //Dano de ataque especial
        public void ataqueEspecial(InimigoSPATK inimigo)
        {
            List<Projetil> despawn = new List<Projetil>(); // Lista de projeteis a remover assim que colidem com a hitbox dos inimigos
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
        // Desenhar o player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (vida > 0)
            {
                spriteBatch.Draw(texture, posicao, Color.White);
            }
        }
    }
}
