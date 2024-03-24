using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Scripts
{
    /// <summary>
    /// Основной класс, руководящий поведением диалоговой системы.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueStory currentStory;
        [SerializeField] private ChoiceManager choiceManager;

        [SerializeField] private TMP_Text authorText;
        [SerializeField] private TMP_Text describeText;
        [SerializeField] private Image authorSprite;

        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        [SerializeField] private bool isAnimationNeeded;
        
        private Random _rnd;

        private void Start()
        {
            // инициализирует текущую сцену
            currentStory.Init(-1);
            _rnd = new Random();
            ShowSentenceUp();
        }

        /// <summary>
        /// Инициализирует новый диалог. Метод используется в случае смены сюжетной ветки.
        /// </summary>
        /// <param name="story">Ссылка на объект DialogueStory, описывающий сюжетную линию</param>
        private void InitNewDialogue(DialogueStory story)
        {
            currentStory = story;
            story.Init(0);
        }

        /// <summary>
        /// Перелистывает сюжетную линию вперед на 1 ход
        /// </summary>
        public void ShowSentenceUp()
        {
            try
            {
                // Уничтожаем кнопки с предыдущего вопроса, если они есть
                choiceManager.DestroyAllButtons();
                
                // Получаем данные о текущем ходе
                ActionProperties sentence = currentStory.GetAndIncreaseSentence();
                describeText.text = "";
                
                // Если ход - ход с выбором, то спавним кнопки с выбором вариантов
                if (sentence.type == StoryType.StoryWithChoice)
                {
                    choiceManager.SpawnChoiceButtons(sentence.choices);
                }

                // Если нужна анимация печатания, то начинаем ее
                if (isAnimationNeeded)
                {
                    PrintTyping(sentence.text);
                }
                else
                {
                    describeText.text = sentence.text;
                }

                authorText.text = sentence.hero.heroName;
                authorSprite.sprite = sentence.hero.image;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        
        /// <summary>
        /// Перелистывает сюжетную линию влево на 1 ход
        /// </summary>
        public void ShowSentenceDown()
        {
            try
            {
                // Уничтожаем кнопки с предыдущего вопроса, если они есть
                choiceManager.DestroyAllButtons();
                
                // Получаем данные о текущем ходе
                ActionProperties sentence = currentStory.GetAndDecreaseSentence();
                describeText.text = "";
                
                // Если ход - ход с выбором, то спавним кнопки с выбором вариантов
                if (sentence.type == StoryType.StoryWithChoice)
                {
                    choiceManager.SpawnChoiceButtons(sentence.choices);
                }

                // Если нужна анимация, то воспроизводим ее
                if (isAnimationNeeded)
                {
                    PrintTyping(sentence.text);
                }
                else
                {
                    describeText.text = sentence.text;
                }

                authorText.text = sentence.hero.heroName;
                authorSprite.sprite = sentence.hero.image;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>
        /// Метод начала анимации печатания текста на поле
        /// </summary>
        /// <param name="text">текст, который следует расположить в поле для текста</param>
        private void PrintTyping(string text)
        {
            rightButton.enabled = false;
            leftButton.enabled = false;
            StartCoroutine(AnimationTyping(text));
        }

        /// <summary>
        /// Обновляет состояние сцены. Бывает нужен, если была изменена сюжетная линия
        /// </summary>
        private void UpdateScene()
        {
            try
            {
                choiceManager.DestroyAllButtons();
                
                ActionProperties sentence = currentStory.GetCurrentState();
                describeText.text = "";
                
                if (sentence.type == StoryType.StoryWithChoice)
                {
                    choiceManager.SpawnChoiceButtons(sentence.choices);
                }

                if (isAnimationNeeded)
                {
                    PrintTyping(sentence.text);
                }
                else
                {
                    describeText.text = sentence.text;
                }

                authorText.text = sentence.hero.heroName;
                authorSprite.sprite = sentence.hero.image;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>
        /// Анимация печатания текста
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        IEnumerator AnimationTyping(string sentence)
        {
            foreach (char ch in sentence)
            {
                describeText.text += ch;
                // время печатания одной буквы - рандомное число от 0.01 до 0.15 секунды. 
                yield return new WaitForSecondsRealtime(_rnd.Next(10, 150) / 1000.0f);
            }
            // Разблокируем кнопки, так как анимация закончилась
            rightButton.enabled = true;
            leftButton.enabled = true;
        }

        /// <summary>
        /// Переводит текущую сюжетную линию на i-й шаг
        /// </summary>
        /// <param name="i">индекс перевода сюжетной линии</param>
        public void GoToStoryFrame(int i)
        {
            try
            {
                currentStory.SetIterator(i);
                UpdateScene();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>
        /// Переводит ход событий на новую сюжетную линию
        /// </summary>
        /// <param name="story">объект типа DialogueStory</param>
        /// <param name="iterator">необязательный параметр, индекс на который переводится новая сюжетная линия</param>
        public void GoToAnotherStory(DialogueStory story, int iterator = 0)
        {
            try
            {
                InitNewDialogue(story);
                GoToStoryFrame(iterator);
                UpdateScene();
            }
            catch (Exception ex)
            { 
                Debug.Log(ex.Message);
            }
        }
    }
}
