using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Resources
{
    public class SlideProgresser : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private ProductionBuilding _builder;
        [SerializeField] private Button _button;
        
        private float _startPercent = 0.00f;

        /// <summary>
        /// Fill progress bar in moment when user touch the button
        /// </summary>
        public void Fill()
        {
            _button.interactable = false;
             // Debug.Log($"speed = {_builder.ProductionTime}. amount = {_builder.CountToAdd}");
            int numOfSteps = Convert.ToInt32(_builder.ProductionTime * 100);
            float dist = 1.0f / numOfSteps;
            StartCoroutine(FillCoroutine(numOfSteps, dist));
        }

        IEnumerator FillCoroutine(int steps, float dist)
        {
            for (int i = 0; i < steps; ++i)
            {
                image.fillAmount = _startPercent;
                _startPercent += dist;
                yield return new WaitForEndOfFrame();
            }
            _startPercent = 0;
            image.fillAmount = dist;
            _button.interactable = true;
            yield return new WaitForEndOfFrame();
        }
    }
}
