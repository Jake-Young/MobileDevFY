using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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

public class WeatherController : MonoBehaviour
{

	private const string API_KEY = "5690ad6d963e3cf4ab163080864a9a84";
	private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes
	private float m_ApiCheckCountdown = API_CHECK_MAXTIME;

	public string m_CityId;
	public GameObject m_SnowSystem;
    public GameObject m_RainSystem;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(GetWeather(CheckSnowStatus));
    }

    // Update is called once per frame
    void Update()
    {
		m_ApiCheckCountdown -= Time.deltaTime;
		if (m_ApiCheckCountdown <= 0)
		{
			m_ApiCheckCountdown = API_CHECK_MAXTIME;
			StartCoroutine(GetWeather(CheckSnowStatus));
		}
	}

	public void CheckSnowStatus(WeatherInfo weatherObj)
	{
		bool snowing = weatherObj.weather[0].main.Equals("Clouds");
		print("MAIN : " + weatherObj.weather[0].main);
		if (snowing)
			m_SnowSystem.SetActive(true);
		else
			m_SnowSystem.SetActive(false);
	}

	IEnumerator GetWeather(Action<WeatherInfo> onSuccess)
	{
		using (UnityWebRequest req = UnityWebRequest.Get(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", m_CityId, API_KEY)))
		{
			yield return req.SendWebRequest();
			while (!req.isDone)
				yield return null;
			byte[] result = req.downloadHandler.data;
			string weatherJSON = System.Text.Encoding.Default.GetString(result);
			WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
			onSuccess(info);
		}
	}
}
