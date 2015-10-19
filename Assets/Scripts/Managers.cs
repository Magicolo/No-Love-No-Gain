using UnityEngine;
using System.Collections;
using Magicolo;

public class Managers : MonoBehaviourExtended
{
	public AudioManager AudioManagerPrefab;
	public TimeManager TimeManagerPrefab;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (AudioManager.Instance == null)
			Instantiate(AudioManagerPrefab).transform.parent = transform;

		if (TimeManager.Instance == null)
			Instantiate(TimeManagerPrefab).transform.parent = transform;
	}
}
