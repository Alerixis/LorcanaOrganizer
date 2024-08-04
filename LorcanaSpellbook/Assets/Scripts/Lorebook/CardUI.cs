using LorcanaLorebook.ScriptableObjects;
using UnityEngine;


namespace LorcanaLorebook.UI
{
    public class CardUI : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        private Card cardToDisplay;

        public void InitializeCardUI(Card card)
        {
            cardToDisplay = card;
            spriteRenderer.sprite = Sprite.Create(card.CardImage, new Rect(0, 0, 840, 1174), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}