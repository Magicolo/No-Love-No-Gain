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

	Zone2DBase _zone;

	void Start()
	{
		Timer = Random.Range(CoolDownMin, CoolDownMax);
		_zone = GetComponent<Zone2DBase>();
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
			GameObjectFactory.createClone(prefabToSpawn, this.transform, _zone.GetRandomWorldPoint());
		}
	}
}