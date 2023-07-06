using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent))]
    public class Projectile : MonoBehaviour
    {
        private SideType _side;
        private Vector3 _direction;

        private MoveComponent _moveComponent;

        [SerializeField]
        private int _damage = 1;
        [SerializeField]
        private float _lifeTime = 3f;
        private Rigidbody2D _rb;
        [SerializeField]
        private float _maxSpeed;
        [SerializeField]
        private float _startSpeed;

        private void Start()
        {
            _moveComponent = GetComponent<MoveComponent>();
            _rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, _lifeTime);
            _rb.velocity = _direction * _startSpeed;
        }

        private void FixedUpdate()
        {
             _moveComponent.OnMove(_direction);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
        }

        public void SetParams(DirectionType directionType, SideType side)
        {
            _side = side;
            _direction = directionType.ConvertTypeFromDirection();
        }
        public void SetParams(Vector3 dir, SideType side)
        {
            _side = side;
            _direction = dir;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var fireComponent = collision.GetComponent<FireComponent>();
            if (fireComponent != null)
            {
                if (fireComponent.Side == _side)
                {
                    return;
                }
                var bot = collision.gameObject.GetComponent<BotComponent>();
                if (bot != null && bot.Lose) return;
                SoundManager.instance.Play(SoundManager.SoundType.ProjectileCollision);
                var gm = collision.GetComponent<GodModeComponent>();
                if (gm != null && gm.IsGodmode == true)
                {
                    return;
                }
                var condition = collision.GetComponent<ConditionComponent>();
                condition.SetDamage(_damage);
                if (_side == SideType.Player && fireComponent.Side == SideType.Enemy)
                {
                    bot.SeekPlayer();
                    Destroy(gameObject);
                    return;
                }
            }
            var cellComponent = collision.GetComponent<CellComponent>();
            if (cellComponent != null)
            {
                SoundManager.instance.Play(SoundManager.SoundType.ProjectileCollision);
                if (cellComponent.IsDestroyable)
                {
                    Destroy(cellComponent.gameObject);
                }
                if (cellComponent.CanDestroyBullet == false)
                {
                    return;
                }
            }
            if (collision.gameObject.tag == "Water")
            {
                return;
            }
            if (collision.gameObject.tag == "BackGroundTileMap" || collision.gameObject.tag == "FrameTileMap")
            {
                SoundManager.instance.Play(SoundManager.SoundType.ProjectileCollision);
            }
            Destroy(gameObject);
        }
    }
}