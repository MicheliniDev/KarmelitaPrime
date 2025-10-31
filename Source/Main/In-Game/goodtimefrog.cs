using System;
using GlobalEnums;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.Events;

namespace KarmelitaPrime;

public class goodtimefrog : MonoBehaviour
{
    private bool canChasePlayer;

    private bool playerInRange => Mathf.Abs(HeroController.instance.transform.position.x - transform.position.x) < 10f;

    private GameObject damagerChild;
    private void Awake()
    {
        canChasePlayer = false;
        
        damagerChild = new GameObject("Damager", typeof(BoxCollider2D), typeof(DamageHero));
        damagerChild.transform.SetParent(transform);
        damagerChild.layer =  LayerMask.NameToLayer("Enemy Attack");
        var colliderDamager = GetComponentInChildren<BoxCollider2D>();
        colliderDamager.isTrigger = true;
        colliderDamager.size *= 1.5f;
        
        var damageHero = GetComponentInChildren<DamageHero>();
        damageHero.damageDealt = 10;
        damageHero.damagePropertyFlags = DamagePropertyFlags.None;
        damageHero.AlwaysSendDamaged = false;
        damageHero.HeroDamagedFSMBool = "";
        damageHero.HeroDamagedFSMEvent = "";
        damageHero.HeroDamagedFSMGameObject = "";
        damageHero.OnDamagedHero = new UnityEvent(); 
        
        damagerChild.SetActive(false);
    }

    public void Update()
    {
        if (canChasePlayer)
            transform.position = Vector2.MoveTowards(transform.position,
                HeroController.instance.transform.position, 5f * Time.deltaTime);
    }
    
    public void TrySetCanChasePlayer()
    {
        KarmelitaPrimeMain.Instance.Log(playerInRange);
        if (!playerInRange) return;
        canChasePlayer = true;
        damagerChild.SetActive(true);
    }
}