using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Resources
{
    public class ProductionBuyer : MonoBehaviour
    {
        [SerializeField] private GameResource _resource;
        [SerializeField] private ResourceBank _bank;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button _button;
        private int _price = 10;

        private void Start()
        {
            text.text = $"+1 to\n {_resource.ToString()}\n {Price} coins";
        }

        private int Price
        {
            get => _price * _bank.GetResource(_bank.GetLevelFromResource(_resource)).Value;
        }
        
        /// <summary>
        /// Method that can buy improvement to resource production's fabric
        /// </summary>
        public void BuyAddOn()
        {
            var value = _bank.GetResource(_resource).Value;
            if (value >= Price)
            {
                _bank.GetResource(_resource).Value -= Price;
                _bank.GetResource(_bank.GetLevelFromResource(_resource)).Value += 1;
                Fill(Color.green);
            }
            else
            {
                Fill(Color.red);
            }
            text.text = $"+1 to\n {_resource.ToString()}\n {Price} coins";
        }
        
        /// <summary>
        /// Indicates status of purchase. Button will blink green or red depending on status
        /// </summary>
        /// <param name="color">Status of purchasing: green if balance is enough and red in another case</param>
        private void Fill(Color color)
        {
            _button.interactable = false;
            StartCoroutine(FillCoroutine(300, color));
        }

        private IEnumerator FillCoroutine(int steps, Color color)
        {
            for (int i = 0; i < steps; ++i)
            {
                // Change color every 50 milliseconds
                if (i % 50 == 0 && i % 100 == 0)
                {
                    _button.image.color = color;
                }
                else if (i % 50 == 0 && i % 100 != 0)
                {
                    _button.image.color = Color.white;
                }
                yield return new WaitForEndOfFrame();
            }
            _button.interactable = true;
            yield return new WaitForEndOfFrame();
        }
    }
}
