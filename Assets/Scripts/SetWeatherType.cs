using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeatherType : MonoBehaviour
{
    public enum WeatherType
    {
        Night, Day, Rain, Sunny, Leaves, DayNightCycle, FullWeather
    }
    public WeatherType weatherType;
    public WeatherSystem weatherSystem;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MainCharacter"))
        {
            if (weatherType.Equals(WeatherType.Day))
            {
                weatherSystem.DayOrNight();
            }
            else if (weatherType.Equals(WeatherType.Night))
            {
                weatherSystem.DayOrNight();
            }
            else if (weatherType.Equals(WeatherType.Rain))
            {
                weatherSystem.Rain();
            }
            else if (weatherType.Equals(WeatherType.Sunny))
            {
                weatherSystem.Sunny();
            }
            else if (weatherType.Equals(WeatherType.Leaves))
            {
                weatherSystem.Leaves();
            }
            else if (weatherType.Equals(WeatherType.DayNightCycle))
            {
                weatherSystem.DayNightCycle();
            }
            else if (weatherType.Equals(WeatherType.FullWeather))
            {
                weatherSystem.FullWeather();
            }
        }
    }
    
}
