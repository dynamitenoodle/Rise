using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack 1", menuName = "Custom/Attack", order = 1)]
public class Attack : ScriptableObject
{
    public float attackRange = 1f;
    public float attackTimerMax = 1.5f;
    public float attackDelay = .5f;
    public float attackSpacing = 1.5f;
    public GameObject attackPrefab;
    public float kickBack = 2.5f;

    // ranged attack info
    public bool isMelee = true;
    public float bulletSpeed = .2f;
}

