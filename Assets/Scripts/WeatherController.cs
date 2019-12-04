using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[Serializable]
public class Weather
{
	public int id;
	public string main;
}

[Serializable]
public class WeatherInfo
{
	public int id;
	public string name;
	public List<Weather> weather;
}

[Serializable]
public class Location
{
    public string country;
    public string region;
    public string city;
}

[Serializable]
public class IPLocationInfo
{
    public int id;
    public string name;
    public Location location;
}

public class WeatherController : MonoBehaviour
{

	private const string API_KEY = "5690ad6d963e3cf4ab163080864a9a84";
	private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes
	private float m_ApiCheckCountdown = API_CHECK_MAXTIME;
    private bool m_CanCheck = false;
    private GameObject m_ActiveWeatherSystem;

	public string m_LondonCityId;
    public string m_LosAngelesCityId;
    public GameObject m_SnowSystem;
    public GameObject m_RainSystem;
    public GameObject m_CloudSystem;
    public GameObject m_ClearSkySystem;
    public TMP_Text m_WeatherLabel;
    public TMP_Text m_LocationLabel;

    // Start is called before the first frame update
    void Start()
    {
        //m_CanCheck = true;
        StartCoroutine(GetWeatherOfPlayer(CheckLocation));
    }

    // Update is called once per frame
    void Update()
    {
		//m_ApiCheckCountdown -= Time.deltaTime;
		//if (m_ApiCheckCountdown <= 0)
		//{
            //m_ApiCheckCountdown = API_CHECK_MAXTIME;
            //m_CanCheck = true;
		//}
	}

    public void OnMyLocationClick()
    {
       
    }

    public void OnLondonClick()
    {
        UpdateLocationLabel("London");
        StartCoroutine(GetWeatherByID(CheckWeatherStatus, m_LondonCityId));

    }

    public void OnLosAngelesClick()
    {
        UpdateLocationLabel("Los Angeles");
        StartCoroutine(GetWeatherByID(CheckWeatherStatus, m_LosAngelesCityId));
    }

    public void CheckLocation(IPLocationInfo locationInfo)
    {
        string city = locationInfo.location.city;

        Debug.Log($"Location: {city}");
    }

	public void CheckWeatherStatus(WeatherInfo weatherObj)
	{
        bool drizzleRain = weatherObj.weather[0].main.Equals("Drizzle");
        drizzleRain = weatherObj.weather[0].main.Equals("Rain");
        bool snow = weatherObj.weather[0].main.Equals("Snow");
        bool clear = weatherObj.weather[0].main.Equals("Clear");
        bool clouds = weatherObj.weather[0].main.Equals("Clouds");

        print("MAIN : " + weatherObj.weather[0].main);
        if (clear)
        {
            if (m_ActiveWeatherSystem != null && m_ActiveWeatherSystem.name != m_ClearSkySystem.name) 
            {
                m_ActiveWeatherSystem.SetActive(false);
            }

            m_ClearSkySystem.SetActive(true);
            m_WeatherLabel.text = string.Format("Weather: {0}", "Clear");
            m_ActiveWeatherSystem = m_ClearSkySystem;
        }
        else if (clouds)
        {
            if (m_ActiveWeatherSystem != null && m_ActiveWeatherSystem.name != m_CloudSystem.name)
            {
                m_ActiveWeatherSystem.SetActive(false);
            }

            m_CloudSystem.SetActive(true);
            m_WeatherLabel.text = string.Format("Weather: {0}", "Clouds");
            m_ActiveWeatherSystem = m_CloudSystem;
        }
        else if (drizzleRain)
        {
            if (m_ActiveWeatherSystem != null && m_ActiveWeatherSystem.name != m_RainSystem.name)
            {
                m_ActiveWeatherSystem.SetActive(false);
            }

            m_RainSystem.SetActive(true);
            m_WeatherLabel.text = string.Format("Weather: {0}", "Rain");
            m_ActiveWeatherSystem = m_RainSystem;
        }
        else if (snow)
        {
            if (m_ActiveWeatherSystem != null && m_ActiveWeatherSystem.name != m_SnowSystem.name)
            {
                m_ActiveWeatherSystem.SetActive(false);
            }

            m_SnowSystem.SetActive(true);
            m_WeatherLabel.text = string.Format("Weather: {0}", "Snow");
            m_ActiveWeatherSystem = m_SnowSystem;
        }
            
	}

    private void UpdateLocationLabel(string location)
    {
        m_LocationLabel.text = string.Format("Location: {0}", location);
    }

    IEnumerator GetWeatherOfPlayer(Action<IPLocationInfo> onSuccess)
    {
        //var ip = NetworkManager.singleton.networkAddress;
        string ipv6 = IPManager.GetIP(ADDRESSFAM.IPv6);

        using (UnityWebRequest req = UnityWebRequest.Get(string.Format("https://geo.ipify.org/api/v1?apiKey=at_PsOE0ZNe1iwY5izVDMaUXPNT1rkVQ&ipAddress=", ipv6)))
        {
            yield return req.SendWebRequest();
            while (!req.isDone)
            {
                yield return null;
            }
            byte[] result = req.downloadHandler.data;
            string locationJSON = System.Text.Encoding.Default.GetString(result);
            Debug.Log(locationJSON);
            IPLocationInfo info = JsonUtility.FromJson<IPLocationInfo>(locationJSON);
            onSuccess(info);
        }
    }

    IEnumerator GetWeatherByID(Action<WeatherInfo> onSuccess, string cityID)
	{
		using (UnityWebRequest req = UnityWebRequest.Get(string.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", cityID, API_KEY)))
		{
			yield return req.SendWebRequest();
			while (!req.isDone)
            {
                yield return null;
            }
			byte[] result = req.downloadHandler.data;
			string weatherJSON = System.Text.Encoding.Default.GetString(result);
            Debug.Log(weatherJSON);
            WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
			onSuccess(info);
		}
	}
}
