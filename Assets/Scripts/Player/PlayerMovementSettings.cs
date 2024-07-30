using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerMovementSettings : ScriptableObject
{
    public LayerMask PlayerLayer;
    public LayerMask GoundLayer;
    public bool intergerInputSnapping = true;

    [Header("Movement")]
    public float maxSpeed = 14;
    public float acceleration = 120;
    public float groundDeceleration = 60;
    public float airDeceleration = 30;
    public float groundingForce = -1.5f;
    public float groundedDistance = 0.05f;

    [Header("JUMP")]
    public float jumpPower = 36;
    public float maxFallSpeed = 40;
    public float fallAcceleration = 110f;
    public float jumpEndEarlyGravityModifier = 3;
    public float coyoteTime = .8f;
    public float jumpBuffer = .2f;

    [Header("Shadow")]
    public float swimSpeed = 18;
    public float shadowJumpPower = 42;
    public float shadowJumpHorizontalAcceleration = 60;

}
