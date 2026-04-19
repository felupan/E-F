using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image[] lifeSprites;
        [SerializeField] private Image fadeIn;
        
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
            FadeIn();
            UpdateLivesUI(PlayersLivesManager.Instance.Lives);
        }

        private void UpdateLivesUI(int lives)
        {
            for (int i = 0; i < lifeSprites.Length; i++)
            {
                lifeSprites[i].color = i < lives ? Color.white : Color.black;
            }
        }

        private void FadeIn()
        {
            Color c;
            c = fadeIn.color;
            c.a = 1f;
            fadeIn.color = c;
            fadeIn.DOFade(0f, 2f);
        }
    }
}
