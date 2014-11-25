using UnityEngine;

public class TextButton : MonoBehaviour
{
    public TextButtonGroup TextButtonGroup;
    public TextMesh TextMesh;
    
    public void OnMouseEnter()
    {
        TextButtonGroup.OnMouseEnterDelegate(TextMesh);
    }
    
    public void OnMouseExit()
    {
        // do nothing
    }
    
    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TextButtonGroup.OnMouseDownDelegate(TextMesh);
        }
    }
}

