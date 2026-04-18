using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : PlayerSystem
    {
        public event Action OnPlayerDamaged;

        private bool _hitCooldown;
        private float _cooldownTime = 3f;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageDealer damageDealer) && !_hitCooldown)
            {
                OnPlayerDamaged?.Invoke();
                _hitCooldown = true;
                StartCoroutine(HitCooldown(_cooldownTime));
            }
        }

        private IEnumerator HitCooldown(float wait)
        {
            yield return new WaitForSeconds(wait);
            _hitCooldown = false;
        }
    }
}