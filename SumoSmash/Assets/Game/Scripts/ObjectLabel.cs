using UnityEngine;

[RequireComponent (typeof (GUIElement))]
public class ObjectLabel : MonoBehaviour {
	
	public Transform target;
	public Vector3 offset = Vector3.up;
	public bool clampToScreen = false;
	public float clampBorderLeft = 0.05f;
	public float clampBorderRight = 0.05f;
	public float clampBorderTop = 0.05f;
	public float clampBorderBottom = 0.05f;
	public new Camera camera;
	
	public void LateUpdate()
	{
		if (clampToScreen)
		{
			Vector3 relativePosition = camera.transform.InverseTransformPoint(target.position);
			relativePosition.z =  Mathf.Max(relativePosition.z, 1.0f);
            Vector3 newPosition = camera.WorldToViewportPoint(camera.transform.TransformPoint(relativePosition + offset));
            newPosition = new Vector3(Mathf.Clamp(newPosition.x, clampBorderLeft, 1.0f - clampBorderRight), 
                                      Mathf.Clamp(newPosition.y, clampBorderBottom, 1.0f - clampBorderTop), 
                                      newPosition.z);
			transform.position = newPosition;
		}
		else
		{
            Vector3 newPosition = camera.WorldToViewportPoint(target.position + offset);
			transform.position = newPosition;
		}
	}
}