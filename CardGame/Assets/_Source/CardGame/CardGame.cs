using System;
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
        public List<CardLayout> Layouts = new();

        private readonly Dictionary<CardInstance, CardView> _cardDictionary = new(); 
        private List<CardAsset> _initialCards;
        public int HandCapacity;
        private CardLayout _bucketLayout;
        public CardLayout CenterLayout;

        public void Init(List<CardLayout> layouts, List<CardAsset> assets, int capacity, CardLayout center, CardLayout bucket)
        {
            Layouts = layouts;
            _initialCards = assets;
            HandCapacity = capacity;
            CenterLayout = center;
            _bucketLayout = bucket;
            
            // При создании игры сразу раскладываем карты, но рубашкой
            StartGame();
        }

        private void StartGame()
        {
            // Для каждого игрока создаем CardInstance для каждого CardAsset из начальных карт
            foreach (var layout in Layouts)
            {
                foreach (var cardAsset in _initialCards)
                {
                    CreateCard(cardAsset, layout.LayoutId);
                }
            }
        }

        private void CreateCard(CardAsset asset, int layoutNumber)
        {
            // Создаем карту с LayoutId, CardPosition при создании рассчитываем как текущее кол-во карт в layout (нумерация с 0)
            var instance = new CardInstance(asset)
            {
                LayoutId = layoutNumber,
                CardPosition = Layouts[layoutNumber].NowInLayout++
            };
            CreateCardView(instance);
            MoveToLayout(instance, layoutNumber);
        }

        private void CreateCardView(CardInstance instance)
        {
            // создаем объект на сцене
            GameObject newCardInstance = new GameObject($"Card {_counter++}");
            
            // добавляем ему компоненты: CardView и Image
            CardView view = newCardInstance.AddComponent<CardView>();
            Image image = newCardInstance.AddComponent<Image>();
            
            view.Init(instance, image);
            
            // добавляем ему свойство кнопки, чтобы при нажатии на изображение происходило действие
            Button button = newCardInstance.AddComponent<Button>();
            
            // добавляем "подписчика"
            button.onClick.AddListener(view.PlayCard);

            // устанавливаем ему родителя на сцене
            newCardInstance.transform.SetParent(Layouts[instance.LayoutId].transform);

            _cardDictionary[instance] = view;
        }
        
        private void MoveToLayout(CardInstance card, int layoutId)
        {
            int temp = card.LayoutId;
            card.LayoutId = layoutId;
            
            // Устаналиваем нового родителя
            _cardDictionary[card].transform.SetParent(Layouts[layoutId].transform);
            
            // Пересчитываем Layout-ы
            RecalculateLayout(layoutId);
            RecalculateLayout(temp);
        }

        public void MoveToCenter(CardInstance card)
        {
            int temp = card.LayoutId;
            
            card.LayoutId = CenterLayout.LayoutId;
            
            // Устаналиваем нового родителя
            _cardDictionary[card].transform.SetParent(CenterLayout.transform);
            
            // Пересчитываем Layout-ы
            RecalculateLayout(CenterLayout.LayoutId);
            RecalculateLayout(temp);
        }
        
        public void MoveToTrash(CardInstance card)
        {
            int temp = card.LayoutId;
            card.LayoutId = _bucketLayout.LayoutId;
            _cardDictionary[card].transform.SetParent(_bucketLayout.transform);
            
            // При помещении в сброс будем убирать у карты свойство нажатия 
            try
            {
                Button button = _cardDictionary[card].GetComponent<Button>();
                button.enabled = false;
                button.onClick.RemoveAllListeners();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            // Пересчитываем все id-шники
            RecalculateLayout(_bucketLayout.LayoutId);
            RecalculateLayout(temp);
        }

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
            // Метод для начала хода.
            foreach (var layout in Layouts)
            {
                // Перемешиваем значения в layout-е
                ShuffleLayout(layout.LayoutId);
                
                // Поворачиваем карты лицом вверх
                layout.FaceUp = true;
                
                var cards = GetCardsInLayout(layout.LayoutId);
                
                // Раздаем столько карт, сколько можем
                for (int i = 0; i < HandCapacity; ++i)
                {
                    cards[i].StatusOfCard = CardStatus.Hand;
                }
            }
        }

        private void ShuffleLayout(int layoutId)
        {
            var cards = GetInstancesInLayout(layoutId);

            // Делаем список всех пар карт
            List<(int, int)> pairs = new List<(int, int)>();
            for (int i = 0; i < cards.Count; ++i)
            {
                for (int j = i + 1; j < cards.Count; ++j)
                {
                    pairs.Add((i, j));
                }
            }

            Random rnd = new Random();
            // Устаналиваем "в рандомном порядке"
            pairs = pairs.OrderBy(_ => rnd.Next()).ToList();

            for (var i = 1; i < cards.Count; ++i)
            {
                _cardDictionary[cards[pairs[i].Item1]].transform.SetSiblingIndex(pairs[i].Item2);
            }
        }

        private void RecalculateLayout(int layoutId)
        {
            var games = GetCardsInLayout(layoutId);

            for (int i = 0; i < games.Count; ++i)
            {
                games[i].CardPosition = i;
            }
        }
    }
}