using UnityEngine;
using System.Collections.Generic;

public class PlayerSelect : MonoBehaviour
{
    
    public class GameType
    {
        public TextMesh SelectMesh;
    }
    
    private static readonly int MinPlayersToStart = 2;
    private static readonly int NumberOfJoySticks = 5;
    public TextMesh[] ActionTextMeshes;
    public TextMesh[] HintTextMeshes;
    public TextMesh PressStartMesh;
    private IList<int> started = new List<int>();
    
    public void Update()
    {

        for (int i = 0; i < started.Count; i++)
        {
            ActionTextMeshes [i].text = "READY";
            ActionTextMeshes [i].color = Color.yellow;
            HintTextMeshes [i].text = "";
            HintTextMeshes [i].color = Color.yellow;
        }
        
        for (int i = started.Count; i < ActionTextMeshes.Length; i++)
        {
            ActionTextMeshes [i].text = "PRESS A";
            ActionTextMeshes [i].color = Color.black;
            HintTextMeshes [i].text = "To Join";
            HintTextMeshes [i].color = Color.black;
        }
        
        PressStartMesh.renderer.enabled = started.Count >= MinPlayersToStart;
        
        for (int i = 0; i < NumberOfJoySticks; i++)
        {
            string joyName = "Joy" + (i + 1);
            if (Input.GetButtonDown(joyName + "_A"))
            {
                Ready(i + 1);
            } else if (Input.GetButtonDown(joyName + "_B"))
            {
                if (!IsReady(i + 1))
                {
                    HandleBack();
                } else
                {
                    Unready(i + 1);
                }
            }
            
            if (started.Count >= MinPlayersToStart)
            {
                if (Input.GetButtonDown(joyName + "_Start"))
                {
                    HandleStart();
                }
            }
        }
    }
    
    private void Ready(int joyNumber)
    {
        if (!started.Contains(joyNumber))
        {
            started.Add(joyNumber);
        }
    }
    
    private void Unready(int joyNumber)
    {
        started.Remove(joyNumber);
    }
    
    private bool IsReady(int joyNumber)
    {
        return started.Contains(joyNumber);
    }
    
    private void HandleBack()
    {
        Application.LoadLevel("MainMenu");
    }
    
    private void HandleStart()
    {
        PlayerData.PlayerList = started;
        Application.LoadLevel("SumoSmash");
    }
}
