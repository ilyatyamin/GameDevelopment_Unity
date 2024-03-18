using System;
using UnityEngine;

namespace CardGame
{
    public class CardLayout : MonoBehaviour
    {
        internal int NowInLayout = 0;
        
        [SerializeField] private Vector2 offset;
        [SerializeField] private Vector2 cardOffset;
        internal int LayoutId;

        internal bool FaceUp;

        internal void Update()
        {
            // Получаем карты для данного Layout-а
            var cardsInLayout = CardGame.Instance.GetCardsInLayout(LayoutId);
            
            foreach (var card in cardsInLayout)
            {
                try
                {
                    Transform cardTransform = card.GetComponent<Transform>();
                    
                    // В зависимости от статуса карты перемещаем ее в нужное положение
                    switch (card.StatusOfCard)
                    {
                        case CardStatus.CardDeck:
                        {
                            FaceUp = false;
                            cardTransform.localPosition = CalculatePosition(card.CardPosition, CardStatus.CardDeck);
                            card.Rotate(FaceUp);
                            break;
                        }
                        case CardStatus.Hand:
                        {
                            FaceUp = true;
                            cardTransform.localPosition = CalculatePosition(card.CardPosition, CardStatus.Hand);
                            card.Rotate(FaceUp);
                            break;
                        }
                        case CardStatus.Center:
                            FaceUp = true;
                            cardTransform.position = CardGame.Instance.CenterLayout.transform.position;
                            card.Rotate(FaceUp);
                            break;
                        case CardStatus.Deleted:
                            FaceUp = false;
                            cardTransform.localPosition = CalculatePosition(card.CardPosition, CardStatus.Deleted);
                            card.Rotate(FaceUp);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
                
            }
        }
        
        private Vector2 CalculatePosition(int siblingIndex, CardStatus status)
        {
            // Метод для расчитывания текущей позиции карты на сцене
            switch (status)
            {
                case CardStatus.CardDeck:
                    return new Vector2(siblingIndex * offset.x, 0);
                case CardStatus.Hand:
                    return new Vector2(siblingIndex * offset.x, cardOffset.y);
                case CardStatus.Deleted:
                    return new Vector2(0, cardOffset.y * siblingIndex);
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

    }
}