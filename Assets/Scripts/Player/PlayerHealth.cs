using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : PlayerSystem
    {
        public static event Action OnPlayerDamaged;

        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageDealer damageDealer))
            {
                OnPlayerDamaged?.Invoke();
            }
        }
    }
}