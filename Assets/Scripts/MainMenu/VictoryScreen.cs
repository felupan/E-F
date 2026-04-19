using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private AudioClip music;

        private void Start()
        {
            StartCoroutine(WaitToMainMenu());
        }

        private IEnumerator WaitToMainMenu()
        {
            AudioManager.Instance.PlayMusic(music, 0.2f);
            yield return new WaitForSeconds(10);
            AudioManager.Instance.StopMusic();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
