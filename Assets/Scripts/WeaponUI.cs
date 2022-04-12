using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public GameObject weaponUI;
    public GameObject neo;
    private NeoState _neoState;
    public bool get;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Neo") == null)
        {
            return;
        }
        neo = GameObject.Find("Neo");
        _neoState = neo.GetComponent<NeoState>();
        if (_neoState.currentAttackState == AttackState.Melee)
        {
            weaponUI.GetComponent<CanvasGroup>().alpha = 0;
            get = false;
        }
        else if (_neoState.currentAttackState == AttackState.Remote)
        {
            get = true;
            weaponUI.GetComponent<CanvasGroup>().alpha = 1;
        }
        
    }

    private void FixedUpdate()
    {
        
    }
}
