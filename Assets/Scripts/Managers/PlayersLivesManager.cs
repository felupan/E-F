using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Managers
{
    public class PlayersLivesManager : MonoBehaviour
    {
        //los jugadores comparten vidas, jugador recibe daño, lanza evento, este script
        //recibe el evento y resta vidas, manda evento para actualizar UI
        
        [field:SerializeField] public int Lives { get; private set; }
        public static event Action OnGameEnd;
        public static event Action<int> OnLivesChange;
        
        private bool _hitCooldown;
        private float _cooldownTime = 3f;
        
        public static PlayersLivesManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            PlayerHealth.OnPlayerDamaged += OnPlayerDamage;
        }

        private void OnDisable()
        {
            PlayerHealth.OnPlayerDamaged -= OnPlayerDamage;
        }

        private void OnPlayerDamage()
        {
            if (_hitCooldown) return;
            Lives--;
            OnLivesChange?.Invoke(Lives);
            
            if (Lives <= 0)
            {
                OnGameEnd?.Invoke();
            }
            _hitCooldown = true;
            StartCoroutine(HitCooldown(_cooldownTime));
        }
        
        private IEnumerator HitCooldown(float wait)
        {
            yield return new WaitForSeconds(wait);
            _hitCooldown = false;
        }
    }
}
