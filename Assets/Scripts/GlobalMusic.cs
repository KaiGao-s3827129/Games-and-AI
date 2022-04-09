using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMusic : MonoBehaviour
{
    public GameObject globalMusic;

    GameObject myMusic;
    // Start is called before the first frame update
    void Start()
    {
        myMusic = GameObject.FindGameObjectWithTag("GlobalUIMusic");
        if (myMusic == null)
        {
            myMusic = (GameObject)Instantiate(globalMusic);
        }
    }
}
