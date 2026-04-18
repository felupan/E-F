using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Timeline
{
    public class SubtitleController : MonoBehaviour
    {
        public void NextScene()
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
