﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIeasy : MonoBehaviour
{
    public static AIState CurrentState = AIState.start;
    public  TowerScript[] towers= new TowerScript[6];
    private Vector3 buildPos;
    public TowerScript[] TowersWeCanBuild=new TowerScript[6];

    private GameObject player;
    private short currentHealth;
    public float speed = 5f;
    Vector3[] BuildPos = new[]{
        new Vector3(-15,2,15),
        new Vector3(-5,2,10),
        new Vector3(5,2,8),
        new Vector3(15,2,10),
        new Vector3(25,2,15),
        };
    public TowerScript[] Inventory1;

    //can replace with buildpos.x
    private float[] hiding = { -15, -5, 5, 15, 25 };


    public enum AIState
    {
        idle, die, shoot, build, hide,start
    }
    public static void changeState(AIState state)
    {
        CurrentState = state;
        //Debug.Log(state);
    }

    void selectPlayer() { player = GameManagerPartie.instance.player_; }
    public byte minCostTower = 0;
    // Start is called before the first frame update
    void Start()
    {
       
        //towers = GameManagerPartie.instance.EnemySelectedTowers;
        selectPlayer();
        //enemyBuildZone = GameObject.FindGameObjectsWithTag("EnemyTowersZones");
        //currentHealth = player.Get_health();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log(CurrentState);

        switch(CurrentState)
        {
            case AIState.start:
                startStrategy1();
                changeState(AIState.idle);
                break;




            case AIState.idle:
                //  Debug.Log("hani fi idle state");

                if (!isMoving)
                {
                    float rand = Random.Range(1, 3);
                    StartCoroutine(move(rand));
                }
                if ((positionManager.testbuild > 0)&& (towers[minCostTower].cost <= GameManagerPartie.instance.enemyCoins))
                {
                   
                    for (int i = 0; i < 5; i++)
                    {
                        if (positionManager.buildingGameObject[1, i] == null )
                        {
                            buildPos = BuildPos[i];
                            changeState(AIState.build);
                        }
                    }
                }
                if (!GameManagerPartie.instance.player_.activeSelf)
                {
                    changeState(AIState.shoot);
                }        
                break;
            case AIState.hide:
                Vector3 destination = new Vector3(0, transform.position.y, transform.position.z);
                float minDist = Mathf.Infinity;
                float currentPosX = transform.position.x;
                foreach (float x in hiding)
                {
                    float dist = Mathf.Abs(x - currentPosX);
                    if (dist < minDist)
                    {
                        destination.x = x;
                        minDist = dist;
                    }
                }
                
                transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
                if (Mathf.Abs(transform.position.x - destination.x) < 0.001f)
                {
                    changeState(AIState.idle);
                }
                break;

            case AIState.build:
                /* int j = 0;
                 //Debug.Log("hani fi build state");
                 for (int i = 0; i < 6; i++)
                 {
                     if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                     {
                         TowersWeCanBuild[j] = towers[i];
                         j++;
                     }
                 }
                 positionManager.add(TowersWeCanBuild[Random.Range(0, j - 1)], buildPos, GameManagerPartie.instance.enemylvl);
                 changeState(AIState.idle);*/
                strategy1();
                break;

            case AIState.shoot:
                LeanTween.moveX(GameManagerPartie.instance.enemy_, 5, 0.2f).setEaseInOutSine();
                if (GameManagerPartie.instance.player_.activeSelf)
                {
                    changeState(AIState.idle);
                }
                if ((positionManager.testbuild > 0) && (towers[minCostTower].cost <= GameManagerPartie.instance.enemyCoins))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (positionManager.buildingGameObject[1, i] == null)
                        {
                            buildPos = BuildPos[i];
                            changeState(AIState.build);
                        }
                    }
                }
                break;
            case AIState.die:
                //player win
                break;
        }
    }
    bool isMoving = false;
    IEnumerator move(float time)
    {
        isMoving = true;
        float rand = Random.Range(-18, 28);
        LeanTween.moveX(GameManagerPartie.instance.enemy_, rand, time).setEaseLinear();
        yield return new WaitForSeconds (time);
        isMoving = false;
        //Debug.Log("moved");
    }

    Vector3 choosingDestination()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(-17.8f, 28.8f), 1.8f, 25);
        }
        while (position.x < 5);
        return position;
    } 
    Vector3 choosingPos() { return BuildPos[Random.Range(0, 5)];}

    TowerScript choosingBuild() { return towers[Random.Range(0, 5)]; }
    void seePlayer() { //see player build on idle state 
    }
    public void startStrategy1()
    {
        towers = Inventory1;
        if (positionManager.buildingGameObject[1, 2] == null)
        {
            int random = Random.Range(0, 2);
            if (random == 0 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 1 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[2], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if(positionManager.buildingGameObject[1,0]==null)
        {
            int random = Random.Range(2, 4);
            
            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[0], GameManagerPartie.instance.enemylvl);
            }
            else
                return;   
        }
        if (positionManager.buildingGameObject[1, 4] == null)
        {
            int random = Random.Range(2, 4);
            
            if (random == 2 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 3 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[4], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 1] == null)
        {
            
            int random = Random.Range(4, 6);
          
            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[1], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }
        if (positionManager.buildingGameObject[1, 3] == null)
        {
            int random = Random.Range(4, 6);
           
            if (random == 4 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else if (random == 5 && towers[(byte)random].cost <= GameManagerPartie.instance.enemyCoins)
            {
                positionManager.add(towers[(byte)random], BuildPos[3], GameManagerPartie.instance.enemylvl);
            }
            else
                return;
        }


    }
    public void strategy1 ()
    {

        int j;
        switch (buildPos.x)
        {
            case -15:
                if (positionManager.buildingTowerScript[0, 0] != null)
                {
                    
                    switch (positionManager.buildingTowerScript[0, 0].name)
                    {
                        case "block tower":

                             j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                                changeState(AIState.idle);
                            break;

                        case "mirror tower":
                             j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins)&&((towers[i].name== "block tower")||(towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                             j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                           

                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") ))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                     
                    }
                }

                break;
            case -5:
                if (positionManager.buildingTowerScript[0, 1] != null)
                {

                    switch (positionManager.buildingTowerScript[0, 1].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                }
                break;
            case 5:
                if (positionManager.buildingTowerScript[0, 2] != null)
                {

                    switch (positionManager.buildingTowerScript[0, 2].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                }
                break;
            case 15:
                if (positionManager.buildingTowerScript[0, 3] != null)
                {

                    switch (positionManager.buildingTowerScript[0, 3].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                }
                break;
            case 25:
                if (positionManager.buildingTowerScript[0, 4] != null)
                {

                    switch (positionManager.buildingTowerScript[0, 4].name)
                    {
                        case "block tower":

                            j = 0;
                            for (int i = 2; i < 6; i++)
                            {
                                if (towers[i].cost <= GameManagerPartie.instance.enemyCoins)
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mirror tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "block tower") || (towers[i].name == "healing tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "freezing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "block tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "lazer tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;


                        case "tesla":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "healing tower":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "healing tower") || (towers[i].name == "lazer tower")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "canon":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow") || (towers[i].name == "mortar")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                        case "mortar":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;
                        case "x-bow":
                            j = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                if ((towers[i].cost <= GameManagerPartie.instance.enemyCoins) && ((towers[i].name == "mirror tower") || (towers[i].name == "x-bow")))
                                {
                                    TowersWeCanBuild[j] = towers[i];
                                    j++;
                                }
                            }
                            if (j > 0)
                            {
                                int random = Random.Range(0, j);

                                positionManager.add(towers[(byte)random], buildPos, GameManagerPartie.instance.enemylvl);
                            }
                            changeState(AIState.idle);
                            break;

                    }
                }
                break;


        }
       
    }


}