using System;
using System.Text;
using LorcanaLorebook.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


namespace LorcanaLorebook.UI
{
    public class CardUI : MonoBehaviour
    {
        public Image image;
        private Card cardToDisplay;

        public void InitializeCardUI(Card card)
        {
            StringBuilder stringBuilder = new StringBuilder();

            cardToDisplay = card;
            try
            {
                Resize(card.CardImage, (int)image.preferredWidth, (int)image.preferredHeight);
                //Resize the sprite to fit our image renderer.
                image.sprite = Sprite.Create(card.CardImage, new Rect(0, 0, (int)image.preferredWidth, (int)image.preferredHeight), new Vector2(0.5f, 0.5f), 100.0f);
            }
            catch (Exception _)
            {
                stringBuilder.AppendLine("Failed card: " + card.FullName);
            }

            if (stringBuilder.Length > 0)
            {
                Debug.LogError("Failed Cards: " + stringBuilder.ToString());
            }
        }

        private Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            RenderTexture rt = new RenderTexture(targetX, targetY, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);
            Texture2D result = new Texture2D(targetX, targetY);
            result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
            result.Apply();
            return result;
        }
    }
}