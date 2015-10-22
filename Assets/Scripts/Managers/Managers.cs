using UnityEngine;
using System.Collections;
using Magicolo;

public class Managers : MonoBehaviourExtended
{
	public AudioManager AudioManagerPrefab;
	public Kronos KronosPrefab;
	public Mars MarsPrefab;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (AudioManager.Instance == null)
			Instantiate(AudioManagerPrefab).transform.parent = transform;

		if (Kronos.Instance == null)
			Instantiate(KronosPrefab).transform.parent = transform;

		if (Mars.Instance == null)
			Instantiate(MarsPrefab).transform.parent = transform;
	}
}
