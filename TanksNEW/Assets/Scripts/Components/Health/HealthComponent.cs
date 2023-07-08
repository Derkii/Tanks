using Components.Lose;
using Game.Settings;
using UnityEngine;

namespace Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        private int _startHealth;
        private bool _played;
        private int _health;
        public int Health => _health;
        public int StartHealth => _startHealth;

        private void Start()
        {
            _startHealth = _health = GameSettings.GetStartHealth(this);;
        }

        public void Damage(int damage)
        {
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