using System;
using Components.Lose;
using Game.Settings;
using UnityEngine;

namespace Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        private int _health;
        public int StartHealth { get; private set; }

        private void Start()
        {
            StartHealth = _health = GameSettings.GetStartHealth(this);
        }

        public void Damage(int damage)
        {
            if (damage < 0) throw new ArgumentException("Damage can't be less than 0", nameof(damage));
            _health -= damage;
            if (_health <= 0)
            {
                var losable = GetComponent<ILosable>();
                losable.Lose();
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }
    }
}