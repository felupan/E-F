using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainMenu
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private AudioClip hoverSound;

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySfx(hoverSound, 0.4f);
            transform.DOScale(hoverScale, duration).SetEase(Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, duration).SetEase(Ease.OutBack);
        }
    }
}