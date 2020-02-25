﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortarShooting : MonoBehaviour
{
    public GameObject fireBall;
    private GameObject target_;
    public float speed;
    public short damage = 200;
    public float fireRate = 4f;
    private float nextTimeFire = 0f;
    public Transform firePoint;
    public TowerScript tower;

    void SetDamage()
    {
        damage = tower.Get_damage();
        this.GetComponent<target>().Sethealth(tower.Get_health());
    }
    private void Start()
    {
        SetDamage();
    }

    private mortorState CurrentState = mortorState.idle;

    public enum mortorState
    {
        idle, shoot
        //, die
    }

    public void shoot(GameObject targetGameObject)
    {
        CurrentState = mortorState.shoot;
        target_ = targetGameObject;
    }
    public void stopShoot()
    {
        CurrentState = mortorState.idle;
        target_ = null;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case mortorState.idle:
                break;

            case mortorState.shoot:
                if (Time.time > nextTimeFire)
                {
                    Vector3 vo = Calculatevelocity(target_.transform.position, firePoint.position, speed);
                    shoot(vo);
                    nextTimeFire = Time.time + fireRate;
                }
                break;
        }
        

    }

    void shoot(Vector3 vo)
    {
        /*Vector3 FireballPos = new Vector3(target.transform.position.x, target.transform.position.y + 3, target.transform.position.z);
        var heading = FireballPos - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        speed = (distance - transform.position.y + 10);*/


        GameObject go = Instantiate(fireBall, firePoint.position,firePoint.rotation);
        go.GetComponent<Rigidbody>().velocity = vo;
        go.GetComponent<bullet>().changedam(damage);

        //go.GetComponent<MortarFireBall>().Set(transform.position, direction, speed);

    }
    Vector3 Calculatevelocity(Vector3 target,Vector3 origin,float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float sXZ = distanceXZ.magnitude;

        float Vxz = sXZ / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;

    }
}
