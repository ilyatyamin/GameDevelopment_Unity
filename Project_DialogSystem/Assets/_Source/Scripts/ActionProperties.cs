using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Класс, описывающий настройки данного кадра: кто говорит, что говорит и тип "сцены":
    /// с выбором ответа или нет
    /// </summary>
    [Serializable]
    public class ActionProperties
    {
        // Автоматически создаваемый id сцены. Может понадобиться при дальнейшем расширении проекта
        [NonSerialized] private static int _counter;
        [NonSerialized] public int ID;
        
        public StoryType type;
        public string text;
        public Hero hero;

        [Space] [Header("Only if type is Story With Choice")]
        [SerializeField] public List<ChoiceProperties> choices;

        public ActionProperties()
        {
            ID = ++_counter;
        }
    }
}