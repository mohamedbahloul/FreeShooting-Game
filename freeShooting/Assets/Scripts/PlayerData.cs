﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData  
{
    public int currentLevel;
    public short winCount;
    public short loseCount;
    public uint damageDone;
    public short gamePlayed;
    public byte playerPicture;
    public string PlayerName;
    public string SelectedPlayer;
    public string[] SelectedTowers=new string[6];
    public ushort diamonds;
    public int XP;
    public byte playersNumber;
    public byte towersNumber;
    public string selectedGun;
    public byte[] gunsLevel = new byte[GameManager.instance.guns.Length];
    public bool[] gunsLocked = new bool[GameManager.instance.guns.Length];
    // public string[] UnlockedPlayers = new string[GameManager.instance.players.Length];
    // public string[] Unlockedtowers =new string[GameManager.instance.Towers.Length];
    public byte[] towersLevel = new byte[GameManager.instance.Towers.Length];
    public byte[] playersLevel=new byte[GameManager.instance.players.Length];
    public bool[] lockPlayersData=new bool[GameManager.instance.players.Length];
    public bool[] lockTowersData=new bool[GameManager.instance.Towers.Length];
    public bool music;
    public bool vibrate;
    public bool notification;
    public bool soundEffect;


    public PlayerData()
    {
        music= GameManager.instance.music;
        vibrate=GameManager.instance.vibrate;
        notification=GameManager.instance.notification;
        soundEffect= GameManager.instance.soundEffect;
        currentLevel = GameManager.instance.CurrentLevel;
        winCount = GameManager.instance.winCount;
        loseCount = GameManager.instance.loseCount;
        damageDone = GameManager.instance.damageDone;
        gamePlayed = GameManager.instance.gamePlayed;
        playerPicture = GameManager.instance.playerPicture;
        PlayerName = GameManager.instance.playerName;
        diamonds = GameManager.instance.diamond;
        XP = GameManager.instance.XP;
        selectedGun = GameManager.instance.getGun().name;
        SelectedPlayer = GameManager.instance.getPlayer().name;
        for (int i = 0; i < GameManager.instance.Towers.Length;i++)
        {
            lockTowersData[i] = GameManager.instance.Towers[i].locked;
            towersLevel[i] = GameManager.instance.Towers[i].level;
        }
        for(int i = 0; i < GameManager.instance.guns.Length; i++)
        {
            gunsLevel[i] = GameManager.instance.guns[i].level;
            gunsLocked[i] = GameManager.instance.guns[i].locked;
        }
        for(int k=0;k<6;k++)
        {
            SelectedTowers[k] = GameManager.instance.GetSelectedTowers()[k].name;
            //Debug.Log(SelectedTowers[k]);
        }
        for (int j = 0; j < GameManager.instance.players.Length;j++)
        {
            lockPlayersData[j] = GameManager.instance.players[j].locked;
            playersLevel[j] = GameManager.instance.players[j].level;
        }
        towersNumber = GameManager.instance.TowersNumber;



    }
    public void addTower()
    {
        for (int i = 0; i < GameManager.instance.Towers.Length; i++)
        {
            lockTowersData[i] = GameManager.instance.Towers[i].locked;
            towersLevel[i] = GameManager.instance.Towers[i].level;
        }
        SaveSystem.SavePlayer();
    }
}
