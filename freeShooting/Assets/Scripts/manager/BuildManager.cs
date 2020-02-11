﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    RaycastHit hit;
    GameManagerPartie gm;
    private TowerScript[] towers= new TowerScript[6];
    
    bool  test = false;
    private int nb;
    
    // Start is called before the first frame update
    void Start()
    {
        towers = this.GetComponent<GameManager>().GetSelectedTowers();
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0) && test == true)
        {
            testBuilding();
        }
    }

    public void click (int nb)
    {
        test = true;
        
        this.nb = nb;
    }
    public void testBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "TowerDefendZone")
            {
                Vector3 towerpos = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y +2.25f, hit.collider.transform.position.z);
               
                GameObject go = Instantiate(towers[nb].prefab, towerpos, Quaternion.Euler(0,0,0));
               
               
            }
            
                test = false;
        }
    }
    
}