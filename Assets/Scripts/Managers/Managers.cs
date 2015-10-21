using UnityEngine;
using System.Collections;
using Magicolo;

public class Managers : MonoBehaviourExtended
{
	public AudioManager AudioManagerPrefab;
	public Kronos KronosPrefab;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (AudioManager.Instance == null)
			Instantiate(AudioManagerPrefab).transform.parent = transform;

		if (Kronos.Instance == null)
			Instantiate(KronosPrefab).transform.parent = transform;
	}
}
