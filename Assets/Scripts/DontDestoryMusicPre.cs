using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryMusicPre : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
