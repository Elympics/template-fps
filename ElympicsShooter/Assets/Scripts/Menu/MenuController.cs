using System;
using Elympics;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Elympics.Models.Authentication;
using Elympics.Models.Matchmaking;

// Caution! Attaching this script to ElympicsLobby game object may cause problems with connecting to the second and next matches.
// The reason behind this is that ElympicsLobby is a persistent singleton gameobject and reloading the scene would cause references missmatch
// because of second instance which spawns when reloading the scene being destroyed.
public class MenuController : MonoBehaviour
{
	[SerializeField] private List<Button> playButtons = null;
	[SerializeField] private string matchmakingQueueDuel = "Default"; // default, predefined queue (feel free to change it)
	[SerializeField] private string matchmakingQueueSolo = "Solo"; // name defined in the panel https://www.elympics.cc/
	[SerializeField] private string matchmakingAdditionalInfo = "Free"; // additional info which may be useful to separate players in the same queue

	public GameQueueMessagesType CurrentQueueType { get; private set; }

	private void Start()
	{
		ElympicsLobbyClient.Instance.AuthenticationSucceeded += HandleAuthenticated;
		ElympicsLobbyClient.Instance.AuthenticationFailed += HandleError;
		ControlPlayAccess(ElympicsLobbyClient.Instance.IsAuthenticated);
	}

	private void HandleError(string Error)
	{
		Debug.Log(Error);
	}

	private void HandleAuthenticated(AuthData result)
	{
		ControlPlayAccess(true);
	}

	private void PlayOnline(string matchmakingQueue,
		string matchmakingAdditionalInfo)
	{
		ElympicsLobbyClient.Instance.PlayOnlineInClosestRegionAsync(null, null,
			$"{matchmakingQueue}:{matchmakingAdditionalInfo}");
		ControlPlayAccess(false);
	}

	public void OnPlayDuelClicked()
	{
		CurrentQueueType = GameQueueMessagesType.DuelMessages;
		PlayOnline(matchmakingQueueDuel, matchmakingAdditionalInfo);
	}

	public void OnPlaySoloClicked()
	{
		CurrentQueueType = GameQueueMessagesType.SoloMessages;
		PlayOnline(matchmakingQueueSolo, matchmakingAdditionalInfo);
	}

	public void ControlPlayAccess(bool allowToPlay)
	{
		playButtons.ForEach(x => x.interactable = allowToPlay);
	}
}