using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField, Min(0.01f)]
        private float _speed = 1f;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void OnMove(DirectionType type)
        {
            if (type == DirectionType.None) return;
            OnMove(type.ConvertTypeFromDirection(), type.ConvertTypeFromRotation());
        }

        public void OnMove(Vector3 direction, Vector3 rotation)
        {
            OnMove(direction);
            transform.eulerAngles = rotation;
        }

        public void OnMove(Vector3 direction)
        {
            if (_rb == null) return;
            _rb.velocity += (Vector2)direction * Time.deltaTime * _speed;
        }
    }
}