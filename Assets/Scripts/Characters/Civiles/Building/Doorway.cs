using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class Doorway : MonoBehaviour
{
	public Building building;
	public Vector2 ExitDirection;

	void OnTriggerEnter2D(Collider2D other)
	{
		Civile civile = other.GetComponent<Civile>();
		if (civile && civile.LeftEntrance)
		{
			BehaviourPool<Civile>.Recycle(civile);
			building.CurrentCivilesCount++;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		Civile civile = other.GetComponent<Civile>();
		if (civile)
		{
			civile.LeftEntrance = true;
		}
	}
}