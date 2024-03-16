using System;
using UnityEngine;

namespace CardGame
{
    public class CardLayout : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        internal int LayoutId;
        private bool _faceUp;

        internal void Update()
        {
            var cardsInLayout = CardGame.Instance.GetCardsInLayout(LayoutId);
            
            foreach (var card in cardsInLayout)
            {
                try
                {
                    switch (card.StatusOfCard)
                    {
                        case CardStatus.CardDeck:
                        {
                            Transform cardTransform = card.GetComponent<Transform>();
                            cardTransform.localPosition = CalculateCardDeckPosition(cardTransform.GetSiblingIndex());
                            card.Rotate(_faceUp);
                            break;
                        }
                        case CardStatus.Hand:
                        {
                            Transform cardTransform = card.GetComponent<Transform>();
                            cardTransform.localPosition = CalculateHandPosition(cardTransform.GetSiblingIndex());
                            card.Rotate(_faceUp);
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception ex)
                {
                    // TODO smth.
                }
                
            }
        }

        private Vector2 CalculateCardDeckPosition(int siblingIndex)
        {
            return new Vector2(siblingIndex * offset.x, 0);
        }
        
        private Vector2 CalculateHandPosition(int siblingIndex)
        {
            return new Vector2(siblingIndex * offset.x, 10);
        }

    }
}