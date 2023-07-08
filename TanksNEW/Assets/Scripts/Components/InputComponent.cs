using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Components
{
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent), typeof(Rigidbody2D))]
    public class InputComponent : MonoBehaviour
    {
        [SerializeField]
        private KeyCode _fireKey;
        [SerializeField] private InputAction _movement;
        private FireComponent _fireComponent;
        private MoveComponent _moveComponent;
        private DirectionType _lastDir;
        private DirectionType _currentDirection;
        private bool _collision;
        [Inject]
        private SoundManager _soundManager;

        private void Start()
        {
            _fireComponent = GetComponent<FireComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _movement.Enable();
        }

        private void FixedUpdate()
        {
            if (_collision)
            {
               _soundManager.Play(SoundManager.SoundType.PlayerTankCollisionWithBlock);
            }
            _moveComponent.Move(_currentDirection);
        }

        private void Update()
        {
            if (Input.GetKey(_fireKey))
            {
                _fireComponent.Fire();
            }
            var dir = _movement.ReadValue<Vector2>();

            if (dir.x != 0f && dir.y != 0f)
            {
                _currentDirection = _lastDir;
            }
            else if (dir.x == 0f && dir.y == 0f)
            {
                _currentDirection = DirectionType.None;
            }
            else
            {
                _currentDirection = dir.ConvertDirectionFromType();
                _lastDir = _currentDirection;
                if (_collision == false)
                {
                    _soundManager.Play(SoundManager.SoundType.PlayerTankMoving);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _collision = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            _collision = false;
        }
        
        private void OnDestroy()
        {
            _movement.Dispose(); 
        }
    }
}