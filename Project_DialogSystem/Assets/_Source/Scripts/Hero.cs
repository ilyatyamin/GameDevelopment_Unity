using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Описывает класс персонажа, который говорит реплику
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/Hero", fileName = "Hero")]
    public class Hero : ScriptableObject
    {
        public string heroName;
        public Sprite image;
    }
}