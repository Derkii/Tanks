using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent), typeof(ConditionComponent))]
    public class BotComponent : MonoBehaviour
    {
        private DirectionType _dirType;
        public ConditionComponent ConditionComponent;
        [SerializeField]
        private Transform _startPosition;
        private FireComponent _fireComponent;
        private InputComponent _player;
        private MoveComponent _moveComponent;
        private Rigidbody2D _rb;
        [SerializeField]
        private float _maxVelocity, _maxSpeed;
        [SerializeField, Min(0.0f)]
        private float _rotationTime = 1f;
        private Vector3 velocity;
        private bool _isSeekPlayer;
        [SerializeField]
        private float _minTime, _maxTime;
        private bool _wait = false;

        public bool Lose { get; set; }

        private void Start()
        {
            _fireComponent = GetComponent<FireComponent>();
            _player = GameObject.Find("Player").GetComponent<InputComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            StartCoroutine(Fire());
            _rb = GetComponent<Rigidbody2D>();
            StartCoroutine(Rotation());
        }

        private IEnumerator Fire()
        {
            while (true)
            {
                if (Lose == false)
                {
                    _fireComponent.OnFire();
                }
                yield return null;
            }
        }

        private IEnumerable<RaycastHit2D> Raycast()
        {
            return Physics2D.RaycastAll(_startPosition.transform.position, -_startPosition.right, 10000f);
        }
        private void FixedUpdate()
        {
            if (Lose) return;
            var hits = Raycast();
            var iteration = 0;
            foreach (var hit in hits.Where(t => t.transform != null))
            {
                iteration++;
                if (hit.collider.gameObject.tag == "Cell")
                {
                    var cellComponent = hit.collider.gameObject.GetComponent<CellComponent>();

                    if (cellComponent.IsDestroyable == true)
                    {
                        StartCoroutine(ChangeDirection(UnityEngine.Random.Range(_minTime, _maxTime)));
                    }

                    break;
                }
                else if (hit.collider.gameObject.tag == "Water"
                    || hit.collider.gameObject.tag == "BackGroundTileMap"
                    || (hit.collider.gameObject.tag == "FrameTileMap" && hits.Count(t => t.transform != null) == 2
                    || hit.collider.gameObject.tag == "Water" 
                    || hit.collider.gameObject.tag == "TankBot" && iteration >= 2))
                {
                    ChangeDirection();
                    break;
                }
            }
            if (_dirType == DirectionType.None)
            {
                SeekPlayer();
            }
            else
            {
                _isSeekPlayer = false;
                _moveComponent.OnMove(_dirType);
            }
        }

        private IEnumerator ChangeDirection(float delay)
        {
            if (_wait == false && _isSeekPlayer == false)
            {
                _wait = true;
                yield return new WaitForSeconds(delay);
                yield return ChangeDirectionCoroutine();
                yield return new WaitForFixedUpdate();
                _wait = false;
            }
        }

        private void ChangeDirection()
        {
            StartCoroutine(ChangeDirectionCoroutine());
        }

        private IEnumerator ChangeDirectionCoroutine()
        {
            var oldType = _dirType;
            var newType = _dirType;
            while (newType == oldType)
            {
                newType = _dirType.RandomType();
                yield return null;
            }
            _dirType = newType;
        }

        public void SeekPlayer()
        {
            if (Lose) return;
            _isSeekPlayer = true;
            var desired_velocity = (_player.transform.position - transform.position).normalized * _maxVelocity;
            var steering = desired_velocity - (Vector3)_rb.velocity;
            steering = Vector3.ClampMagnitude(steering, _maxVelocity) / _rb.mass;
            velocity = Vector3.ClampMagnitude(_rb.velocity + (Vector2)steering, _maxSpeed);
            _rb.velocity = velocity;
        }

        private IEnumerator Rotation()
        {
            while (true)
            {
                while (!_isSeekPlayer)
                {
                    yield return null;
                }
                var startRotation = transform.rotation;
                var endRotation = Quaternion.FromToRotation(Vector3.up, velocity);
                var currentTime = 0f;
                var time = _rotationTime;

                while (currentTime <= time)
                {
                    transform.rotation = Quaternion.Slerp(startRotation, endRotation, 1f - (time - currentTime) / time);
                    currentTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}