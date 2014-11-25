using UnityEngine;

public class Sumo : MonoBehaviour
{
    private static readonly string XAxis = "_LA_X";
    private static readonly string YAxis = "_LA_Y";
    private static readonly string AButton = "_A";
    private static readonly string BButton = "_B";
    private static readonly float AxisThreshold = 0.5f;
    private static readonly float LargeMass = 1000.0f;
    private static readonly float StunnedForce = 50.0f;

    public enum SumoState
    {
        Walking, Idle, Attack, Defend, Recoil, Stunned, Victory, Lost
    }
    public SumoState State { get; set; }
    
    public Animation Animation;
    private AnimationClip clip;
    
    public ProgressBar ProgressBar;
    public GUIElement ObjectLabel;
    
    public SumoConstants SumoConstants;
    
    public int JoyStickNumber { get; set; }
    private float Stamina;
    
    private float? timer;
    private Vector3 forward;
    private float forwardMomentum;
    
    public void Start() {
        State = SumoState.Idle;
        Stamina = SumoConstants.StaminaMax;
        ProgressBar.Progress = 1.0f;
        forward = gameObject.transform.rotation * new Vector3(0.0f, 0.0f, 1.0f);
        forwardMomentum = 0.0f;
        gameObject.rigidbody.mass = SumoConstants.SumoMass;
    }
    
