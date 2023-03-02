using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    public Slider m_slider;

    public void SetMaxHealth(int health)
    {
        m_slider.maxValue = health;
        m_slider.value = health;
    }
    public void SetHealth(int health)
    {
        m_slider.value = health;
    }
}
