using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    /// <summary>
    /// Основной объект, управляющий "сценами" с выбором варианта развития сюжета.
    /// Создает кнопки на сцене (их количество не лимитировано), то есть может быть создано хоть 10 вариантов ответа
    /// После ответа на вопрос есть возможность удалить все кнопки.
    /// </summary>
    public class ChoiceManager : MonoBehaviour
    {
        [SerializeField] public GameObject buttonPrefab;
        private List<GameObject> _spawnedObjects;

        [SerializeField] private Button moveLeft;
        [SerializeField] private Button moveRight;
        [SerializeField] private DialogueManager dialogueManager;

        private void Start()
        {
            // Инициализирует список созданных 
            _spawnedObjects = new List<GameObject>();
        }

        /// <summary>
        /// Создает и размещает на сцене кнопки, их количество совпадает с длиной переданного списка.
        /// Пока игрок не ответит и не выберет путь развития событий, кнопки влево и вправо заблокированы.
        /// </summary>
        /// <param name="choices"></param>
        public void SpawnChoiceButtons(List<ChoiceProperties> choices)
        {
            gameObject.SetActive(true);
            moveLeft.interactable = false;
            moveRight.interactable = false;
            
            for (var i = 0; i < choices.Count; ++i)
            {
                try
                {
                    GameObject go = Instantiate(buttonPrefab, transform, true);
                    go.transform.localPosition = new Vector2(0, 40 * ((choices.Count / 2) - i));
                    _spawnedObjects.Add(go);
                    go.GetComponentInChildren<TMP_Text>().text = choices[i].choiceText;
                    var i1 = i;
                    
                    // Есть два типа вопросов с выбором ответа: перемещение по одной сюжетной ветке
                    // Просто меняется id внутри сюжетной линии (перескакивает на другой диалог)
                    if (choices[i].type == ChoiceSwitchType.InOneScene)
                    {
                        // добавляю слушателя на кнопку, который вызывет метод в менеджере диалогов
                        go.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            dialogueManager.GoToStoryFrame(choices[i1].idTransfer);
                            moveLeft.interactable = true;
                            moveRight.interactable = true;
                        });
                    }
                    // И другой тип вопроса: с перемещением на другую сцену
                    else
                    {
                        // добавляю слушателя на кнопку, который вызывет метод в менеджере диалогов
                        go.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            dialogueManager.GoToAnotherStory(choices[i1].story);
                            moveLeft.interactable = true;
                            moveRight.interactable = true;
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }

        /// <summary>
        /// После ответа на вопрос, кнопки уничтожаются,
        /// а сама панель становится неактивной
        /// </summary>
        public void DestroyAllButtons()
        {
            foreach (GameObject go in _spawnedObjects)
            {
                Destroy(go);
            }
            gameObject.SetActive(false);
        }
    }
}