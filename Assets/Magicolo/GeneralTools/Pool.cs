using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo
{
	public static class Pool<T> where T : class, IPoolable, new()
	{
		public static event Action<T> OnCreate;
		public static event Action<T> OnRecycle;

		static readonly Queue<T> _pool = new Queue<T>(4);

		static bool isScriptableObject;

		static Pool()
		{
			isScriptableObject = typeof(ScriptableObject).IsAssignableFrom(typeof(T));
		}

		public static T Create()
		{
			T item = GetItem();

			item.OnCreate();
			RaiseOnCreateEvent(item);

			return item;
		}

		public static TC Create<TC>(TC reference) where TC : class, T, ICopyable<T>
		{
			TC item = (TC)GetItem();

			if (reference != null)
				item.Copy(reference);

			item.OnCreate();
			RaiseOnCreateEvent(item);

			return item;
		}

		public static void CreateElements<TC>(IList<TC> array) where TC : class, T, ICopyable<T>
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				array[i] = Create(array[i]);
		}

		public static void Recycle(T item)
		{
			if (item == null)
				return;

			item.OnRecycle();
			RaiseOnRecycleEvent(item);

#if UNITY_EDITOR
			if (isScriptableObject && !Application.isPlaying)
				(item as ScriptableObject).Destroy();
			else
#endif
				_pool.Enqueue(item);
		}

		public static void Recycle(ref T item)
		{
			Recycle(item);

			item = null;
		}

		public static void RecycleElements(IList<T> array)
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
			{
				Recycle(array[i]);
				array[i] = null;
			}
		}

		public static bool Contains(T item)
		{
			return _pool.Contains(item);
		}

		public static int Count()
		{
			return _pool.Count;
		}

		public static void Clear()
		{
			_pool.Clear();
		}

		static T GetItem()
		{
			T item = null;

			while (item == null)
			{
				if (_pool.Count > 0)
					item = _pool.Dequeue();
				else if (isScriptableObject)
					item = ScriptableObject.CreateInstance(typeof(T)) as T;
				else
					item = new T();
			}

			return item;
		}

		static void RaiseOnCreateEvent(T item)
		{
			if (OnCreate != null)
				OnCreate(item);
		}

		static void RaiseOnRecycleEvent(T item)
		{
			if (OnRecycle != null)
				OnRecycle(item);
		}
	}

	public static class ComponentPool<T> where T : Component
	{
		public static event Action<T> OnCreate;
		public static event Action<T> OnRecycle;

		static readonly Queue<T> _pool = new Queue<T>(4);

		public static T Create(T reference)
		{
			T item = GetItem(reference);

			item.gameObject.SetActive(true);
			RaiseOnCreateEvent(item);

			return item;
		}

		public static void Recycle(T item)
		{
			if (item == null)
				return;

			item.gameObject.SetActive(false);
			RaiseOnRecycleEvent(item);

			_pool.Enqueue(item);
		}

		public static void Recycle(ref T item)
		{
			Recycle(item);

			item = null;
		}

		public static bool Contains(T item)
		{
			return _pool.Contains(item);
		}

		public static int Count()
		{
			return _pool.Count;
		}

		public static void Clear()
		{
			_pool.Clear();
		}

		static T GetItem(T reference)
		{
			T item = null;

			while (item == null)
			{
				if (_pool.Count > 0)
					item = _pool.Dequeue();
				else
					item = UnityEngine.Object.Instantiate(reference);
			}

			return item;
		}

		static void RaiseOnCreateEvent(T item)
		{
			if (OnCreate != null)
				OnCreate(item);
		}

		static void RaiseOnRecycleEvent(T item)
		{
			if (OnRecycle != null)
				OnRecycle(item);
		}
	}
}