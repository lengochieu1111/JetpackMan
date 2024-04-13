using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RyoScriptableObject", menuName = "Data/Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Move")]
    public float RunSpeed = 10f;
    public float AirRunningSpeed = 13f;
    public float TimeToFlyUp = 0.2f;

    [Header("Sound")]
    public AudioClip FootstepSound;
    public AudioClip ExplosionSound;

    // public readonly int Anim_Jump = Animator.StringToHash("Anim_JumpStart");

}
