using UnityEngine;
using System.Collections;

public class FlagWaver : MonoBehaviour
{
    
    public Vector3 StartRotation;
    public Vector3 EndRotation;
    public float RotTime;
    private float timer = 0.0f;
    private Vector3 step;
    private bool goingToStart = true;
    
    public void Start()
    {
        Vector3 startQ = gameObject.transform.localEulerAngles;
        step = new Vector3((StartRotation.x - startQ.x) / RotTime, (StartRotation.y - startQ.y) / RotTime, (StartRotation.z - startQ.z) / RotTime);
    }
    
    public void Update()
    {
        float delta = Time.deltaTime;
        Vector3 startQ = gameObject.transform.localEulerAngles;
        gameObject.transform.localEulerAngles = new Vector3(startQ.x + (step.x * delta), startQ.y + (step.y * delta), startQ.z + (step.z * delta));
        timer += delta;
        
        if (timer >= RotTime)
        {
            timer = 0.0f;
            Vector3 currentQ = gameObject.transform.localEulerAngles;
            if (goingToStart)
            {
                step = new Vector3((EndRotation.x - currentQ.x) / RotTime, (EndRotation.y - currentQ.y) / RotTime, (EndRotation.z - currentQ.z) / RotTime);
            } else
            {
                step = new Vector3((StartRotation.x - currentQ.x) / RotTime, (StartRotation.y - currentQ.y) / RotTime, (StartRotation.z - currentQ.z) / RotTime);
            }
            goingToStart = !goingToStart;
        }
    }
}