    public void Update()
    {
        string joyName = "Joy" + JoyStickNumber;
        switch(State) {
            case SumoState.Attack: {
                if(timer.HasValue) {
                    timer += Time.deltaTime;
                } else {
                    timer = 0.0f;
                    clip = Animation.GetClip("Attack");
                    Animation.Play("Attack");
                    float momentum = forwardMomentum;
                    if(momentum < SumoConstants.MinForwardMomentum)
                    {
                        momentum = SumoConstants.MinForwardMomentum;
                    }
                    Vector3 attackVector = forward * (SumoConstants.MaxAttackForce * momentum);
                    gameObject.rigidbody.AddForce(attackVector, ForceMode.Impulse);
                    
                }
                if(timer.HasValue && timer.Value >= clip.length) {
                    timer = null;
                    State = SumoState.Idle;
                    gameObject.rigidbody.velocity = Vector3.zero;
                }
                
                float xAxis = Input.GetAxis(joyName + XAxis);
                float yAxis = Input.GetAxis(joyName + YAxis);
                if(xAxis > AxisThreshold || xAxis < -AxisThreshold || yAxis > AxisThreshold || yAxis < -AxisThreshold)
                {
                    forward = new Vector3(xAxis, 0.0f, yAxis);
                    forwardMomentum = forward.magnitude;
                    forward.Normalize();
                    Vector3 moveVector = forward * (SumoConstants.PlayerMovementSpeed * Time.deltaTime);
                    Vector3 newPosition = transform.position + moveVector;
                    transform.LookAt(newPosition + moveVector);
                    transform.position = newPosition;
                }
                break;
            }
            case SumoState.Defend: {
                AnimationClip aClip = Animation.GetClip("Defend");
                if(clip != aClip)
                {
                    clip = aClip;
                    Animation.Play("Defend");
                    gameObject.rigidbody.mass = LargeMass;
                }
                
                if(Input.GetButtonUp(joyName + BButton))
                {
                    State = SumoState.Idle;
                    gameObject.rigidbody.mass = 1.0f;
                }
                Stamina -= SumoConstants.StaminaDrain_Defend * Time.deltaTime;
                if(Stamina <= 0.0f) {
                    Stamina = 0.0f;
                    State = SumoState.Stunned;
                    gameObject.rigidbody.mass = 1.0f;
                }
                break;
            }
            case SumoState.Idle: {
                AnimationClip aClip = Animation.GetClip("Idle");
                if(clip != aClip)
                {
                    clip = aClip;
                    Animation.Play("Idle");
                }
                
                if(timer.HasValue)
                {
                    timer += Time.deltaTime;
                    if(timer.Value >= SumoConstants.StaminaRegenResetTimer)
                    {
                        Stamina += SumoConstants.StaminaRegenRate * Time.deltaTime;
                    }
                    if(Stamina >= SumoConstants.StaminaMax)
                    {
                        Stamina = SumoConstants.StaminaMax;
                    }
                } else
                {
                    timer = 0.0f;
                }
                
                float xAxis = Input.GetAxis(joyName + XAxis);
                float yAxis = Input.GetAxis(joyName + YAxis);
                if(Input.GetButtonDown(joyName + AButton))
                {
                    if((Stamina - SumoConstants.StaminaDrain_Attack) > 0.0f)
                    {
                        State = SumoState.Attack;
                        timer = null;
                        Stamina -= SumoConstants.StaminaDrain_Attack;
                    }
                } else if(Input.GetButtonDown(joyName + BButton))
                {
                    State = SumoState.Defend;
                    timer = null;
                } else if(xAxis >= AxisThreshold || xAxis <= -AxisThreshold || yAxis >= AxisThreshold || yAxis <= -AxisThreshold)
                {
                    State = SumoState.Walking;
                    timer = null;
                    forward = new Vector3(xAxis, 0.0f, yAxis);
                    forwardMomentum = forward.magnitude;
                    forward.Normalize();
                }
                break;
            }
            case SumoState.Recoil: {
                if(timer.HasValue) {
                    timer += Time.deltaTime;
                } else {
                    timer = 0.0f;
                    clip = Animation.GetClip("Recoil");
                    Animation.Play("Recoil");
                }
                if(timer.HasValue && timer.Value >= SumoConstants.RecoilTimer) {
                    timer = null;
                    State = SumoState.Idle;
                }
                break;
            }
            case SumoState.Victory: {
                AnimationClip aClip = Animation.GetClip("Victory");
                if(clip != aClip)
                {
                    clip = aClip;
                    Animation.Play("Victory");
                    ObjectLabel.enabled = false;
                }
                break;
            }
            case SumoState.Walking: {
                AnimationClip aClip = Animation.GetClip("Walking");
                if(clip != aClip)
                {
                    clip = aClip;
                    Animation.Play("Walking");
                }
                
                float xAxis = Input.GetAxis(joyName + XAxis);
                float yAxis = Input.GetAxis(joyName + YAxis);
                if(Input.GetButtonDown(joyName + AButton))
                {
                    if((Stamina - SumoConstants.StaminaDrain_Attack) > 0.0f)
                    {
                        State = SumoState.Attack;
                        Stamina -= SumoConstants.StaminaDrain_Attack;
                    }
                } else if(Input.GetButtonDown(joyName + BButton))
                {
                    State = SumoState.Defend;
                } else if(xAxis <= AxisThreshold && xAxis >= -AxisThreshold && yAxis <= AxisThreshold && yAxis >= -AxisThreshold)
                {
                    State = SumoState.Idle;
                    forwardMomentum = 0.0f;
                } else
                {
                    forward = new Vector3(xAxis, 0.0f, yAxis);
                    forwardMomentum = forward.magnitude;
                    forward.Normalize();
                    Vector3 moveVector = forward * (SumoConstants.PlayerMovementSpeed * Time.deltaTime);
                    Vector3 newPosition = transform.position + moveVector;
                    transform.LookAt(newPosition + moveVector);
                    transform.position = newPosition;
                }
                break;
            }
            case SumoState.Stunned: {
                if(timer.HasValue) {
                    timer += Time.deltaTime;
                } else {
                    timer = 0.0f;
                    clip = Animation.GetClip("Idle");
                    Animation.Play("Idle");
                    gameObject.audio.PlayOneShot(SumoConstants.StunndedSounds[Random.Range(0, SumoConstants.StunndedSounds.Length)]);
                    gameObject.rigidbody.constraints = RigidbodyConstraints.None;
                    gameObject.rigidbody.mass = SumoConstants.StunnedSumoMass;
                    gameObject.rigidbody.AddForce(new Vector3(Random.Range(-StunnedForce, StunnedForce), 0.0f, Random.Range(-StunnedForce, StunnedForce)));
                }
                if(timer.Value >= SumoConstants.StunnedTimer) {
                    timer = null;
                    State = SumoState.Idle;
                    gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    gameObject.transform.localEulerAngles = new Vector3(0.0f, gameObject.transform.localEulerAngles.y, 0.0f);
                    gameObject.rigidbody.mass = SumoConstants.SumoMass;
                }
                
                Stamina += SumoConstants.StaminaRegenRate * Time.deltaTime;
                if(Stamina >= SumoConstants.StaminaMax)
                {
                    Stamina = SumoConstants.StaminaMax;
                }
                break;
            }
            case SumoState.Lost: {
                AnimationClip aClip = Animation.GetClip("Idle");
                if(clip != aClip)
                {
                    clip = aClip;
                    Animation.Play("Idle");
                    gameObject.rigidbody.constraints = RigidbodyConstraints.None;
                    gameObject.rigidbody.AddForce(new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f)));
                    ObjectLabel.enabled = false;
                }
                break;
            }
            default: {
                break;
            }
        }
        
        ProgressBar.Progress = Stamina / SumoConstants.StaminaMax;
    }
    
    public void OnCollisionStay(Collision c)
    {
        HandleCollision(c);
    }
    
    public void OnCollisionEnter(Collision c)
    {
        HandleCollision(c);
    }
    
    private void HandleCollision(Collision c)
    {
        switch(State) {
            case SumoState.Attack: {
                Sumo s = c.gameObject.GetComponent("Sumo") as Sumo;
                if(s != null)
                {
                    switch(s.State)
                    {
                        case SumoState.Defend: {
                            // do nothing
                            break;
                        }
                        case SumoState.Attack: {
                            // do nothing
                            break;
                        }
                        case SumoState.Stunned: {
                            // do nothing
                            break;
                        }
                        case SumoState.Victory: {
                            // do nothing
                            break;
                        }
                        default: {
                            if(s.State != SumoState.Recoil)
                            {
                                s.State = SumoState.Recoil;
                                s.timer = null;
                                gameObject.audio.PlayOneShot(SumoConstants.HitSounds[Random.Range(0, SumoConstants.HitSounds.Length)]);
                            }
                            break;
                        }
                    }
                }
                break;
            }
            default: {
                break;
            }
        }
    }
    
    public void Win()
    {
        if(State != SumoState.Victory)
        {
            State = SumoState.Victory;
            clip = null;
        }
    }
    
    public void Lost()
    {
        if(State != SumoState.Lost)
        {
            State = SumoState.Lost;
            clip = null;
        }
    }
}

