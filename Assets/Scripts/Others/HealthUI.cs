using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject health1, health2, health3, neo;
    private NeoState _neoState;
    // Start is called before the first frame update
    void Start()
    {
        health1.GetComponent<CanvasGroup>().alpha = 1;
        health2.GetComponent<CanvasGroup>().alpha = 1;
        health3.GetComponent<CanvasGroup>().alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Neo") == null)
        {
            return;
        }
        neo = GameObject.Find("Neo");
        _neoState = neo.GetComponent<NeoState>();
        int currentHealth = _neoState.healthPoint;

        switch (currentHealth)
        {
            case 3:
                health1.GetComponent<CanvasGroup>().alpha = 1;
                health2.GetComponent<CanvasGroup>().alpha = 1;
                health3.GetComponent<CanvasGroup>().alpha = 1;
                break;
            case 2:
                health1.GetComponent<CanvasGroup>().alpha = 1;
                health2.GetComponent<CanvasGroup>().alpha = 1;
                health3.GetComponent<CanvasGroup>().alpha = 0;
                break;
            case 1:
                health1.GetComponent<CanvasGroup>().alpha = 1;
                health2.GetComponent<CanvasGroup>().alpha = 0;
                health3.GetComponent<CanvasGroup>().alpha = 0;
                break;
            case 0:
                health1.GetComponent<CanvasGroup>().alpha = 0;
                health2.GetComponent<CanvasGroup>().alpha = 0;
                health3.GetComponent<CanvasGroup>().alpha = 0;
                break;
        }
    }
}
