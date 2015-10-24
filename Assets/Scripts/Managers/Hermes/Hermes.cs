using UnityEngine;
using System.Collections.Generic;
using Magicolo;
using Rick;

[ExecuteInEditMode]
public class Hermes : Singleton<Hermes>
{
	List<PlateformPath> Paths = new List<PlateformPath>();
	//public int startingPopulation; // SUPER UNUSED

	public List<Civile> CivilePrefabs;


	[Button("Refresh Paths ", "RefreshPathsLines")]
	public bool RefreshPaths;

	void Start()
	{
		/*Building[] buildings = Object.FindObjectsOfType<Building>();
		int nbBuilding = buildings.Length;
		foreach (var building in buildings)
		{
			building.CurrentCivilesCount = startingPopulation / nbBuilding;
		}*/
	}


	void Update()
	{

	}

	void OnDrawGizmos()
	{
		for (int i = 0; i < Paths.Count; i++)
		{
			PlateformPath path = Paths[i];
			Gizmos.color = new Color(1, 1, 1);
			Vector2 lastPoint = path.points[0];
			Gizmos.DrawSphere(lastPoint, 0.1f);
			for (int j = 1; j < path.points.Count; j++)
			{
				Gizmos.color = new Color(j / path.points.Count, 1, 1);
				Gizmos.DrawLine(lastPoint, path.points[j]);
				lastPoint = path.points[j];
			}
			Gizmos.color = new Color(0, 0, 0);
			Gizmos.DrawSphere(lastPoint + new Vector2(0, 0.1f), 0.1f);
		}
	}

	public void RefreshPathsLines()
	{
		Paths.Clear();
		Plateform[] plateforms = Object.FindObjectsOfType<Plateform>();

		foreach (var plateform in GameObjectSorter.SortObjectByTransformX(plateforms))
		{
			//TODO Sort d'un ordre particulier pour que ca aille mieux
			BoxCollider2D box = plateform.GetComponent<BoxCollider2D>();
			AddSegment(box.GetTopLeftCorner(), box.GetTopRightCorner());
		}

		foreach (var building in Object.FindObjectsOfType<BuildingBase>())
		{
			BoxCollider2D box = building.GetComponent<BoxCollider2D>();
			RemoveSegment(box.GetBottomLeftCorner(), box.GetBottomRightCorner());
		}

		/*foreach (var item in Paths)
		{
			Debug.Log("--");
			foreach (var p in item.points)
			{
				Debug.Log(p);
			}
		}
		Debug.Log(Paths.Count);*/
	}

	// On assume que a et b sont horizontale et que les points le sont aussi, eventuellement a amelirer :)
	private void RemoveSegment(Vector2 a, Vector2 b)
	{
		List<PlateformPath> newPaths = new List<PlateformPath>();
		foreach (var path in Paths)
		{
			int pointIndexLeft = -1;
			int pointIndexRight = -1;
			foreach (var p in path.points)
			{
				if (System.Math.Abs(p.y - a.y) < 0.01f)
				{
					if (p.x < a.x)
					{
						pointIndexLeft = path.points.IndexOf(p);
					}
					if (pointIndexRight == -1 && p.x > b.x)
					{
						pointIndexRight = path.points.IndexOf(p);
					}
				}
			}
			if (pointIndexLeft == -1)
			{
				newPaths.Add(path);
			}
			else
			{
				PlateformPath newPath1 = new PlateformPath();
				for (int i = 0; i <= pointIndexLeft; i++)
				{
					newPath1.points.Add(path.points[i]);
				}
				newPath1.points.Add(a);
				newPaths.Add(newPath1);

				PlateformPath newPath2 = new PlateformPath();
				newPath2.points.Add(b);
				for (int i = pointIndexRight; i < path.points.Count; i++)
				{
					newPath2.points.Add(path.points[i]);
				}
				newPaths.Add(newPath2);
			}
		}
		Paths.Clear();
		Paths.AddRange(newPaths);
	}

	private void AddSegment(Vector2 a, Vector2 b)
	{
		bool found = false;
		foreach (var path in Paths)
		{

			if (path.Contains(a))
			{
				path.points.Insert(path.IndexOf(a) + 1, b);
				found = true;
			}
			else if (path.Contains(b))
			{
				path.points.Insert(path.IndexOf(b) + 1, a);
				found = true;
			}
		}

		if (!found)
		{

			PlateformPath path = new PlateformPath();
			path.points.Add(a);
			path.points.Add(b);
			Paths.Add(path);
		}
	}
}


[System.Serializable]
public class PlateformPath
{
	public List<Vector2> points = new List<Vector2>();

	public bool Contains(Vector2 point)
	{
		foreach (var p in points)
		{
			if (p == point)
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(Vector2 point)
	{
		int index = 0;
		foreach (var p in points)
		{
			if (p == point)
			{
				return index;
			}
			index++;
		}
		return -1;
	}
}
