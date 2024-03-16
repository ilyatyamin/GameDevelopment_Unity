using UnityEngine;

namespace CardGame
{
    public class CardInstance
    {
        internal readonly CardAsset Asset;

        public int LayoutId { get; set; }
        internal CardStatus Status { get; set; }
        public int CardPosition { get; set; }
        public GameObject AssociatedObject { get; set; }

        public CardInstance(CardAsset asset)
        {
            Asset = asset;
        }
    }
}