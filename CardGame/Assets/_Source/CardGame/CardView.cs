using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    /// <summary>
    /// Presentation of the card
    /// </summary>
    public class CardView : MonoBehaviour
    {
        private CardInstance _cardInstance;
        private Image _image;

        // Поле, инкапсулирующее Status в CardInstance
        public CardStatus StatusOfCard
        {
            get => _cardInstance.Status;
            set => _cardInstance.Status = value;
        }

        // Поле, инкапсулирующее CardPosition в CardInstance
        public int CardPosition
        {
            get => _cardInstance.CardPosition;
            set => _cardInstance.CardPosition = value;
        }

        // Меняет спрайт у image
        public void Rotate(bool up)
        {
            _image.sprite = up ? _cardInstance.Asset.onSprite : _cardInstance.Asset.offSprite;
        }
        
        public void Init(CardInstance instance, Image imageObj)
        {
            _cardInstance = instance;
            _image = imageObj;
        }

        public void PlayCard()
        {
            // В зависимости от текущего положения карты, перемещаем ее или в центр, или в колоду сброса 
            switch (_cardInstance.Status)
            {
                case CardStatus.Hand:
                    CardGame.Instance.MoveToCenter(_cardInstance);
                    _cardInstance.Status = CardStatus.Center;
                    break;
                case CardStatus.Center:
                    CardGame.Instance.MoveToTrash(_cardInstance);
                    _cardInstance.Status = CardStatus.Deleted; 
                    break;
            }
        }
    }
}