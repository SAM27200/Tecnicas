# Tecnicas
Trabalho de Técnicas (2º Fase)

# (Nome do Jogo)
Breve resumo

# Pré-Requesitos
- MonoGame SDK
- Visual Studio 2022 (ou superior)
- .NET Framework/Core
  
# Membros do Grupo:
- Luís Gomes - 31473
- Samuel Fernandes - 31470
- David Barbosa - 31461

# Organização por pastas
- Content - Nesta pasta estão presentes todos os ficheiros dos sprites(.png) e fontes(.spritefont) utilizadas neste jogo, para além disso possui o ficheiro Content.mgcb que serve para definir e organizar os recursos que precisam ser processados e convertidos para o formato .xnb;
- Classes - Esta é a pasta que possui todas as classes usadas pela Main.

# FollowCamera
Na classe FollowCamera, a câmera fica ajustada para seguir o jogador, através de cálculos para centralizar-se na hitbox desse.
```csharp
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PROJETO2FASE.Classes
{
    public class FollowCamera
    {
        public Vector2 position;

        public FollowCamera(Vector2 position)
        {
            this.position = position;
        }

        public void Follow(Vector2 targetPosition, Vector2 targetSize, Vector2 screenSize)
        {
            // Calcula a posição para centralizar o jogador, considerando o centro da hitbox.
            position = new Vector2(
                 -targetPosition.X - (targetSize.X / 2) + (screenSize.X / 2),
                 -targetPosition.Y - (targetSize.Y / 2) + (screenSize.Y / 2)
            );

            // Adiciona um offset vertical para "baixar" a câmera.
            // Um valor positivo neste sistema (onde Y cresce para baixo) desloca a câmera para baixo.
            float verticalOffset = 30f; // ajuste esse valor conforme a sua preferência
            position.Y += verticalOffset;
        }

    }
}
```
# GameOver
Na classe GameOver, é introduzida a tela de Game Over, que será chamada para preencher a tela quando o jogador morrer.
```csharp
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace PROJETO2FASE.Classes
{    
    internal class GameOver
    {
        private SpriteFont Fonte;
        GraphicsDevice Graphicsdevice;

        public GameOver(SpriteFont fonte, GraphicsDevice graphicsdevice)
        {
            Fonte = fonte;  
            Graphicsdevice = graphicsdevice;
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                System.Environment.Exit(0); // sair do jogo no esc
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {   // Tela 100% proposital
            spriteBatch.DrawString(Fonte, "GAME OVER", new Vector2(50, 100), Color.Black);
            spriteBatch.DrawString(Fonte, "Não é um bug, é uma feature.", new Vector2(100, 300), Color.Black);

        }
    }
}
```
# InimigoATK
Na classe InimmigoATK, são introduzidas as características do inimigo que resiste a ataques de longo alcance como:

- Posição;
- Hitbox;
- Textura;
- Quantidade de vida;
- Velocidade;
- Zona de ataque;
- Cooldown de ataque;

No método Update conseguimos:

- Criar colisão com plataformas;
```csharp
   foreach(var plataforma in plataformas)
                {
                    if (hitbox.Intersects(plataforma)) 
                    {
                        if(posicao.Y + texture.Height <= plataforma.Top + 5) // cima
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

```
- Atacar e seguir o player;
```csharp
   if (distancia < zonaATK)
                {
                    direcao.Normalize();                       // para o inimigo vir atras do player
                    posicao += direcao * velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
             
                if (hitbox.Intersects(player.hitbox) && cooldownDecorr >= cooldown)
                {
                    player.levarDano(20);          
                    cooldownDecorr = 0;
                }
            }
```

# InimigoSPATK
A classe InimmigoSPATK é muito semelhante à classe InimigoSPATK, a única coisa diferente são os valores das características do inimigo, neste caso, aquele que resiste a ataques físicos.
Os tipos de características são:

- Posição;
- Hitbox;
- Textura;
- Quantidade de vida;
- Velocidade;
- Zona de ataque;
- Cooldown de ataque;

No método Update conseguimos:

