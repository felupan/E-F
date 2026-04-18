using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class EnemyMain : MonoBehaviour
    {
        public Animator Anim { get; private set; }

        private void Awake()
        {
            Anim = GetComponent<Animator>();
        }
    }
}
