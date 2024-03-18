using UnityEngine;

namespace Assets
{
    public class GizmoSpheraDrawer : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField, Range(0, 1)] private float _radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}
