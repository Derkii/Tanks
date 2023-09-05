using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] [Min(0.01f)] private float _speed = 1f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(DirectionType type)
        {
            if (type == DirectionType.None) return;
            Move(type.ConvertTypeToDirection(), type.ConvertTypeToRotation());
        }

        public void Move(Vector3 direction, Vector3 rotation)
        {
            Move(direction);
            transform.eulerAngles = rotation;
        }

        public void Move(Vector3 direction)
        {
            _rb.velocity = direction * _speed;
        }
    }
}