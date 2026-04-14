using System;
using Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
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
