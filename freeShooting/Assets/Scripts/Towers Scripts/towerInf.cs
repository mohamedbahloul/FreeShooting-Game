﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerInf : MonoBehaviour
{
    public short damage;
    public float fireRate;
    // Start is called before the first frame update
    public void SetDamage(short d)
    {
        damage = d;
    }

    public void SetFireRate(float f)
    {
        fireRate = f;
    }

    public void SetHealth(short h)
    {
        this.GetComponent<target>().Sethealth(h);
    }
}
