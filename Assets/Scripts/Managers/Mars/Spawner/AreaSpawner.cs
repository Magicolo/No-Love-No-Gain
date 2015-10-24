using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class AreaSpawner : BaseSpawner
{

	[Disable]
	public float Timer;

	public float CoolDownMin;
	public float CoolDownMax;

	public float SpawnMin;
	public float SpawnMax;
	public GameObject prefabToSpawn;

	RectZone rectZone;

	void Start()
	{
		Timer = Random.Range(CoolDownMin, CoolDownMax);
		rectZone = GetComponent<RectZone>();
	}

	public override void Update()
	{
		Timer -= Kronos.Enemy.DeltaTime;
		if (Timer <= 0)
			Spawn();

	}

	private void Spawn()
	{
		Timer = Random.Range(CoolDownMin, CoolDownMax);
		for (int i = 0; i < Random.Range(SpawnMax, SpawnMax); i++)
		{
			GameObjectFactory.createClone(prefabToSpawn, this.transform, rectZone.WorldRect.GetRandomPoint());
		}
	}
}