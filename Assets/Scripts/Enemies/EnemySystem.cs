using Interfaces;
using UnityEngine;

namespace Enemies
{
    public class EnemySystem : MonoBehaviour, IDamageDealer
    {
        protected EnemyMain main;

        protected virtual void Awake()
        {
            main = GetComponent<EnemyMain>();
        }
    }
}
