using System;
using UnityEngine;

namespace Managers
{
    public class PlayersLivesManager : MonoBehaviour
    {
        //los jugadores comparten vidas, jugador recibe daño, lanza evento, este script
        //recibe el evento y resta vidas
        [field:SerializeField] public int Lives { get; private set; }
        public int MaxLives { get; private set; }
        public event Action OnGameEnd;
        public event Action<int> OnLivesChange;
        
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
            MaxLives = Lives;
            GameManager.Instance.player1.Health.OnPlayerDamaged += OnPlayerDamage;
            GameManager.Instance.player2.Health.OnPlayerDamaged += OnPlayerDamage;
        }

        private void OnDisable()
        {
            GameManager.Instance.player1.Health.OnPlayerDamaged -= OnPlayerDamage;
            GameManager.Instance.player2.Health.OnPlayerDamaged -= OnPlayerDamage;
        }

        private void OnPlayerDamage()
        {
            Lives--;
            OnLivesChange?.Invoke(Lives);
            if (Lives <= 0)
            {
                OnGameEnd?.Invoke();
            }
        }
    }
}
