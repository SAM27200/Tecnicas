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
<<<<<<< Updated upstream
        private List<Rectangle> platforms;

=======
        private Texture2D hitboxatk; //temporario
        public List<Projetil> projeteis = new List<Projetil>();
        public Texture2D projetilTexture;
>>>>>>> Stashed changes
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            platforms = new List<Rectangle>
            {
                new Rectangle(0, 400, 800, 50)
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
<<<<<<< Updated upstream
            mapa = new Mapa(GraphicsDevice); 
=======
            mapa = new Mapa(GraphicsDevice, Content); // e atualizei aqui para o caminho do content

            projetilTexture = Content.Load<Texture2D>("boladefogo"); // Esta é a textura teste dos projeteis samuel



>>>>>>> Stashed changes
            // Criar textura vermelha para o player
            playerTexture = new Texture2D(GraphicsDevice, 50, 50);
            Color[] playerData = new Color[50 * 50];
            for (int i = 0; i < playerData.Length; i++) playerData[i] = Color.Red;
            playerTexture.SetData(playerData);

            // Criar textura azul para plataformas
            platformTexture = new Texture2D(GraphicsDevice, 1, 1);
            platformTexture.SetData(new[] { Color.Blue });

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
            base.Update(gameTime);

            foreach (var projetil in player.projeteis)
            {
                projetil.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var platform in platforms)
                _spriteBatch.Draw(platformTexture, platform, Color.White);

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

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