- Criar colisão com plataformas;
```csharp
   foreach(var plataforma in plataformas)
                {
                    if (hitbox.Intersects(plataforma)) 
                    {
                        if(posicao.Y + texture.Height <= plataforma.Top + 5) // cima
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

```
- Atacar e seguir o player;
```csharp
   if (distancia < zonaATK)
                {
                    direcao.Normalize();                       // para o inimigo vir atras do player
                    posicao += direcao * velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
             
                if (hitbox.Intersects(player.hitbox) && cooldownDecorr >= cooldown)
                {
                    player.levarDano(30);          
                    cooldownDecorr = 0;
                }
            }
```

# Mapa
A classe Mapa possui uma lista de retângulos com tamanho e coordenadas predefinidas, que vão ser desenhadas na tela e vão servir como plataformas para o jogador se deslocar e proteger de inimigos.

```csharp
﻿using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PROJETO2FASE.Classes
{
    internal class Mapa
    {
        public List<Rectangle> plataformas;
        private Texture2D texture;

        public Mapa(GraphicsDevice graphicsDevice, ContentManager content) 
        {
            texture = content.Load<Texture2D>("plataforma");  

            // Lista de plataformas a carregar no mapa
            plataformas = new List<Rectangle> {  
                new Rectangle(-40, 400, 1200, 110),       // coordenas da plataforma 1º - eixo do x 2º eixo do y 3º largura 4º altura  
                new Rectangle(300, 300, 200, 30),
                new Rectangle(600, 200, 150, 30),
                new Rectangle(-2480, 400, 2300, 110),
                new Rectangle(-1080, 296, 400, 35),
                new Rectangle(-2480, 196, 1370, 35),
                new Rectangle(900, 200, 600, 30),
                new Rectangle(-2000, 96, 400, 30),
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
```

# ParallaxLayer
A classe ParallaxLayer, funciona para desenhar as layers do background para dar a impressão que estas se repetem, através de cálculos com o offset da tela, e que também se movem com o player, sendo que estas também tenham o mesmo tamanho da Tela, tanto em altura como em largura.

```csharp
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ParallaxLayer
{
    private Texture2D texture;
    private float speedFactor;
    private bool hasDefaultMovement;
    private GraphicsDevice graphicsDevice;
    private float basePositionX; // posição base (em pixels da textura) para efetuar a repetição

    public ParallaxLayer(Texture2D texture, float speedFactor, GraphicsDevice graphicsDevice, bool hasDefaultMovement = false)
    {
        this.texture = texture;
        this.speedFactor = speedFactor;
        this.graphicsDevice = graphicsDevice;
        this.hasDefaultMovement = hasDefaultMovement;
        basePositionX = 0f;
    }

    public void Update(Vector2 cameraPosition, GameTime gameTime)
    {
        if (hasDefaultMovement)
        {
            // Movimento automático do nevoiero (exemplo: 50 pixels por segundo)
            basePositionX += 50f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            // Movimento baseado na posição da câmera e no fator de velocidade
            basePositionX = cameraPosition.X * speedFactor;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Obtém as dimensões da tela
        Viewport viewport = graphicsDevice.Viewport;

        // Calcula a escala necessária para fazer a textura ter a altura da tela
        float scale = (float)viewport.Height / texture.Height;

        // Largura da textura já escalada
        float scaledTileWidth = texture.Width * scale;

        // Calcula o offset horizontal para começar a desenhar de forma que a repetição seja contínua
        // Multiplica a posição base pela escala e pega o resto da divisão pela largura do tile
        float offsetX = -(basePositionX * scale % scaledTileWidth);
        if (offsetX > 0)
            offsetX -= scaledTileWidth; // garante que o offset seja negativo para alinhar à esquerda

        // Loop para desenhar a textura tantas vezes quanto necessário para cobrir toda a tela
        for (float x = offsetX; x < viewport.Width; x += scaledTileWidth)
        {
            // Cria um retângulo destino que estica a textura para a resolução atual da tela
            Rectangle destRect = new Rectangle((int)x, 0, (int)scaledTileWidth, viewport.Height);
            spriteBatch.Draw(texture, destRect, Color.White);
        }
    }
}
```
# Projetil
A classe Projetil define a hitbox, a partir da textura dela, a direção da movimentação e a sua velocidade, para que o projetil funcione da maneira suposta.

```csharp
﻿
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
```

