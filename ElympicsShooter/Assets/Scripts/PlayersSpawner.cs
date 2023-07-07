using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersSpawner : ElympicsMonoBehaviour, IInitializable
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private Transform[] spawnPoints = null;

	private System.Random random = null;

	public static PlayersSpawner Instance = null;

	private void Awake()
	{
		if (PlayersSpawner.Instance == null)
			PlayersSpawner.Instance = this;
		else
			Destroy(this);
	}

	public void Initialize()
	{
		if (!Elympics.IsServer)
			return;

		random = new System.Random();

		if (playersProvider.IsReady)
			InitialSpawnPlayers();
		else
			playersProvider.IsReadyChanged += InitialSpawnPlayers;
	}

	private void InitialSpawnPlayers()
	{
		var preparedSpawnPoints = GetRandomizedSpawnPoints().Take(playersProvider.AllPlayersInScene.Length).ToArray();

		for (int i = 0; i < playersProvider.AllPlayersInScene.Length; i++)
		{
			playersProvider.AllPlayersInScene[i].transform.position = preparedSpawnPoints[i].position;
		}
	}

	public void SpawnPlayer(PlayerData player)
	{
		Vector3 spawnPoint = GetSpawnPointWithoutPlayersInRange().position;

		player.transform.position = spawnPoint;
	}

	private Transform GetSpawnPointWithoutPlayersInRange()
	{
		var randomizedSpawnPoints = GetRandomizedSpawnPoints();
		Transform chosenSpawnPoint = null;

		foreach (Transform spawnPoint in randomizedSpawnPoints)
		{
			chosenSpawnPoint = spawnPoint;

			Collider[] objectsInRange = Physics.OverlapSphere(chosenSpawnPoint.position, 3.0f);

			if (!objectsInRange.Any(x => x.transform.root.gameObject.TryGetComponent<PlayerData>(out _)))
				break;
		}

		return chosenSpawnPoint;
	}

	private IOrderedEnumerable<Transform> GetRandomizedSpawnPoints()
	{
		return spawnPoints.OrderBy(x => random.Next());
	}
}
