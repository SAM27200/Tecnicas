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
        private InimigoATK inimigo;
        private Texture2D texturaInimigo;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            texturaInimigo = Content.Load<Texture2D>("teste"); 
            inimigo = new InimigoATK(new Vector2(500, 300), texturaInimigo);

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
            inimigo.Update(gameTime, player, mapa.plataformas);
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
            inimigo.Draw(_spriteBatch);
            _spriteBatch.Draw(hitboxatk, inimigo.hitbox, Color.Red * 0.5f); // temporario

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
