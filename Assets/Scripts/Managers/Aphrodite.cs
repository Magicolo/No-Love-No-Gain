using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

public class Aphrodite : Singleton<Aphrodite>
{
	public Player PlayerPrefab;

	int _playerCount;
	Player[] _players;

	public int PlayerCount { get { return _playerCount; } }

	protected override void Awake()
	{
		base.Awake();

		_players = new Player[8];

		for (int i = 0; i < _players.Length; i++)
		{
			Player player = Instantiate(PlayerPrefab);
			player.InputHandler.SetJoystick((Joysticks)(i + 1));
			DespawnPlayer(player);

			_players[i] = player;
		}
	}

	void Update()
	{
		for (int i = 0; i < _players.Length; i++)
		{
			Player player = _players[i];

			if (player.gameObject.activeSelf == false && player.InputHandler.GetButtonDown("Start"))
				SpawnPlayer(player);
		}
	}

	void SpawnPlayer(Player player)
	{
		player.gameObject.SetActive(true);
		player.transform.parent = null;
		player.transform.position = transform.position;

		_playerCount++;
	}

	void DespawnPlayer(Player player)
	{
		player.gameObject.SetActive(false);
		player.transform.parent = transform;

		_playerCount--;
	}
}
