﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManagerPartie : MonoBehaviour
{
    public static GameManagerPartie instance;

    public short GameDamage;
    [Header("Player 1: player")]
    public TowerScript towerBase;
    private PlayerScript player;
    public GameObject itemParent;
    public Vector3 playerPos = new Vector3(0, 1.8f, -28);
    public Vector3 playerTowerPos = new Vector3(0, -7.4f, -38);
    public TowerScript[] towersSelected = new TowerScript[6];
    public GameObject player_;
    public GameObject playerGun_;
    GunsScript playerGun;
    public GameObject playerTowerBase_;
    public short playerCoins = 1000;
    public Text playerCoinsTxt;
    public GameObject playerMagic1;
    public GameObject playerMagic2;
    public short playerDamage = 0;
    public uint playerTotalDamage = 0;
    public byte playerKills = 0;

    [Header("Player 2: enemy")]
    public TowerScript enemybase;
    private PlayerScript enemy;
    public Vector3 enemyPos = new Vector3(0, 1.8f, 28);
    public Vector3 enemyTowerPos = new Vector3(0, -7.4f, 38);
    public TowerScript[] EnemySelectedTowers = new TowerScript[6];
    public GameObject enemy_;
    public GameObject enemyGun_;
    GunsScript enemyGun;
    public GameObject enemyTowerBase_;
    public short enemyCoins = 1000;
    public Text enemyCoinsTxt;
    [Range(1,3)]
    public byte enemylvl;
    public short enemyDamage = 0;
    public byte enemyKills = 0;
    public GameObject enemyMagic1;
    public GameObject enemyMagic2;

    [Header("Win lose")]
    public GameObject winPanel;
    public GameObject losePanel;
    private bool gameOver = false;
    private void Awake()
    {
        instance = this;
        
    }
    void Start()
    {
        player = GameManager.instance.getPlayer();
        enemy = GameManager.instance.players[Random.Range(0,4)];
        playerGun = GameManager.instance.getGun();
        enemyGun = GameManager.instance.guns[Random.Range(0, 5)];

        //Debug.Log(player.name);
        setMagic();
        towersSelected = GameManager.instance.GetSelectedTowers();

        ChangeSprites();
        instantiatePrefabs();

        playerCoinsTxt.text = playerCoins.ToString();
        enemyCoinsTxt.text = enemyCoins.ToString();
        
    }

    public void enableMagic1()
    {
        switch (player.name)
        {
            case "Taurus":
                MagicFunctions.instance.TaurusMagic1(0);
                break;
            case "Panda":
                MagicFunctions.instance.PandaMagic1(0);
                break;
            case "Rabbit":
                MagicFunctions.instance.RabbitMagic1(0);
                break;
            case "Pig":
                MagicFunctions.instance.PigMagic1(0);
                break;
        }
        playerMagic1.GetComponent<Button>().interactable = false;
        //playerMagic1.GetComponent<Image>().color = new Color(120, 120, 120);
        playerDamage = 0;
    }
    public void enableMagic2()
    {
        switch (player.name)
        {
            case "Taurus":             
                MagicFunctions.instance.TaurusMagic2(0);
                break;
            case "Panda":               
                MagicFunctions.instance.PandaMagic2(0);
                break;
            case "Rabbit": 
                MagicFunctions.instance.RabbitMagic2(0);
                break;
            case "Pig":  
                MagicFunctions.instance.PigMagic2();
                break;
        }
        playerMagic2.GetComponent<Button>().interactable = false;
        //playerMagic2.GetComponent<Image>().color = new Color(120, 120, 120);
        playerKills = 0;
    }
    private void setMagic()
    {
        playerMagic1.GetComponent<Image>().sprite = GameManager.instance.getPlayer().magic1.image;
        playerMagic2.GetComponent<Image>().sprite = GameManager.instance.getPlayer().magic2.image;

        //playerMagic1.GetComponent<Image>().color = new Color(120, 120, 120);
        //playerMagic2.GetComponent<Image>().color = new Color(120, 120, 120);
        playerMagic1.GetComponent<Button>().interactable = false;
        playerMagic2.GetComponent<Button>().interactable = false;
    }
    public void SetMagic1Enable()
    {      
        playerMagic1.GetComponent<Button>().interactable = true;
        //playerMagic1.GetComponent<Image>().color = Color.white;
    }
    public void SetMagic2Enable()
    {
        playerMagic2.GetComponent<Button>().interactable = true;
        //playerMagic2.GetComponent<Image>().color = Color.white;
    }
    private void instantiatePrefabs()
    {
        playerTowerBase_= Instantiate(towerBase.prefab, playerTowerPos, Quaternion.Euler(0, 0, 0));
        changeLayerMask(playerTowerBase_, "Player");
        playerTowerBase_.GetComponent<towerInf>().SetHealth(towerBase.Get_health_player());

        enemyTowerBase_ =Instantiate(enemybase.prefab, enemyTowerPos, Quaternion.Euler(0, 0, 0));
        changeLayerMask(enemyTowerBase_, "Enemy");
        enemyTowerBase_.GetComponent<towerInf>().SetHealth(towerBase.Get_health_enemy(enemylvl));

        player_ = Instantiate(player.prefab, playerPos, Quaternion.Euler(0, 0, 0));
        enemy_ = Instantiate(enemy.prefab, enemyPos, Quaternion.Euler(0, 180, 0));
        player_.GetComponent<playerMovement>().SetHealth(player.Get_health_player());
        enemy_.GetComponent<playerMovement>().SetHealth(enemy.Get_health_enemy(enemylvl));
        enemy_.GetComponent<playerMovement>().enabled = false;
        changeLayerMask(enemy_, "Enemy");
        changeLayerMask(player_, "Player");

        playerGun_ = Instantiate(playerGun.prefab,player_.transform.GetChild(1).transform);
        enemyGun_ = Instantiate(enemyGun.prefab,enemy_.transform.GetChild(1).transform);
        playerGun_.GetComponent<playerShooting>().SetDamage(20);
        playerGun_.GetComponent<playerShooting>().SetFireRate(playerGun.Get_fireRate_Gun_player());
        enemyGun_.GetComponent<playerShooting>().SetDamage(playerGun.Get_damage_Gun_enemy(enemylvl));
        enemyGun_.GetComponent<playerShooting>().SetFireRate(playerGun.Get_fireRate_Gun_enemy(enemylvl));
        playerGun_.GetComponent<playerShooting>().bullet_ = playerGun.GunBullet;
        enemyGun_.GetComponent<playerShooting>().bullet_ = enemyGun.GunBullet;
    }

    public void ChangeInteractableSpritesPrice()
    {
        for (int i = 0; i < 6; i++)
        {
            if (towersSelected[i].cost > playerCoins)
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = false;
                //change sprite to non interactable
                itemParent.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = towersSelected[i].Lockedimage;
            }
            else
            {
                if (itemParent.transform.GetChild(i).GetChild(3).gameObject.activeInHierarchy == false)
                {
                    //Debug.Log("itemParent.transform.GetChild(" + i + ").GetChild(3).gameObject.activeSelf == false");
                    itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
                    //change sprite to interactable
                    itemParent.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = towersSelected[i].image;
                }             
            }
            //itemParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = towersSelected[i].cost.ToString();
        }
    }

    public void ChangeSprites()
    {
        for (int i = 0; i < 6; i++)
        {
            /*if (towersSelected[i].cost > playerCoins)
            {
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = false;
                //change sprite to non interactable
                itemParent.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = towersSelected[i].Lockedimage;
            }
            else
            {*/
                itemParent.transform.GetChild(i).GetComponent<Button>().interactable = true;
                //change sprite to interactable
                itemParent.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = towersSelected[i].image;
                itemParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = towersSelected[i].cost.ToString();
            /*}
            itemParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = towersSelected[i].cost.ToString();
            */
        }
    }

    public void ChangeCosts()
    {
        for (int i = 0; i < 6; i++)
        {
            itemParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = towersSelected[i].cost.ToString();
        }
    }

    private void changeLayerMask(GameObject go, string layer)
    {
        go.layer = LayerMask.NameToLayer(layer);
        foreach (Transform g in go.transform)
        {
            g.gameObject.layer = LayerMask.NameToLayer(layer);
            foreach (Transform gobj in g.transform)
            {
                gobj.gameObject.layer = LayerMask.NameToLayer(layer);
            }
        }
    }

    public void win()
    {
        if (!gameOver)
        {
            Debug.Log("winnnnnnn");
            GameManager.instance.winCount++;
            GameManager.instance.gamePlayed++;
            GameManager.instance.damageDone += GameDamage;
            AIeasy.CurrentState = AIeasy.AIState.wait;
            winPanel.SetActive(true);
            uint winXP;
            //GameManager.instance.CurrentLevel
            //playerTotalDamage
            if (GameManager.instance.CurrentLevel !=0)
            {
                winXP = (uint)(playerTotalDamage * Mathf.Pow(GameManager.instance.CurrentLevel + 1, 2) / Mathf.Pow(GameManager.instance.CurrentLevel, 2) * 100);
            }
            else
            {
                winXP = (uint)(playerTotalDamage) / 100;
            } 

            winPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "x" + winXP.ToString();
            GameManager.instance.UpdateXp((int)winXP);

            if ((byte)Random.Range(1, 3) == 2)
            {
                short winGem = (short)Random.Range(1, 50);
                winPanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
                winPanel.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "x" + winGem.ToString();
                GameManager.instance.diamond += winGem;
            }
            SaveSystem.SavePlayer();
            gameOver = true;
        }
    }
    public void lose()
    {
        if (!gameOver)
        {
            AIeasy.CurrentState = AIeasy.AIState.wait;
            GameManager.instance.loseCount++;
            GameManager.instance.damageDone += GameDamage;
            GameManager.instance.gamePlayed++;

            losePanel.SetActive(true);

            //GameManager.instance.CurrentLevel
            //playerTotalDamage
            int curlvl = (int)(0.1f * Mathf.Sqrt(GameManager.instance.XP));
            short winXP = (short)(curlvl * Random.Range(50, 150) / 4);

            losePanel.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "x" + winXP.ToString();
            GameManager.instance.UpdateXp(winXP);

            SaveSystem.SavePlayer();
            gameOver = true;
        }

    }
}
