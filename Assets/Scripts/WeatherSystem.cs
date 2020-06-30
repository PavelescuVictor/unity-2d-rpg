using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class WeatherSystem : MonoBehaviour
{

    public GameObject[] DayLights;
    public GameObject[] NightLights;
    public GameObject[] SunnyParticles;
    public GameObject[] RainParticles;
    public GameObject[] LeavesParticles;
    public GameObject[] ExtraNightLights;

    [SerializeField] private bool dayActive;
    [SerializeField] private bool nightActive;
    [SerializeField] private bool rainActive;
    [SerializeField] private bool sunnyActive;
    [SerializeField] private bool leavesActive;
    [SerializeField] private bool dayNightCycleActive;
    [SerializeField] private bool fullWeatherActive;
    [SerializeField] private bool lightChanging;
    [SerializeField] private bool extraNightLightsActive;

    public float lightStep;
    public float waitTimeLightChange;
    public float dayTime;
    public float nightTime;

    [SerializeField] private float dayTimeCounter;
    [SerializeField] private float nightTimeCounter;

    private float[] dayLightMinIntensity;
    private float[] dayLightMaxIntensity;
    private float[] nightLightMinIntensity;
    private float[] nightLightMaxIntensity;
    public int minLightIntensityPercentage;
    
    [SerializeField] private bool changeDayToNight;
    [SerializeField] private bool changeNightToDay;
    [SerializeField] private bool nightLightsSetToMin;
    [SerializeField] private bool dayLightsSetToMin;

    // Start is called before the first frame update
    void Start()
    {
        lightChanging = false;
        changeDayToNight = false;
        changeNightToDay = false;
        nightLightsSetToMin = false;
        dayLightsSetToMin = false;
        dayTimeCounter = dayTime;
        nightTimeCounter = nightTime;
        setDayAndNightLightMinMaxIntensity();
        /*
        foreach (float index in dayLightMinIntensity)
        {    
            Debug.Log("DaylightMin -- " + index);
        }
        foreach (float index in dayLightMaxIntensity)
        {    
            Debug.Log("DaylightMax -- " + index);
        }
        foreach (float index in nightLightMinIntensity)
        {    
            Debug.Log("NightlightMin -- " + index);
        }
        foreach (float index in nightLightMaxIntensity)
        {    
            Debug.Log("NightlightMax -- " + index);
        }
        */
    }

    private void setDayAndNightLightMinMaxIntensity()
    {
        dayLightMaxIntensity = new float[DayLights.Length];
        dayLightMinIntensity = new float[DayLights.Length];
        nightLightMaxIntensity = new float[NightLights.Length];
        nightLightMinIntensity = new float[NightLights.Length];

        int i = 0;
        int j = 0;
        foreach (GameObject dayLight in DayLights)
        {
            Light2D light = dayLight.GetComponent<Light2D>();
            dayLightMaxIntensity[i] = light.intensity;
            dayLightMinIntensity[i] = minLightIntensityPercentage * light.intensity / 100;
            i = i + 1;
        }
        foreach (GameObject nightLight in NightLights)
        {
            Light2D light = nightLight.GetComponent<Light2D>();
            nightLightMaxIntensity[j] = light.intensity;
            nightLightMinIntensity[j] = minLightIntensityPercentage * light.intensity / 100;
            j = j + 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        CheckForActiveWeatherTypes();
        if (dayNightCycleActive == true)
        {
            if (lightDecreasing == false && lightIncreasing == false)
            {
                int dayLightIndex = 0;
                int nightLightIndex = 0;
                foreach (GameObject dayLight in DayLights)
                {
                    
                    Light2D dLight = dayLight.GetComponent<Light2D>();
                    StartCoroutine(DecreaseLight(dLight, dayLightIndex, dayLight));
                    dayLightIndex = dayLightIndex + 1;
                }

                foreach (GameObject nightLight in NightLights)
                {
                    Light2D nLight = nightLight.GetComponent<Light2D>();
                    nLight.intensity = 0;
                    nightLight.SetActive(true);
                    StartCoroutine(IncreaseLight(nLight, nightLightIndex));
                    nightLightIndex = nightLightIndex + 1;
                }
            }
        }
        */
        
        CheckForActiveWeatherTypes();
        if (dayNightCycleActive == true)
        {
            /*
            if (lightChanging == false)
            {
                int dayLightIndex = 0;
                int nightLightIndex = 0;
                foreach (GameObject nightLight in NightLights)
                {
                    Light2D nLight = nightLight.GetComponent<Light2D>();
                    nLight.intensity = nightLightMinIntensity[nightLightIndex];
                    nightLight.SetActive(true);
                    nightLightIndex = nightLightIndex + 1;
                }

                nightLightIndex = 0;
                StartCoroutine(ChangeLight(dayLightIndex, nightLightIndex));
            }
            */
            if (dayActive == true && nightActive == false)
            {
                if (dayTimeCounter < 0)
                {
                    changeDayToNight = true;
                    changeNightToDay = false;
                    nightLightsSetToMin = false;
                    dayTimeCounter = dayTime;
                    if (lightChanging == false)
                    {
                        dayTimeCounter = dayTime;
                    }
                }
                else
                {
                    dayTimeCounter -= Time.deltaTime;
                }
            }
            else if (nightActive == true && dayActive == false)
            {
                if (nightTimeCounter < 0)
                {
                    changeNightToDay = true;
                    changeDayToNight = false;
                    dayLightsSetToMin = false;
                    if (lightChanging == false)
                    {
                        nightTimeCounter = nightTime;
                    }
                }
                else
                {
                    nightTimeCounter -= Time.deltaTime;
                }
            }

            if (lightChanging == false)
            {
                if (changeDayToNight == true)
                {
                    int dayLightIndex = 0;
                    int nightLightIndex = 0;
                    if (nightLightsSetToMin == false)
                    {
                        foreach (GameObject nightLight in NightLights)
                        {
                            Light2D nLight = nightLight.GetComponent<Light2D>();
                            nLight.intensity = nightLightMinIntensity[nightLightIndex];
                            nightLight.SetActive(true);
                            nightLightIndex = nightLightIndex + 1;
                        }

                        nightLightIndex = 0;
                        nightLightsSetToMin = true;
                    }
                    
                    StartCoroutine(ChangeDayToNight(dayLightIndex, nightLightIndex));
                }
                else if (changeNightToDay == true)
                {
                    int dayLightIndex = 0;
                    int nightLightIndex = 0;
                    if (dayLightsSetToMin == false)
                    {
                        foreach (GameObject dayLight in DayLights)
                        {
                            Light2D dLight = dayLight.GetComponent<Light2D>();
                            dLight.intensity = dayLightMinIntensity[dayLightIndex];
                            dayLight.SetActive(true);
                            dayLightIndex = dayLightIndex + 1;
                        }

                        dayLightIndex = 0;
                        dayLightsSetToMin = true;
                    }
                    
                    StartCoroutine(ChangeNightToDay(dayLightIndex, nightLightIndex));
                }
            }
        }
        
        if (fullWeatherActive == true)
        {
            
        }
    }

    private IEnumerator ChangeDayToNight(int dindex, int nindex)
    {
        lightChanging = true;
        Debug.Log("Changing day to night.");
        int stopTokenDLightCount = 0;
        int stopTokenNLightCount = 0;
        bool stopTokenDLight = false;
        bool stopTokenNLight = false;
        
        foreach (GameObject dayLight in DayLights)
        {
            Light2D dLight = dayLight.GetComponent<Light2D>();
            if (dLight.intensity > dayLightMinIntensity[dindex])
            {
                dLight.intensity = dLight.intensity - lightStep;
            }
            else
            {
                dayLight.SetActive(false);
                stopTokenDLightCount = stopTokenDLightCount + 1;
            }
            dindex = dindex + 1;
        }

        if (stopTokenDLightCount ==  DayLights.Length)
        {
            stopTokenDLight = true;
        }

        foreach (GameObject nightLight in NightLights)
        {
            Light2D nLight = nightLight.GetComponent<Light2D>();
            if (nLight.intensity < nightLightMaxIntensity[nindex])
            {
                nLight.intensity = nLight.intensity + lightStep;
            }
            else
            {
                stopTokenNLightCount = stopTokenNLightCount + 1;
            }
            nindex = nindex + 1;
        }
        if (stopTokenNLightCount == NightLights.Length)
        {
            stopTokenNLight = true;
        }
        if (stopTokenDLight == true && stopTokenNLight == true)
        {
            lightChanging = false;
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(waitTimeLightChange);
            lightChanging = false;
        }
    }
    
    private IEnumerator ChangeNightToDay(int dindex, int nindex)
    {
        lightChanging = true;
        Debug.Log("Changing Night to Day.");
        lightChanging = true;
        int stopTokenDLightCount = 0;
        int stopTokenNLightCount = 0;
        bool stopTokenDLight = false;
        bool stopTokenNLight = false;
        
        foreach (GameObject dayLight in DayLights)
        {
            Light2D dLight = dayLight.GetComponent<Light2D>();
            if (dLight.intensity < dayLightMaxIntensity[dindex])
            {
                dLight.intensity = dLight.intensity + lightStep;
            }
            else
            {
                stopTokenDLightCount = stopTokenDLightCount + 1;
            }
            dindex = dindex + 1;
        }
        
        if (stopTokenDLightCount == DayLights.Length)
        {
            stopTokenDLight = true;
        }

        foreach (GameObject nightLight in NightLights)
        {
            Light2D nLight = nightLight.GetComponent<Light2D>();
            if (nLight.intensity > nightLightMinIntensity[nindex])
            {
                nLight.intensity = nLight.intensity - lightStep;
            }
            else
            {
                nightLight.SetActive(false);
                stopTokenNLightCount = stopTokenNLightCount + 1;
            }
            nindex = nindex + 1;
        }
        if (stopTokenNLightCount == NightLights.Length)
        {
            stopTokenNLight = true;
        }
        if (stopTokenDLight == true && stopTokenNLight == true)
        {
            lightChanging = false;
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(waitTimeLightChange);
            lightChanging = false;
        }
    }

    /*
    private IEnumerator IncreaseLight(Light2D light, int index)
    {
        lightIncreasing = true;
        light.intensity = light.intensity + lightStep;
        if (light.intensity > nightLightMaxIntensity[index])
        {
            nightActive = true;
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(waitTimeLightChange);
        }
        lightIncreasing = false;
    }
    private IEnumerator DecreaseLight(Light2D light, int index, GameObject dayLight)
    {
        lightDecreasing = true;
        light.intensity = light.intensity - lightStep;
        if (light.intensity < dayLightMinIntensity[index])
        {
            dayLight.SetActive(false);
            dayActive = false;
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(waitTimeLightChange);
        }
        lightDecreasing = false;
    }
    */
    
    private void CheckForActiveWeatherTypes()
    {
        int activeDayLightsCounter = 0;
        foreach (GameObject dayLight in DayLights)
        {
            if (dayLight.activeSelf == true)
            {
                activeDayLightsCounter += 1;
            }
        }
        
        if (activeDayLightsCounter == DayLights.Length)
        {
            dayActive = true;
        }
        else
        {
            dayActive = false;
        }

        int activeNightLightsCounter = 0;
        foreach (GameObject nightLight in NightLights)
        {
            if (nightLight.activeSelf == true)
            {
                activeNightLightsCounter += 1;
            }
        }

        if (activeNightLightsCounter == NightLights.Length)
        {
            nightActive = true;
        }
        else
        {
            nightActive = false;
        }

        foreach (GameObject rainParticle in RainParticles)
        {
            if (rainParticle.activeSelf == true)
            {
                rainActive = true;
            }
            else
            {
                rainActive = false;
            }
        }

        foreach (GameObject sunnyParticle in SunnyParticles)
        {
            if (sunnyParticle.activeSelf == true)
            {
                sunnyActive = true;
            }
            else
            {
                sunnyActive = false;
            }
        }

    }

    public void DayOrNight()
    {
        if (dayActive == true)
        {
            foreach (GameObject dayLight in DayLights)
            {
                dayLight.SetActive(false);
                dayActive = false;
            }
            foreach (GameObject nightLight in NightLights)
            {
                nightLight.SetActive(true);
                nightActive = true;
            }
            ActivateExtraNightLights();
        }
        else
        {
            foreach (GameObject dayLight in DayLights)
            {
                dayLight.SetActive(true);
                dayActive = true;
            }
            foreach (GameObject nightLight in NightLights)
            {
                nightLight.SetActive(false);
                nightActive = false;
            }
            ActivateExtraNightLights();
        }
        
    }
    public void Rain()
    {
        if (rainActive == true)
        {
            foreach (GameObject rainParticle in RainParticles)
            {
                rainParticle.SetActive(false);
                rainActive = false;
            }
        }
        else
        {
            foreach (GameObject rainParticle in RainParticles)
            {
                rainParticle.SetActive(true);
                rainActive = true;
            }
        }
    }
    public void Sunny()
    {
        if (sunnyActive == true)
        {
            foreach (GameObject sunnyParticle in SunnyParticles)
            {
                sunnyParticle.SetActive(false);
                sunnyActive = false;
            }
        }
        else
        {
            foreach (GameObject sunnyParticle in SunnyParticles)
            {
                sunnyParticle.SetActive(true);
                sunnyActive = true;
            }
        }
    }

    public void Leaves()
    {
        if (leavesActive == true)
        {
            foreach (GameObject leavesParticle in LeavesParticles)
            {
                leavesParticle.SetActive(false);
                leavesActive = false;
            }
        }
        else
        {
            foreach (GameObject leavesParticle in LeavesParticles)
            {
                leavesParticle.SetActive(true);
                leavesActive = true;
            }
        }
    }
    public void DayNightCycle()
    {
        if (dayNightCycleActive == true)
        {
            dayNightCycleActive = false;
        }
        else
        {
            dayNightCycleActive = true;
        }
    }
    public void FullWeather()
    {
        fullWeatherActive = true;
    }

    public void ActivateExtraNightLights()
    {
        if (nightActive == true)
        {
            foreach (GameObject extraNightLight in ExtraNightLights)
            {
                extraNightLight.SetActive(true);
                extraNightLightsActive = true;
            }
        }
        else
        {
            foreach (GameObject extraNightLight in ExtraNightLights)
            {
                extraNightLight.SetActive(false);
                extraNightLightsActive = false;
            }
        }
    }
}
