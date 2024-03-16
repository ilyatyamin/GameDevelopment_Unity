using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace CardGame
{
    /// <summary>
    /// Singleton, that allows to start game and to do other manipulations with cards
    /// </summary>
    public class CardGame
    {
        private static CardGame _instance;
        public static CardGame Instance
        {
            get
            {
                return _instance ??= new CardGame();
            }
        }

        private static int _counter = 1;
        private List<CardLayout> _layouts = new();

        private readonly Dictionary<CardInstance, CardView> _cardDictionary = new(); 
        private List<CardAsset> _initialCards;
        private int handCapacity;
        public CardLayout centerLayout;

        public void Init(List<CardLayout> layouts, List<CardAsset> assets, int capacity, CardLayout center)
        {
            _layouts = layouts;
            _initialCards = assets;
            handCapacity = capacity;
            centerLayout = center;

            StartGame();
        }

        private void StartGame()
        {
            // Для каждого игрока создаем CardInstance для каждого CardAsset из начальных карт
            foreach (var layout in _layouts)
            {
                foreach (var cardAsset in _initialCards)
                {
                    CreateCard(cardAsset, layout.LayoutId);
                }
            }
        }

        private void CreateCard(CardAsset asset, int layoutNumber)
        {
            var instance = new CardInstance(asset)
            {
                LayoutId = layoutNumber
            };
            CreateCardView(instance);
            MoveToLayout(instance, layoutNumber);
        }

        private void CreateCardView(CardInstance instance)
        {
            GameObject newCardInstance = new GameObject($"Card {_counter++}");
            
            CardView view = newCardInstance.AddComponent<CardView>();
            Image image = newCardInstance.AddComponent<Image>();
            
            view.Init(instance, image);
            
            Button button = newCardInstance.AddComponent<Button>();
            button.onClick.AddListener(view.PlayCard);

            instance.AssociatedObject = newCardInstance;
            newCardInstance.transform.SetParent(_layouts[instance.LayoutId].transform);

            _cardDictionary[instance] = view;
        }

        private void MoveToLayout(CardInstance card, int layoutId) // !!!
        {
            card.LayoutId = layoutId;
            card.AssociatedObject.transform.SetParent(_layouts[layoutId].transform);
            foreach (var layout in _layouts)
            {
                layout.Update();
            }
        }
        
        // Я не использую метод RecalculateLayout, потому что расчитываю сдвиг по дочерним индексам.
        // Когда один элемент пропадает, то локация других автоматически перерасчитывается и так.

        public List<CardView> GetCardsInLayout(int layoutId)
        {
            return _cardDictionary.Where(x => x.Key.LayoutId == layoutId).Select(x => x.Value).ToList();
        }
        
        private List<CardInstance> GetInstancesInLayout(int layoutId)
        {
            return _cardDictionary.Where(x => x.Key.LayoutId == layoutId).Select(x => x.Key).ToList();
        }

        public void StartTurn()
        {
            foreach (var layout in _layouts)
            {
                ShuffleLayout(layout.LayoutId);
                var cards = GetCardsInLayout(layout.LayoutId);
                for (int i = 0; i < handCapacity; ++i)
                {
                    cards[i].StatusOfCard = CardStatus.Hand;
                }
            }
        }

        private void ShuffleLayout(int layoutId)
        {
            var cards = GetInstancesInLayout(layoutId);

            List<(int, int)> pairs = new List<(int, int)>();
            for (int i = 0; i < cards.Count; ++i)
            {
                for (int j = i + 1; j < cards.Count; ++j)
                {
                    pairs.Add((i, j));
                }
            }

            Random rnd = new Random();
            pairs = pairs.OrderBy(_ => rnd.Next()).ToList();

            for (int i = 1; i < cards.Count; ++i)
            {
                cards[pairs[i].Item1].AssociatedObject.transform.SetSiblingIndex(pairs[i].Item2);
            }
        }
    }
}