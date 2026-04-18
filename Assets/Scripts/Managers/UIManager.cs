using System;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image[] lifeSprites;
        
        public static UIManager Instance { get; private set; }

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
            PlayersLivesManager.OnLivesChange += UpdateLivesUI;
        }

        private void OnDisable()
        {
            PlayersLivesManager.OnLivesChange -= UpdateLivesUI;
        }

        private void Start()
        {
            UpdateLivesUI(PlayersLivesManager.Instance.Lives);
        }

        private void UpdateLivesUI(int lives)
        {
            Debug.Log($"Me he updateado las vidas perro");
            for (int i = 0; i < lifeSprites.Length; i++)
            {
                lifeSprites[i].color = i < lives ? Color.white : Color.black;
            }
        }
    }
}
