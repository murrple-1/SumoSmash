using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    private static readonly int NumberOfJoySticks = 5;
    private static readonly string RematchTextStr = "Rematch?\nA - YES\nB - NO";
    
    public Sumo[] Sumos;
    public GUIText[] PlayerText;
    public GUIText RematchText;
    public SmoothFollow SmoothFollow;
    private Sumo winner;
    public float KillY;
    
    public int[] TestPlayers;
    
    private Sumo lastConfirmedAlive;
    
    public void Start()
    {
        RematchText.text = RematchTextStr;
        
        if(PlayerData.PlayerList == null)
        {
            PlayerData.PlayerList = new List<int>();
            foreach(int i in TestPlayers)
            {
                PlayerData.PlayerList.Add(i);
            }
        }
        
        for (int i = 0; i < PlayerData.PlayerList.Count; i++)
        {
            Sumo sumo = Sumos[i];
            int joyStickNumber = PlayerData.PlayerList[i];
            sumo.gameObject.SetActive(true);
            sumo.JoyStickNumber = joyStickNumber;
            
            PlayerText[i].enabled = true;
        }
    }
    
    public void Update()
    {
        if (winner == null)
        {
            int livingSumos = 0;
            for(int i = 0; i < PlayerData.PlayerList.Count; i++)
            {
                Sumo sumo = Sumos[i];
                if(sumo.gameObject.transform.position.y < KillY)
                {
                    sumo.Lost();
                } else 
                {
                    livingSumos++;
                    lastConfirmedAlive = sumo;
                }
            }
            if(livingSumos <= 1) {
                winner = lastConfirmedAlive;
                winner.Win();
                ShowWinner();
            }
        } else
        {
            for(int i = 0; i < NumberOfJoySticks; i++)
            {
            string joyName = "Joy" + (i + 1);
                if (Input.GetButtonDown(joyName + "_A"))
                {
                    Replay(true);
                } else if (Input.GetButtonDown(joyName + "_B"))
                {
                    Replay(false);
                }
            }
        }
    }
    
    private void ShowWinner()
    {
        SmoothFollow.target = winner.transform;
        SmoothFollow.distance = 5;
        SmoothFollow.height = 2;
        RematchText.enabled = true;
    }
    
    private void Replay(bool rematch)
    {
        if (rematch)
        {
            Application.LoadLevel("SumoSmash");
        } else
        {
            Application.LoadLevel("MainMenu");
        }
    }
}
