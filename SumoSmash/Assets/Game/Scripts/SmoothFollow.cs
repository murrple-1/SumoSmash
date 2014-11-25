using UnityEngine;

public class SmoothFollow : MonoBehaviour 
{
	public Transform target;
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	
	public void LateUpdate()
	{
		if (!target)
			return;
		
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
			
		float currentRotationAngle = this.transform.eulerAngles.y;
		float currentHeight = this.transform.position.y;
		
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
	
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
	
		Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
		
		this.transform.position = new Vector3(target.position.x, currentHeight, target.position.z);
		this.transform.position -= currentRotation * Vector3.forward * distance;
		
		this.transform.LookAt (target);	
	}
}
