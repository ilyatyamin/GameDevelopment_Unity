using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class CardView : MonoBehaviour
    {
        private CardInstance _cardInstance;
        private Image _image;

        public CardStatus StatusOfCard
        {
            get => _cardInstance.Status;
            set => _cardInstance.Status = value;
        }

        public void Rotate(bool up)
        {
            _image.sprite = up ? _cardInstance.Asset.onSprite : _cardInstance.Asset.offSprite;
        }
        
        public void Init(CardInstance instance, Image imageObj)
        {
            _cardInstance = instance;
            _image = imageObj;
            Rotate(true);
        }

        public void PlayCard()
        {
            _cardInstance.AssociatedObject.transform.position = CardGame.Instance.centerLayout.transform.position;
            _cardInstance.Status = CardStatus.Center;
        }
    }
}