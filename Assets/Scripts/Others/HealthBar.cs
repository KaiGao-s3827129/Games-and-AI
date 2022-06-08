using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Health bar for the boss
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject boss;
    // Start is called before the first frame update

    void Start()
    {   
        //Get the all Leader Minions.
        boss = GameObject.Find("TheBoss");
    }
    public void SetHealth(int health) {
        slider.value = health;
    }
    void Update(){
        transform.position = new Vector2(boss.transform.position.x,boss.transform.position.y+7);
    }
}
