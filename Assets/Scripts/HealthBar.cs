using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Health bar for the boss
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update

    public void SetHealth(int health) {
        slider.value = health;
    }
}
