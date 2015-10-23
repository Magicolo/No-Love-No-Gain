using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class Civile : MonoBehaviour, IPoolable, ICopyable<Civile>
{

	public float MovementSpeed;
	public float MovementSpeedMin;
	public float MovementSpeedMax;

	void Start()
	{

	}

	void ICopyable<Civile>.Copy(Civile reference)
	{
		MovementSpeedMin = reference.MovementSpeedMin;
	}

	void IPoolable.OnCreate()
	{
		MovementSpeed = Random.Range(MovementSpeedMin, MovementSpeedMax);
	}

	void IPoolable.OnRecycle()
	{

	}

	void Update()
	{

	}
}