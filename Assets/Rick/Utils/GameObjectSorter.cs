﻿using UnityEngine;
using System.Collections.Generic;
using Magicolo;
using Rick;

public static class GameObjectSorter
{
	public static IEnumerable<T> SortObjectByTransformX<T>(T[] things) where T : MonoBehaviour
	{
		List<T> sorted = new List<T>();

		foreach (T item in things)
		{
			int index = 0;
			bool found = false;
			foreach (var sortedItem in sorted)
			{
				if (sortedItem.transform.position.x > item.transform.position.x)
				{
					sorted.Insert(index, item);
					found = true;
					break;
				}
				index++;
			}

			if (!found)
			{
				sorted.Add(item);
			}
		}

		return sorted;
	}

}