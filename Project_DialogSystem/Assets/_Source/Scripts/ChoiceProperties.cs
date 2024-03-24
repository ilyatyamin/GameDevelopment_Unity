using System;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Дата-класс, описывающий один из вариантов выбора в сцене с выбором действия
    /// </summary>
    [Serializable]
    public class ChoiceProperties
    {
        // тип вопроса
        public ChoiceSwitchType type;
        public string choiceText;
        public int idTransfer;
        
        [SerializeField] [Header("Only if you need to switch to another story")] 
        public DialogueStory story;
    }
}