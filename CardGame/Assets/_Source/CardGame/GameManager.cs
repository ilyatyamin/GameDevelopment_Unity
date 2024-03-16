using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<CardLayout> layouts;
        [SerializeField] private List<CardAsset> assets;
        [SerializeField] private int handCapacity;
        [SerializeField] private CardLayout centerLayout;

        private void Start()
        {
            int id = 0;
            foreach (var layout in layouts)
            {
                layout.LayoutId = id++;
            }
            
            CardGame.Instance.Init(layouts, assets, handCapacity, centerLayout);
        }
        
        public void StartTurn()
        {
           CardGame.Instance.StartTurn();
        }
    }
}