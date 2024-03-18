using UnityEngine;

namespace Assets
{
    public class PlayerAnimator
    {
        private readonly Animator _animator;
        private float _speed;

        private const string SPEED = "speed";
        private readonly int SPEED_ID = Animator.StringToHash(SPEED);

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void ChangeSpeed(float speed)
        {
            _speed = speed;
        }
    }
}
