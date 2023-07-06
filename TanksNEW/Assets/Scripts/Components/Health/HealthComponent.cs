using Components.Lose;
using Game.Settings;
using UnityEngine;

namespace Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        private int _startHealth;
        private bool _played;
        private int _health = 5;
        public int Healh => _health;
        public int StartHealh => _startHealth;

        private void Start()
        {
            _startHealth = _health = GameSettings.GetStartHealth(this);;
        }

        public void Damage(int damage)
        {
            _health -= damage;
            Debug.Log(_health);
            if (_health <= 0)
            {
                var losable = GetComponent<ILosable>();
                Debug.Log(Healh);
                losable.Lose();
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }
    }
}