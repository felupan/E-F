using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] private Image fadeOut;
        [SerializeField] private AudioClip buttonSelected;
        [SerializeField] private AudioClip music;

        private void Start()
        {
            fadeOut.enabled = true;
            StartCoroutine(FadeIn());
            AudioManager.Instance.PlayMusic(music, 0.4f);
        }

        public void OnStartButton()
        {
            StartCoroutine(FadeOut());
            AudioManager.Instance.PlaySfx(buttonSelected,0.4f);
        }

        public void OnExitButton()
        {
            Application.Quit();
        }

        private IEnumerator FadeOut()
        {
            fadeOut.enabled = true;
            yield return fadeOut.DOFade(1f, 3f).WaitForCompletion();
            AudioManager.Instance.StopMusic();
            SceneManager.LoadScene("AirplaneScene");
        }
        
        private IEnumerator FadeIn()
        {
            fadeOut.enabled = true;
            yield return fadeOut.DOFade(0f, 2f).WaitForCompletion();
            fadeOut.enabled = false;
        }
    }
}
