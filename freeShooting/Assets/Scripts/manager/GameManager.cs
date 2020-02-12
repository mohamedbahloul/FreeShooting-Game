﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerScript[] players;
    public GameObject inventory;
    public TowerScript[] Towers;
    private PlayerScript playerSelected;
    private TowerScript[] towersSelected = new TowerScript[6];
    private TowerScript[] towersNotSelected;
   // public PlayerData data;



    private void Awake()
    {
        instance = this;
        loadData();
        towersNotSelected = new TowerScript[(Towers.Length - towersSelected.Length)];
        
        remplirSelectedTower();
        FillTowersNotSelected();
        //SaveSystem.SavePlayer();
         //data = SaveSystem.loadPlayerData();
        
       // fillSelectedtowers();
        
        // Debug.Log(data.SelectedPlayer);
    }
    private void loadData()
    {
         PlayerData data = SaveSystem.loadPlayerData(); 
        for (int i=0;i<players.Length;i++)
        {
            if(players[i].name==data.SelectedPlayer)
            {
                playerSelected = players[i];
                
            }
            
            players[i].locked = data.lockPlayersData[i];
            players[i].level = data.playersLevel[i];
        }
        Debug.Log(playerSelected.name);
       /* int k = 0;
        for(int i=0;i<Towers.Length;i++)
        {
            for(int j = 0; i < 6; j++)
            {
                if(Towers[i].name==data.SelectedTowers[j])
                {
                    towersSelected[k] = Towers[i];
                }
            }
            Towers[i].locked = data.lockTowersData[i];
        }*/
    }
    public PlayerScript getPlayer()
    {
        return playerSelected;
    }
    public void setPlayer(PlayerScript Player)
    {
        playerSelected = Player;
    }
    /*private void fillSelectedtowers()
    {
        int k = 0;
        bool test;
        for (int i = 0; i < Towers.Length; i++)
        {
            test = true;
            for (int j = 0; j < data.SelectedTowers.Length; j++)
            {
                if (Towers[i].name == data.SelectedTowers[j])
                {
                    test = false;
                    break;
                }
            }
            if (!test)
            {
                towersSelected[k] = Towers[i];
                k++;
                Debug.Log(towersSelected[k].name);
            }
        }
    }*/
    private void FillTowersNotSelected()
    {
        int k = 0;
        bool test;
        for(int i=0;i<Towers.Length;i++)
        {
            test = true;
            for (int j = 0; j<towersSelected.Length;j++)
            {
                if (Towers[i] == towersSelected[j])
                {
                    test = false ;
                    break;
                }
            }
            if (test)
            {
                towersNotSelected[k] = Towers[i];
                k++;
            }
        }
    }
    public TowerScript[] GetSelectedTowers()
    {
        return towersSelected;
    }
    public TowerScript[] GetNonSelectedTowers()
    {
        return towersNotSelected;
    }
    public void remplirSelectedTower()
    {
        for (int j = 0; j < 6; j++)
        {
            towersSelected[j] = Towers[j];
        }
    }
    public void  setSelectedTower(TowerScript[] towers)
    {
        towersSelected = towers;
    }

}
