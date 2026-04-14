using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerMain : MonoBehaviour
    {
        public enum PlayerType
        {
            PlayerOne,
            PlayerTwo
        }
        [field: SerializeField] public PlayerMain.PlayerType playerType { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public Animator Anim { get; private set; }
        public PlayerMovement Movement { get; private set; }

        private void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            Movement = GetComponent<PlayerMovement>();
            GameManager.Instance.RegisterPlayer(this, playerType);
        }
    }
}