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
				Civile civile = BehaviourPool<Civile>.Create(Hermes.Instance.CivilePrefabs.GetRandom());
				Doorway doorway = doorways.GetRandom();
				civile.transform.position = doorway.transform.position;
				if (doorway.ExitDirection.x < 0)
				{
					civile.transform.SetEulerAngles(180, Axes.Y);
				}
				else
				{
					civile.transform.SetEulerAngles(0, Axes.Y);
				}
			}
		}
	}
}