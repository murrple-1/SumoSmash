using UnityEngine;

public class TextButtonGroup : MonoBehaviour
{
    private static readonly int NumberOfJoySticks = 5;
    public TextMesh[] TextMeshes;
    public string[] Options;
    public Color Color;
    public Color SelectedColor;
    public float TimeBetweenMoves = 0.2f;
    public float AxisThreshold = 0.5f;
    public bool VerticalList = true;
    public bool EnableASelect = true;
    public int CurrentSelectionIndex = 0;
    private float? lastMovedTime;

    public void Update()
    {
        if (lastMovedTime.HasValue)
        {
            lastMovedTime = lastMovedTime.Value + Time.deltaTime;
            
            if (lastMovedTime.Value >= TimeBetweenMoves)
            {
                lastMovedTime = null;
            }
        }
        
        for (int i = 0; i < TextMeshes.Length; i++)
        {
            TextMesh tMesh = TextMeshes [i];
            if (i == CurrentSelectionIndex)
            {
                tMesh.color = SelectedColor;
            } else
            {
                tMesh.color = Color;
            }
        }
        
        string axisIdentifier;
        if (VerticalList)
        {
            axisIdentifier = "_LA_Y";
        } else
        {
            axisIdentifier = "_LA_X";
        }
        
        for (int i = 0; i < NumberOfJoySticks; i++)
        {
            string joyName = "Joy" + (i + 1);
            
            if (EnableASelect)
            {
                if (Input.GetButtonDown(joyName + "_A"))
                {
                    if (CurrentSelectionIndex >= 0)
                    {
                        HandleOption(Options [CurrentSelectionIndex]);
                        return;
                    }
                }
            }
            
            if (!lastMovedTime.HasValue)
            {
                float xAxis = Input.GetAxis(joyName + axisIdentifier);
                if (xAxis > AxisThreshold)
                {
                    CurrentSelectionIndex--;
                    lastMovedTime = 0.0f;
                } else if (xAxis < -AxisThreshold)
                {
                    CurrentSelectionIndex++;
                    lastMovedTime = 0.0f;
                }
                
                if (CurrentSelectionIndex < 0)
                {
                    CurrentSelectionIndex = 0;
                } else if (CurrentSelectionIndex >= TextMeshes.Length)
                {
                    CurrentSelectionIndex = TextMeshes.Length - 1;
                }
            }
        }
    }

    public virtual void OnMouseEnterDelegate(TextMesh textButton)
    {
        for (int i = 0; i < TextMeshes.Length; i++)
        {
            if (textButton == TextMeshes [i])
            {
                CurrentSelectionIndex = i;
                return;
            }
        }
    }
    
    public virtual void OnMouseDownDelegate(TextMesh textButton)
    {
        HandleOption(Options [CurrentSelectionIndex]);
    }

    protected virtual void HandleOption(string data)
    {
        if (data == "ExitApp")
        {
            Debug.Log("Exit App");
            Application.Quit();
        } else
        {
            Application.LoadLevel(data);
        }
    }
}

