using System;
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