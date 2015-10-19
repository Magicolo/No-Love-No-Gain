using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;

// Utiliser le design BaseManager, AKA get rid of this class
public abstract class Singleton<T> : MonoBehaviourExtended where T : Singleton<T>
{
	protected static T _instance;
	public static T Instance { get { return _instance; } }

	public static T Find()
	{
		_instance = FindObjectOfType<T>();

		return _instance;
	}

	protected virtual void Awake()
	{
		if (_instance = null)
			_instance = this as T;
	}
}
