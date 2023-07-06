using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent), typeof(Rigidbody2D))]
    public class InputComponent : MonoBehaviour
    {
        private FireComponent _fireComponent;
        private MoveComponent _moveComponent;
        private DirectionType _lastDir;
        [SerializeField]
        private InputAction _fire, _movement;
        private DirectionType _currentDirection;
        private bool _collision;
        private Collider _collider;

        public bool IsPlaying { get; set; } = true;

        private void Start()
        {
            _fireComponent = GetComponent<FireComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _fire.Enable();
            _movement.Enable();
            _fire.performed += _ => _fireComponent.OnFire();
            _collider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if (IsPlaying == false)
            {
                Destroy(_collider);
                return;
            }
            _collision = false;
            _moveComponent.OnMove(_currentDirection);
        }
        private void Update()
        {
            if (IsPlaying == false) return;
            var dir = _movement.ReadValue<Vector2>();

            if (dir.x != 0f && dir.y != 0f)
            {
                _currentDirection = _lastDir;
            }
            else if (dir.x == 0f && dir.y == 0f)
            {
                _currentDirection = DirectionType.None;
                return;
            }
            else
            {
                _currentDirection = dir.ConvertDirectionFromType();
                _lastDir = _currentDirection;
                if (_collision == false)
                {
                    SoundManager.instance.Play(SoundManager.SoundType.PlayerTankMoving);
                }
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            _collision = true;
            if (collision.gameObject.IsBot() == false)
            {
                SoundManager.instance.Play(SoundManager.SoundType.PlayerTankCollisionWithBlock);
            }
        }
        public void SetCollision(bool b)
        {
            _collision = b;
        }
        private void OnDestroy()
        {
            _movement.Dispose();
            _fire.Dispose();
        }
    }
}