# Player
Na classe Player, são criadas todas as características do jogador, como o tamanho,a posição, a velocidade, a vida, a textura, a velocidade do movimento, o jump speed, a gravidade e a hitbox, o ataque e as características deste.
O ataque é dividido em dois, o ataque físico e o ataque especial, que é um projétil, também é atribuída a hitbox do ataque físico e a sua colisão, e por fim é atribuído o dano a cada ataque para matar os inimigos específicos para o qual eles foram feitos.
Ainda são feitas algumas correções na classe para que o jogador não abuse das suas físicas, tal como voar e cooldown para o ataque físico.

<details>
    <summary>Clique aqui para ver a função completa</summary>
  
  ```csharp 
      ﻿using Microsoft.Xna.Framework;
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
```
</details>

# Game1/Main

No Game1, também referido como main, é carregado todo o conteúdo da pasta content, através do método LoadContent, e através do método Update atualiza frame a frame o jogo para aplicar as Classes, que vai buscar na pasta “Classes”, e ainda desenha através do método Draw, as texturas, como o Background, a textura do Player, dos inimigos, das plataformas e a do projétil, e ainda desenha a tela de Game Over quando o player morre.

<details>
    <summary>Clique aqui para ver a função completa</summary>
  
  ```csharp
  ﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PROJETO2FASE.Classes;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;


namespace PROJETO2FASE
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Mapa mapa;

        private Player player;
        private Texture2D playerTexture;
        private Texture2D hitboxatk; // opcional

        public List<Projetil> projeteis = new List<Projetil>();
        public Texture2D projetilTexture;

        private List<InimigoATK> inimigosATK;
        private List<InimigoSPATK> inimigosSPATK;
        private Texture2D texturaInimigoatk;
        private Texture2D texturaInimigospatk;

        private GameOver gameover;
        private bool morto = false;
        Song song;
        private Song gameOver;
        private bool gameOverTriggered = false;

        private FollowCamera camera;
        private List<ParallaxLayer> backgroundLayers; 
    

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            camera = new(Vector2.Zero);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Layers de paralaxe
            backgroundLayers = new List<ParallaxLayer>
            {
               new ParallaxLayer(Content.Load<Texture2D>("Layer0"), 0.1f, GraphicsDevice),
               new ParallaxLayer(Content.Load<Texture2D>("Layer1"), 0.2f, GraphicsDevice),
               new ParallaxLayer(Content.Load<Texture2D>("Layer2"), 0.4f, GraphicsDevice),
               new ParallaxLayer(Content.Load<Texture2D>("Layer3"), 0.6f, GraphicsDevice),
               new ParallaxLayer(Content.Load<Texture2D>("Layer4"), 0.8f, GraphicsDevice),
               new ParallaxLayer(Content.Load<Texture2D>("Layer5"), 1.0f, GraphicsDevice, true)
            };

            // Mapa
            mapa = new Mapa(GraphicsDevice, Content);

            // Projetil 
            projetilTexture = Content.Load<Texture2D>("boladefogo"); 

            // Inimigo fisico
            texturaInimigoatk = Content.Load<Texture2D>("InimigoATK");
            inimigosATK = new List<InimigoATK>
            {
               new InimigoATK(new Vector2(500, 350), texturaInimigoatk),
               new InimigoATK(new Vector2(-1500, 350), texturaInimigoatk),
               new InimigoATK(new Vector2(-1600, 350), texturaInimigoatk),
               new InimigoATK(new Vector2(-1300, 350), texturaInimigoatk),
               new InimigoATK(new Vector2(-1300, 350), texturaInimigoatk),
               new InimigoATK(new Vector2(-1300, 350), texturaInimigoatk),
               new InimigoATK(new Vector2(-1050, 260), texturaInimigoatk), 
               new InimigoATK(new Vector2(-2400, 160), texturaInimigoatk), 
               new InimigoATK(new Vector2(950, 150), texturaInimigoatk),   
               new InimigoATK(new Vector2(1650, 250), texturaInimigoatk),  
               new InimigoATK(new Vector2(1270, -40), texturaInimigoatk), 
               new InimigoATK(new Vector2(-1600, -40), texturaInimigoatk),
               new InimigoATK(new Vector2(500, -170), texturaInimigoatk),
            };

            // Inimigo especial
            texturaInimigospatk = Content.Load<Texture2D>("InimigoSPATK");
            inimigosSPATK = new List<InimigoSPATK>
            {
               new InimigoSPATK(new Vector2(700, 300), texturaInimigospatk),
               new InimigoSPATK(new Vector2(-1000, 230), texturaInimigospatk),   
               new InimigoSPATK(new Vector2(-2450, 150), texturaInimigospatk),   
               new InimigoSPATK(new Vector2(1000, 145), texturaInimigospatk),     
               new InimigoSPATK(new Vector2(1520, 50), texturaInimigospatk),     
               new InimigoSPATK(new Vector2(-20, -150), texturaInimigospatk),    
               new InimigoSPATK(new Vector2(-390, -100), texturaInimigospatk),
               new InimigoSPATK(new Vector2(300, -150), texturaInimigospatk),
            };

            //Player
            playerTexture = Content.Load<Texture2D>("Player");
            player = new Player(playerTexture, new Vector2(100, 100));

            //Projetil
            player.projetilTexture = projetilTexture;                    
            hitboxatk = new Texture2D(GraphicsDevice, 1, 1);
            hitboxatk.SetData(new[] { Color.White });
            
            // Tela de GAME OVER e fonte
            SpriteFont fonte = Content.Load<SpriteFont>("File");
            gameover = new GameOver(fonte, GraphicsDevice);
            gameOver = Content.Load<Song>("GameOver");

            // Musica de fundo
            song = Content.Load<Song>("Tralalero");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);


        }

        protected override void Update(GameTime gameTime)
        {   
            //Paralaxe
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach (var layer in backgroundLayers)
            {
                layer.Update(camera.position, gameTime);
            }

            // Update do player
            player.Update(gameTime, mapa.plataformas); 
            
            
            // Update de inimigos físicos
            foreach (var inimigo in inimigosATK)
            {
                inimigo.Update(gameTime, player, mapa.plataformas);
                player.ataqueFisico(inimigo);
            }
            // Update de inimigos especiais
            foreach (var inimigo in inimigosSPATK)
            {
                inimigo.Update(gameTime, player, mapa.plataformas);
                player.ataqueEspecial(inimigo);
            }

            // Update de camera
            camera.Follow(player.posicao, player.size, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)); 
            base.Update(gameTime);

            // Update de projéteis
            foreach (var projetil in player.projeteis)
            {
                projetil.Update(gameTime);
            }

            // Void
            if(player.posicao.Y > 1000) 
            {
                player.levarDano(400);
            } 

            // Se o player morrer :(
            if (player.vida <= 0)
            {
                morto = true;
            }

            if (player.vida <= 0 && gameOverTriggered == false)
            {
                gameOverTriggered = true;
                MediaPlayer.Stop();
                MediaPlayer.Play(gameOver);
            }

            if (morto == true)
            {
                gameover.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Paralaxe
            _spriteBatch.Begin();
            foreach (var layer in backgroundLayers)
            {
                layer.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            //Camera
            float verticalOffset = 30f; // ajustar conforme preferência
            Matrix cameraMatrix = Matrix.CreateTranslation(new Vector3(camera.position.X, camera.position.Y + verticalOffset, 0f));

            _spriteBatch.Begin(transformMatrix: cameraMatrix);
            //Player
            player.Draw(_spriteBatch);

            //Mapa
            mapa.Draw(_spriteBatch);

            //Temporario - Hitobx de Ataque
            if (!player.atkHitbox.IsEmpty)
            {
                _spriteBatch.Draw(hitboxatk, player.atkHitbox, Color.Green * 0.5f);
            }

            //Projeteis
            foreach (var projetil in player.projeteis)
            {
                projetil.Draw(_spriteBatch);    
            }

            // Inimigo Ataque Fisico
            foreach (var inimigo in inimigosATK)
            {
                inimigo.Draw(_spriteBatch);
                _spriteBatch.Draw(hitboxatk, inimigo.hitbox, Color.Red * 0.5f); 
            }
            // Inimigo Ataque Especial
            foreach (var inimigo in inimigosSPATK)
            {
                inimigo.Draw(_spriteBatch);
                _spriteBatch.Draw(hitboxatk, inimigo.hitbox, Color.Yellow * 0.5f);
            }
            _spriteBatch.End();

            // Game over
            if (morto == true)
            {
                
                GraphicsDevice.Clear(Color.White);
                _spriteBatch.Begin();
                gameover.Draw(_spriteBatch);
                _spriteBatch.End();
                return;     
            }

   

            base.Draw(gameTime);
        }
    }
}
  ```
</details>
