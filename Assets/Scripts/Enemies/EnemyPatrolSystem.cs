using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyPatrolSystem : EnemySystem
    {
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        [SerializeField] private Transform patrolPath;
        [SerializeField] private float patrolSpeed;
        private List<Vector3> patrolPositions = new List<Vector3>();
        private Vector3 currentDestination;
        private int currentIndex = 0;
        
        protected override void Awake()
        {
            base.Awake();
            foreach (Transform patrolPoint in patrolPath)
            {
                patrolPositions.Add(patrolPoint.position);
            }
        }

        private void Start()
        {
            StartCoroutine(PatrolAndWait());
        }

        private IEnumerator PatrolAndWait()
        {
            while (true)
            {
                CalculateNewDestination();
                FaceToDest();
                //Move
                main.Anim.SetBool(IsRunning, true);
                while (transform.position != currentDestination)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentDestination, patrolSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                //Idle
                main.Anim.SetBool(IsRunning, false);
                yield return new WaitForSeconds(Random.Range(0.5f, 1.75f));
                currentIndex = (currentIndex + 1) % patrolPositions.Count;
            }
            
        }

        private void FaceToDest()
        {
            float x = currentDestination.x - transform.position.x;
            if (Mathf.Sign(x) == 1f)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (Mathf.Sign(x) == -1f)
            {
                transform.eulerAngles = Vector3.zero;
            }
        }

        private void CalculateNewDestination()
        {
            currentDestination = patrolPositions[currentIndex];
        }
    }
}
