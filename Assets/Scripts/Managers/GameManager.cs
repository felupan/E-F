using System;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [field:SerializeField] public float Gravity { get; private set; }
        
        public PlayerMain player1 { get; private set; }
        public PlayerMain player2 { get; private set; }
        
        public static GameManager Instance { get; private set; }

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

            Physics2D.gravity = new Vector2(0, -Gravity);
        }

        private void OnEnable()
        {
            PlayersLivesManager.OnGameEnd += GameOver;
        }

        private void GameOver()
        {
            SceneManager.LoadScene("GameOver");
        }

        private void OnDisable()
        {
            PlayersLivesManager.OnGameEnd -= GameOver;
        }

        private void Start()
        {
            Physics2D.IgnoreCollision(player1.GetComponent<Collider2D>(), player2.GetComponent<Collider2D>());
        }

        public void RegisterPlayer(PlayerMain player, PlayerMain.PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerMain.PlayerType.PlayerOne: player1 = player; break;
                case PlayerMain.PlayerType.PlayerTwo: player2 = player; break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerType), playerType, null);
            }
        }
    }
}
