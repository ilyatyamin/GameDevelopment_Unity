using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    /// <summary>
    /// Класс-"настройщик" игрового поля. Принимает из инспектора нужные параметры и инициализирует CardGame
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<CardLayout> layouts;
        [SerializeField] private List<CardAsset> assets;
        [SerializeField] private int handCapacity;
        [SerializeField] private CardLayout centerLayout;
        [SerializeField] private CardLayout bucketLayout;

        private void Start()
        {
            // Для каждого layout-а устанавливает свой id
            int id = 0;
            foreach (var layout in layouts)
            {
                layout.LayoutId = id++;
            }

            centerLayout.LayoutId = id++;
            bucketLayout.LayoutId = id;
            
            CardGame.Instance.Init(layouts, assets, handCapacity, centerLayout, bucketLayout);
        }
        
        // Начинает ход
        public void StartTurn()
        {
           CardGame.Instance.StartTurn();
        }
    }
}