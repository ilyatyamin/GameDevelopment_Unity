using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets
{
    public class PlayerMovement : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        
        [SerializeField] private Transform _startPosition;
        [SerializeField] private Transform _finishPosition;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Animator _animator;
        private PlayerAnimator _playerAnimator;
        

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerAnimator = new PlayerAnimator(_animator);
            
            _camera = Camera.main;
            transform.position =
                new Vector3(_startPosition.position.x, 0, _startPosition.position.z);
        }

        private void Start()
        {
            _navMeshAgent.destination = _finishPosition.position;
        }

        private void Update()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask) && Input.GetMouseButtonDown(0))
            {
                Vector3 position = hit.point;
                _navMeshAgent.destination = position;
                _playerAnimator.ChangeSpeed(_navMeshAgent.speed);
                
            }
        }
    }
}