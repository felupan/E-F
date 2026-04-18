using System;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image[] lifeSprites;
        
        private void OnEnable()
        {
            PlayersLivesManager.Instance.OnLivesChange += UpdateLivesUI;
        }

        private void OnDisable()
        {
            PlayersLivesManager.Instance.OnLivesChange -= UpdateLivesUI;
        }

        private void UpdateLivesUI(int lives)
        {
            for (int i = 0; i < lifeSprites.Length; i++)
            {
                lifeSprites[i].color = i < lives ? Color.white : Color.black;
            }
        }
    }
}
