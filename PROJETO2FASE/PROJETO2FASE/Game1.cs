using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace PROJETO2FASE
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;
        private Texture2D playerTexture;
        private Texture2D platformTexture;
        private List<Rectangle> platforms;

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
                new Rectangle(0, 400, 800, 50),
                new Rectangle(300, 300, 200, 30),
                new Rectangle(600, 200, 150, 30)
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime, platforms);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var platform in platforms)
                _spriteBatch.Draw(platformTexture, platform, Color.White);

            player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
