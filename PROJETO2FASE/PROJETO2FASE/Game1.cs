using Microsoft.Xna.Framework;
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
        private Texture2D platformTexture;
        private Texture2D hitboxatk; //temporario
        public List<Projetil> projeteis = new List<Projetil>();
        public Texture2D projetilTexture;
        private List<InimigoATK> inimigosATK;
        private List<InimigoSPATK> inimigosSPATK;
        private Texture2D texturaInimigoatk;
        private InimigoSPATK inimigospatk;
        private Texture2D texturaInimigospatk;
        private FollowCamera camera;
        private GameOver gameover;
        private bool morto = false;
        private List<ParallaxLayer> backgroundLayers; // adicionar
        Song song;
        private Song gameOver;
        private bool gameOverTriggered = false;
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
            mapa = new Mapa(GraphicsDevice, Content); // e atualizei aqui para o caminho do content

            // Projetil 
            projetilTexture = Content.Load<Texture2D>("boladefogo"); // Esta é a textura teste dos projeteis samuel

            // Inimigo fisico
            texturaInimigoatk = Content.Load<Texture2D>("enimigo");
            inimigosATK = new List<InimigoATK>
            {
              new InimigoATK(new Vector2(500, 350), texturaInimigoatk),
              new InimigoATK(new Vector2(-1500, 350), texturaInimigoatk),
              new InimigoATK(new Vector2(-1600, 350), texturaInimigoatk),
              new InimigoATK(new Vector2(-1300, 350), texturaInimigoatk),
              new InimigoATK(new Vector2(-1300, 350), texturaInimigoatk),
              new InimigoATK(new Vector2(-1300, 350), texturaInimigoatk),


            };

            // Inimigo especial
            texturaInimigospatk = Content.Load<Texture2D>("teste1");
            inimigosSPATK = new List<InimigoSPATK>
            {
               new InimigoSPATK(new Vector2(700, 300), texturaInimigospatk),
            };

            // Criar textura vermelha para o player e coisas do player
            playerTexture = new Texture2D(GraphicsDevice, 50, 50);
            Color[] playerData = new Color[50 * 50];
            for (int i = 0; i < playerData.Length; i++) playerData[i] = Color.Red;
            playerTexture.SetData(playerData);

            player = new Player(playerTexture, new Vector2(100, 100));
            player.projetilTexture = projetilTexture;                    // aqui associa a a textura do projetil aos do player
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
            player.Update(gameTime, mapa.plataformas); // alterei o player.Update(gameTime, plataformas) para isto porque assim ele atualiza a colisão do player com as plataformas geradas no mapa
            
            
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
            camera.Follow(player.posicao, player.size, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)); //Adiciona este no teu código do game1
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
            float verticalOffset = 30f; // ajusta este valor conforme a tua preferência
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
                projetil.Draw(_spriteBatch);    //desenha os projeteis
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
                return;     // com este return n desenha mais
            }

   

            base.Draw(gameTime);
        }
    }
}
