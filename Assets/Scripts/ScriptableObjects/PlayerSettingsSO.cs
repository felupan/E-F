using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Movement")]
    public int moveForce;
    public int jumpForce;
    public float gravityScale;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public float detectionRadius;
}
