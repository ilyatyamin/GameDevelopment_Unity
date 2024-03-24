using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Сюжетная линия диалоговой системы
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "GameSystem/Story", fileName = "Story")]
    public class DialogueStory : ScriptableObject
    {
        // Список "актов" или "сцен" сюжетной линии
        [SerializeField]
        public List<ActionProperties> sentences;

        private int _currentId = 0;

        public void Init(int value)
        {
            _currentId = value;
        }

        /// <summary>
        /// Устанавливает текущее состояние диалоговой ветки на i-й шаг
        /// </summary>
        /// <param name="i">индекс, на который нужно поставить диалоговую ветку</param>
        /// <exception cref="EndOfDialogueException">=Бросает данное исключение, если индекс некорректен</exception>
        public void SetIterator(int i)
        {
            if (i <= 0 || i >= sentences.Count)
            {
                throw new EndOfDialogueException("incorrected index");
            }

            _currentId = i;
        }

        /// <summary>
        /// Возвращает текущее состояние сюжетной линии
        /// </summary>
        /// <returns>объект типа ActionProperties</returns>
        /// <exception cref="EndOfDialogueException">Бросает данное исключение, если текущий индекс некорректен</exception>
        public ActionProperties GetCurrentState()
        {
            if (_currentId < sentences.Count  && _currentId >= 0)
            {
                ActionProperties sentence = sentences[_currentId];
                return sentence;
            }

            throw new EndOfDialogueException("Index out of dialogue!");
        }
        
        /// <summary>
        /// Переводит сюжетную линию на следующий шаг
        /// </summary>
        /// <returns>Возвращает состояние сюжетной линии на следующем шаге</returns>
        /// <exception cref="EndOfDialogueException">Бросает данное исключение, если линия переходит за конец</exception>
        public ActionProperties GetAndIncreaseSentence()
        {
            if (_currentId < sentences.Count - 1)
            {
                ++_currentId;
                ActionProperties sentence = sentences[_currentId];
                return sentence;
            }

            throw new EndOfDialogueException("End of dialogue!");
        }
        
        /// <summary>
        /// Переводит сюжетную линию на шаг назад
        /// </summary>
        /// <returns>Возвращает состояние сюжетной линии на предыдущем шаге</returns>
        /// <exception cref="EndOfDialogueException">Бросает данное исключение, если линия переходит за начало</exception>
        public ActionProperties GetAndDecreaseSentence()
        {
            if (_currentId > 0)
            {
                --_currentId;
                ActionProperties sentence = sentences[_currentId];
                return sentence;
            }

            throw new EndOfDialogueException("Beginning of dialogue!");
        }
    }
}