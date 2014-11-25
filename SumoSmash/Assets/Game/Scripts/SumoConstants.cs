using UnityEngine;
using System.Collections;

public class SumoConstants : MonoBehaviour
{
    public float StaminaMax = 100.0f;
    public float StaminaDrain_Defend = 10.0f;
    public float StaminaDrain_Attack = 20.0f;
    public float StaminaDrain_DefendSuccess = 5.0f;
    public float StaminaRegenRate = 10.0f;
    public float StaminaRegenResetTimer = 5.0f;
    public float StunnedTimer = 5.0f;
    public float RecoilTimer = 0.7f;
    
    public float PlayerMovementSpeed = 20.0f;
    public float MaxAttackForce = 10.0f;
    public float MinForwardMomentum = 0.5f;
    
    public float SumoMass = 1.0f;
    public float StunnedSumoMass = 0.1f;
    
    public AudioClip[] HitSounds;
    public AudioClip[] StunndedSounds;
}

