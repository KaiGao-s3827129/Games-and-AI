using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public GameObject skillUI;
    public GameObject neo;
    private NeoState _neoState;

    // Update is called once per frame
    void Update()
    {
        neo = GameObject.Find("Neo");
        _neoState = neo.GetComponent<NeoState>();
        if (_neoState.currentJumpState == JumpState.SingleJump)
        {
            skillUI.GetComponent<CanvasGroup>().alpha = 0;
        }
        else if (_neoState.currentJumpState == JumpState.DoubleJump)
        {
            skillUI.GetComponent<CanvasGroup>().alpha = 1;
        }
        
    }
}
