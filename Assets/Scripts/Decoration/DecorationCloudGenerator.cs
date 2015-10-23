using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

public class DecorationCloudGenerator : MonoBehaviourExtended
{
	public DecorationCloud[] CloudPrefabs;
	[Min]
	public float MinGenerationTime = 1f;
	[Min]
	public float MaxGenerationTime = 5f;
	public float KillPosition;

	List<DecorationCloud> _clouds = new List<DecorationCloud>();
	float _generationCounter;

	void Update()
	{
		_generationCounter -= Kronos.World.DeltaTime;

		if (_generationCounter <= 0f)
		{
			DecorationCloud cloud = BehaviourPool<DecorationCloud>.Create(CloudPrefabs.GetRandom());
			cloud.transform.parent = transform;
			cloud.transform.SetLocalPosition(0f, Axes.X);
			cloud.transform.SetLocalPosition(UnityEngine.Random.Range(25f, 75f), Axes.Z);
			_clouds.Add(cloud);
			_generationCounter = UnityEngine.Random.Range(MinGenerationTime, MaxGenerationTime);
		}

		for (int i = 0; i < _clouds.Count; i++)
		{
			DecorationCloud cloud = _clouds[i];

			if (cloud.transform.localPosition.x <= KillPosition)
			{
				BehaviourPool<DecorationCloud>.Recycle(cloud);
				_clouds.RemoveAt(i--);
			}
		}
	}
}
