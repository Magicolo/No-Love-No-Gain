using UnityEngine;
using System.Collections.Generic;
using Magicolo;
using Rick;

public class Building : MonoBehaviour
{

	public int CurrentCivilesCount = 0;
	[Disable]
	public float GetOutTimer = 0;

	public float GetOutTimeMin;
	public float GetOutTimeMax;

	public List<Civile> CivilePrefab;
	public Transform CivilParent;

	Doorway[] doorways;

	void Start()
	{
		doorways = GetComponentsInChildren<Doorway>();
		GetOutTimer = Random.Range(GetOutTimeMin, GetOutTimeMax);
	}


	void Update()
	{
		GetOutTimer -= Kronos.World.DeltaTime;
		if (GetOutTimer <= 0)
		{
			GetOutTimer = Random.Range(GetOutTimeMin, GetOutTimeMax);
			if (CurrentCivilesCount > 0)
			{
				CurrentCivilesCount--;
				BehaviourPool<Civile>.Create(CivilePrefab.GetRandom());
			}
		}
	}

	private Vector3 GetRandomDoorLocation()
	{
		Doorway doorway = doorways.GetRandom();
		return doorway.transform.position;
	}
}