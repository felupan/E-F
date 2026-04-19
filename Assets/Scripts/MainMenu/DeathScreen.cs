using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class DeathScreen : MonoBehaviour
    {
        [SerializeField] private Image fadeOut;
        [SerializeField] private AudioClip music;

        private void Start()
        {
            fadeOut.enabled = false;
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(music, 0.2f);
        }

        public void OnRetryButton()
        {
            Color c;
            c = fadeOut.color;
            c.a = 0f;
            fadeOut.color = c;
            fadeOut.enabled = true;
            fadeOut.DOFade(1f, 3).WaitForCompletion();
            AudioManager.Instance.StopMusic();
            SceneManager.LoadScene("Level1");
        }
        
        public void OnExitButton()
        {
            Application.Quit();
        }
    }
}
