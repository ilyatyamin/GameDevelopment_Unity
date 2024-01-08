using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Resources
{
    public class ProductionBuilding : MonoBehaviour
    {
        [SerializeField] private ResourceBank _resourceBank;
        [SerializeField] private GameResource _resource;

        // in seconds
        private float _productionTime = 1.0f;

        /// <summary>
        /// Calculates how much needs to be added to the current amount of resources
        /// </summary>
        public int CountToAdd
        {
            get => _resourceBank.GetResource(_resourceBank.GetLevelFromResource(_resource)).Value;
        }

        /// <summary>
        /// Calculates time of resource production
        /// </summary>
        public float ProductionTime
        {
            get => GetSpeedOfProduction();
            set => _productionTime = value;
        }
        
        private float GetSpeedOfProduction()
        {
            GameResource level = _resourceBank.GetLevelFromResource(_resource);
            return Math.Max(_productionTime * (1.01f - _resourceBank.GetResource(level).Value / 100.0f), 0.01f);
        }

        /// <summary>
        /// Increases value of resource
        /// </summary>
        public void Increase()
        {
            StartCoroutine(IncreaseCoroutine());
        }

        private IEnumerator IncreaseCoroutine()
        {
            yield return new WaitForSeconds(ProductionTime);
            _resourceBank.ChangeResource(_resource, CountToAdd);
        }
    }
}
