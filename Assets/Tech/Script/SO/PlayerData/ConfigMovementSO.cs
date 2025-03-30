using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigMovementSO", menuName = "Player/Config Movement")]
[InlineEditor]
public class ConfigMovementSO : ScriptableObject
{
    [Title("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float sprintSpeed;
    public float rotationSpeed;

    [Title("Ground Check & Jumping")] 
    public float jumpForwardSpeed = 5;
    public float jumpHeight = 3;
    public float fallSpeed = 2;
    public float gravity = -9.8f;
    public float groundYVelocity = -20;
    public float fallStartVelocity = -5;
    public float groundCheckSphereRadius = .3f;
    
}
