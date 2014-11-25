using UnityEngine;

public class Credits : MonoBehaviour
{
    
    private static readonly int NumberOfJoySticks = 5;
    
    public void Update()
    {
        for (int i = 0; i < NumberOfJoySticks; i++)
        {
            string joyName = "Joy" + (i + 1);
            if (Input.GetButtonDown(joyName + "_B"))
            {
                HandleBack();
            }
        }
    }
    
    private void HandleBack()
    {
        Application.LoadLevel("MainMenu");
    }
}
