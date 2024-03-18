using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ResourceBank : MonoBehaviour
    {
        public ResourceBank()
        {
            _dictionary.Add(GameResource.Food, new ObservableInt(0));
            _dictionary.Add(GameResource.Gold, new ObservableInt(0));
            _dictionary.Add(GameResource.Humans, new ObservableInt(0));
            _dictionary.Add(GameResource.Stone, new ObservableInt(0));
            _dictionary.Add(GameResource.Wood, new ObservableInt(0));
            
            _dictionary.Add(GameResource.FoodProdLvl, new ObservableInt(1));
            _dictionary.Add(GameResource.GoldProdLvl, new ObservableInt(1));
            _dictionary.Add(GameResource.HumansProdLvl, new ObservableInt(1));
            _dictionary.Add(GameResource.StoneProdLvl, new ObservableInt(1));
            _dictionary.Add(GameResource.WoodProdLvl, new ObservableInt(1));
        }
        
        private Dictionary<GameResource, ObservableInt> _dictionary = new();

        /// <summary>
        ///  Change resource r to value v
        /// </summary>
        /// <param name="r">type or resource of enum GameResource</param>
        /// <param name="v">value that has been changed</param>
        public void ChangeResource(GameResource r, int v)
        {
            if (!_dictionary.ContainsKey(r))
            {
                _dictionary[r] = new ObservableInt(v);
            }
            else
            {
                _dictionary[r].Value += v;
            }
        }

        /// <summary>
        /// Method that return level of resource
        /// </summary>
        /// <param name="resource">Kind of resource from GameResource enum</param>
        /// <returns>Level or game resource</returns>
        public GameResource GetLevelFromResource(GameResource resource)
        {
            switch (resource)
            {
                case GameResource.Food:
                    return GameResource.FoodProdLvl;
                case GameResource.Gold:
                    return GameResource.GoldProdLvl;
                case GameResource.Humans:
                    return GameResource.HumansProdLvl;
                case GameResource.Wood:
                    return GameResource.WoodProdLvl;
                case GameResource.Stone:
                    return GameResource.StoneProdLvl;
                default:
                    return GameResource.FoodProdLvl;
            }
        }

        /// <summary>
        /// Return resource of type GameResource
        /// </summary>
        /// <param name="r">Type of GameResource</param>
        /// <returns>Value of resource</returns>
        public ObservableInt GetResource(GameResource r)
        {
            if (_dictionary.ContainsKey(r))
            {
                return _dictionary[r];
            }
            
            _dictionary[r].Value = 0;
            return _dictionary[r];
        }

    }
}