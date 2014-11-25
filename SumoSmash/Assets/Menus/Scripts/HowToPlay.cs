using UnityEngine;
using System.Collections;

public class HowToPlay : MonoBehaviour
{
    
    public TextMesh HowToMesh;
    private static readonly string HowToText = @"How to Play
    
    A/'A' Key - Smash
    B/'S' Key - Defend
    Left Analog/Arrow Keys - Move
    
    Be the last Sumo Warrior
    standing in the Ring!
    
    Watch your stamina!";
    private static readonly int NumberOfJoySticks = 5;
    
    public void Start()
    {
        HowToMesh.text = HowToText;
    }
    
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
