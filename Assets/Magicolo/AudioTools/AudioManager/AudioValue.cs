using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;

namespace Magicolo
{
	public class AudioValue<T> : IPoolable, ICopyable<AudioValue<T>>
	{
		T _value;

		public T Value { get { return _value; } set { _value = value; } }

		public virtual void OnCreate()
		{
		}

		public virtual void OnRecycle()
		{
		}

		public void Copy(AudioValue<T> reference)
		{
			_value = reference._value;
		}
	}
}