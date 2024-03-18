using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ResourceBank _bank;

        void Start()
        {
            // Start value of game resources
            _bank.ChangeResource(GameResource.Humans, 10);
            _bank.ChangeResource(GameResource.Food, 5);
            _bank.ChangeResource(GameResource.Wood, 5);
        }
    }
}