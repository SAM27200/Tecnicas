using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PROJETO2FASE.Classes;
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
        private InimigoATK inimigoatk;
        private Texture2D texturaInimigoatk;
        private InimigoSPATK inimigospatk;
        private Texture2D texturaInimigospatk;
        private FollowCamera camera;
        

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
            mapa = new Mapa(GraphicsDevice, Content); // e atualizei aqui para o caminho do content

            projetilTexture = Content.Load<Texture2D>("boladefogo"); // Esta é a textura teste dos projeteis samuel

            texturaInimigoatk = Content.Load<Texture2D>("enimigo"); 
            inimigoatk = new InimigoATK(new Vector2(500, 400), texturaInimigoatk);

            texturaInimigospatk = Content.Load<Texture2D>("teste1");
            inimigospatk = new InimigoSPATK(new Vector2(700, 300), texturaInimigospatk);  

            // Criar textura vermelha para o player
            playerTexture = new Texture2D(GraphicsDevice, 50, 50);
            Color[] playerData = new Color[50 * 50];
            for (int i = 0; i < playerData.Length; i++) playerData[i] = Color.Red;
            playerTexture.SetData(playerData);

            // Inicializar jogador
            player = new Player(playerTexture, new Vector2(100, 100));
            player.projetilTexture = projetilTexture;                    // aqui associa a a textura do projetil aos do player
            hitboxatk = new Texture2D(GraphicsDevice, 1, 1);
            hitboxatk.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime, mapa.plataformas); // alterei o player.Update(gameTime, plataformas) para isto porque assim ele atualiza a colisão do player com as plataformas geradas no mapa
            
            inimigoatk.Update(gameTime, player, mapa.plataformas);
            inimigospatk.Update(gameTime, player, mapa.plataformas);
            player.ataqueFisico(inimigoatk);
            player.ataqueEspecial(inimigospatk);

            camera.Follow(player.posicao, player.size, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)); //Adiciona este no teu código do game1
            base.Update(gameTime);


            foreach (var projetil in player.projeteis)
            {
                projetil.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float verticalOffset = 30f; // ajusta este valor conforme a tua preferência
            Matrix cameraMatrix = Matrix.CreateTranslation(new Vector3(camera.position.X, camera.position.Y + verticalOffset, 0f));

            _spriteBatch.Begin(transformMatrix: cameraMatrix);

            player.Draw(_spriteBatch);
            mapa.Draw(_spriteBatch);

            if (!player.atkHitbox.IsEmpty)
            {
                _spriteBatch.Draw(hitboxatk, player.atkHitbox, Color.Green * 0.5f);
            }
            foreach (var projetil in player.projeteis)
            {
                projetil.Draw(_spriteBatch);    //desenha os projeteis
            }

            inimigoatk.Draw(_spriteBatch);
            _spriteBatch.Draw(hitboxatk, inimigoatk.hitbox, Color.Red * 0.5f); // temporario

            inimigospatk.Draw(_spriteBatch);
            _spriteBatch.Draw(hitboxatk, inimigospatk.hitbox, Color.Yellow * 0.5f); // tambem temporario

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
