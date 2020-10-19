using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        //set healthbar to values
        slider.maxValue = health;
        slider.value = health;

        //set color to when gradient is at 100% value
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        //update health
        slider.value = health;

        //update color
        //fill.color = gradient.Evaluate(slider.normalizedValue);

        //make healthbar flash
        StartCoroutine(Flash());
    }

    //make coroutine numerator to flash with a small delay
    IEnumerator Flash()
    {
        fill.color = Color.white;
        yield return new WaitForSecondsRealtime(.1f);

        fill.color = gradient.Evaluate(slider.normalizedValue);
        yield return new WaitForSecondsRealtime(.25f);

        fill.color = Color.white;
        yield return new WaitForSecondsRealtime(.1f);

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
