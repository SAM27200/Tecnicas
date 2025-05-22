using Microsoft.Xna.Framework;
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
