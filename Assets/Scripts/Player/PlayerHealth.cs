using System;
using System.Collections;
using Interfaces;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : PlayerSystem
    {
        private static readonly int IsHurt = Animator.StringToHash("isHurt");
        public static event Action OnPlayerDamaged;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageDealer damageDealer))
            {
                OnPlayerDamaged?.Invoke();
                main.Anim.SetTrigger(IsHurt);
                AudioManager.Instance.PlaySfx(main.playerSounds.hurtSound, 0.4f);
            }
        }
    }
}