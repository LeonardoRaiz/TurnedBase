using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{


    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform ragdollPrefab1;
    [SerializeField] private Transform originalRootBone;


    private HealthSystem healthSystem;
    private Unit unit;
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        unit = GetComponent<Unit>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        if (unit.IsPlayer1())
        {
            Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
            unitRagdoll.Setup(originalRootBone);
        } else {
            Transform ragdollTransform = Instantiate(ragdollPrefab1, transform.position, transform.rotation);
            UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
            unitRagdoll.Setup(originalRootBone);
        }

    }


}
