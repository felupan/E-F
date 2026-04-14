using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Managers
{
    public class RopeManager : MonoBehaviour
    {
        public struct RopePoint
        {
            public Vector3 currentPos;
            public Vector3 oldPos;
            public bool locked;
        }
    
        [Header("Rope")] 
        [SerializeField] private int numberOfSegments;
        [SerializeField] private float segmentsLength;
        [SerializeField] private float gravityScale;
        [SerializeField] private float ropeForce;
        [SerializeField] private float retractSpeed;
        [SerializeField] private float finishRetractForce;
        [SerializeField] private float stuckOnXSpeed;
        
        private PlayerMain player1;
        private PlayerMain player2;

        private bool player1Anchored;
        private bool player2Anchored;
        
        private float maxForceY;

        private IEnumerator retractRopeCoroutine;
        
        private LineRenderer lineRenderer;
        private List<RopePoint> ropePoints = new List<RopePoint>();


        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            PlayerMovement.OnAnchorChanged += UpdateAnchorState;
            PlayerMovement.OnRetractRope += RetractRope;
            PlayerMovement.OnStopRetractRope += StopRetractingRope;
        }

        private void OnDisable()
        {
            PlayerMovement.OnAnchorChanged -= UpdateAnchorState;
            PlayerMovement.OnRetractRope -= RetractRope;
            PlayerMovement.OnStopRetractRope -= StopRetractingRope;
        }

        private void Start()
        {
            player1 = GameManager.Instance.player1;
            player2 = GameManager.Instance.player2;
            Init();
        }

        private void FixedUpdate()
        {
            UpdatePoints();
            for (int i = 0; i < 50; i++)
            {
                ApplyConstrains();
            }
            ApplyPlayerForces();
        }

        private void Update()
        {
            Render();
        }

        private void Init()
        {
            lineRenderer.positionCount = numberOfSegments;
        
            for (int i = 0; i < numberOfSegments; i++)
            {
                RopePoint ropePoint;
                float t = (float)i / (numberOfSegments - 1);
                ropePoint.currentPos = Vector2.Lerp(player1.transform.position, player2.transform.position, t);
                ropePoint.oldPos = ropePoint.currentPos;
            
                if (t == 0 || i == numberOfSegments - 1) ropePoint.locked = true;
                else ropePoint.locked = false;
            
                ropePoints.Add(ropePoint);
            }
        }

        private void UpdatePoints()
        {
            for (int i = 0; i < numberOfSegments; i++)
            {
                RopePoint ropePoint = ropePoints[i];
                if (ropePoint.locked)
                {
                    Vector3 playerPos;
                    if (i == 0)
                    {
                        playerPos = player1.transform.position;
                    }
                    else
                    {
                        playerPos = player2.transform.position;
                    }

                    ropePoint.currentPos = playerPos;
                    ropePoint.oldPos = ropePoint.currentPos;
                }
                else
                {
                    Vector3 velocity = ropePoint.currentPos - ropePoint.oldPos;
                    Vector3 newPos = ropePoint.currentPos + velocity + Vector3.down * (gravityScale * Time.fixedDeltaTime * Time.fixedDeltaTime);
                    ropePoint.oldPos = ropePoint.currentPos;
                    ropePoint.currentPos = newPos;
                }

                ropePoints[i] = ropePoint;
            }
        }

        private void ApplyConstrains()
        {
            for (int i = 0; i < numberOfSegments - 1; i++)
            {
                RopePoint ropePoint = ropePoints[i];
                RopePoint nextRopePoint = ropePoints[i + 1];
            
                float distance = (ropePoint.currentPos - nextRopePoint.currentPos).magnitude;
                float error = distance - segmentsLength;
                Vector3 direction = (nextRopePoint.currentPos - ropePoint.currentPos).normalized;

                if (!ropePoint.locked && !nextRopePoint.locked)
                {
                    ropePoint.currentPos += direction * (error * 0.5f);
                    nextRopePoint.currentPos -= direction * (error * 0.5f);
                }
                else if (!ropePoint.locked)
                {
                    ropePoint.currentPos += direction * error;
                }
                else if (!nextRopePoint.locked)
                {
                    nextRopePoint.currentPos -= direction * error;
                }

                ropePoints[i] = ropePoint;
                ropePoints[i + 1] = nextRopePoint;
            }
        }

        private void Render()
        {
            for (int i = 0; i < numberOfSegments; i++)
            {
                lineRenderer.SetPosition(i, ropePoints[i].currentPos);
            }
        }

        private void ApplyPlayerForces()
        {
            if (player1Anchored && player2Anchored) return;
            
            float totalLength = segmentsLength * numberOfSegments;
            float currentDistance = Vector3.Distance(player1.transform.position, player2.transform.position);
            float error = currentDistance - totalLength;
            //Debug.Log($"totalLength: {totalLength}, currentDistance: {currentDistance}, error: {error}");
            
            if (error <= 0) return;
            
            Vector3 direction = (player2.transform.position - player1.transform.position).normalized; 
            Vector3 force = direction * (error * ropeForce);
            
            maxForceY = ropeForce * 0.8f;
            
            if (direction.y < 0)
            {
                maxForceY = -maxForceY;
            }
            
            if (player1Anchored)
            {
                maxForceY = Mathf.Min(maxForceY, force.y);
                force.y = maxForceY;
                player2.Rb.AddForce(-force); 
            }
            else if (player2Anchored)
            {
                maxForceY = Mathf.Max(maxForceY, force.y);
                force.y = maxForceY;
                player1.Rb.AddForce(force);
            }
            else
            {
                player1.Rb.AddForce(force);
                player2.Rb.AddForce(-force);   
            }
            //Debug.Log($"direction: {direction}, force: {force}, player1Anch: {player1Anchored}, player2Anch: {player2Anchored}");
        }

        private void UpdateAnchorState(PlayerMain.PlayerType playerType, bool isAnchored)
        {
            if (playerType == PlayerMain.PlayerType.PlayerOne) player1Anchored = isAnchored;
            else player2Anchored = isAnchored;
        }

        private void StopRetractingRope(PlayerMain.PlayerType player)
        {
            PlayerMain playerRetracting;
            PlayerMain playerRetracted;
            if (player == PlayerMain.PlayerType.PlayerOne)
            {
                playerRetracting = player1;
                playerRetracted = player2;
            }
            else
            {
                playerRetracting = player2;
                playerRetracted = player1;
            }
            StopCoroutine(retractRopeCoroutine);
            playerRetracted.Movement.SetInputEnabled(true);
            playerRetracting.Movement.SetInputEnabled(true);
        }

        private void RetractRope(PlayerMain.PlayerType player)
        {
            PlayerMain playerRetracting;
            PlayerMain playerRetracted;
            if (player == PlayerMain.PlayerType.PlayerOne)
            {
                playerRetracting = player1;
                playerRetracted = player2;
            }
            else
            {
                playerRetracting = player2;
                playerRetracted = player1;
            }
            playerRetracted.Movement.SetInputEnabled(false);
            playerRetracting.Movement.SetInputEnabled(false);

            retractRopeCoroutine = RetractPlayer(playerRetracting, playerRetracted);
            StartCoroutine(retractRopeCoroutine);
        }

        private IEnumerator RetractPlayer(PlayerMain retracting, PlayerMain retracted)
        {
            if ((retracting.Rb.position - retracted.Rb.position).y < 0f) yield break;
            
            Vector2 finalDir = Vector2.zero;
            Vector2 previousPos = retracted.Rb.position;
            
            while (Vector3.Distance(retracting.transform.position, retracted.transform.position) > 0.005f)
            {
                Vector2 direction = (retracting.transform.position - retracted.transform.position).normalized;
                float distance = Vector2.Distance(retracted.Rb.position, previousPos);
                bool isStuck = !retracted.Movement.IsGround && distance <= 0.01f;
                
                if (isStuck)
                {
                    direction += Vector2.up * stuckOnXSpeed;
                }
                
                previousPos = retracted.Rb.position;
                retracted.Rb.MovePosition(retracted.Rb.position + direction * (retractSpeed * Time.fixedDeltaTime));
                finalDir = direction;
                yield return new WaitForFixedUpdate();
            }
            retracted.Rb.AddForce(finalDir * finishRetractForce, ForceMode2D.Impulse);
            retracting.Movement.SetInputEnabled(true);
            retracted.Movement.SetInputEnabled(true);
        }
    }
}
