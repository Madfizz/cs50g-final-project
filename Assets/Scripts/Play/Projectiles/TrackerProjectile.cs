﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerProjectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public float destroyAfter;
    public Player player;

    private void OnEnable()
    {
        Invoke("Destroy", destroyAfter);
    }

    private void Update()
    {
        // Find direction player is in and pursue them
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
