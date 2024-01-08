using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace _Source.Resources
{
    public class ResourceVisual : MonoBehaviour
    {
        [SerializeField] ResourceBank _bank;
        [SerializeField] List<TMP_Text> _resourceTexts;

        private readonly List<GameResource> _gameResources = new()
            { GameResource.Food, GameResource.Gold, GameResource.Humans, GameResource.Stone, GameResource.Wood };
        
        private void Awake()
        {
            // Циклом foreach проходимся по всем типам GameResource
            foreach (GameResource res in _gameResources)
            {
                _bank.GetResource(res).OnValueChanged += value =>
                    _resourceTexts[(int)res].text = $"{value}";
            }
        }
    }
}