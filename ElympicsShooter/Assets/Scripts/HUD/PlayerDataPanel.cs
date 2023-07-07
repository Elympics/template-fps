using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataPanel : MonoBehaviour
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private Sprite[] playerIconSprites = null;
	[SerializeField] private TextMeshProUGUI playerNickname = null;
	[SerializeField] private Image playerIcon = null;

	private void Start()
	{
		if (playersProvider.IsReady)
			SetupViewBasedOnClientPlayer();
		else
			playersProvider.IsReadyChanged += SetupViewBasedOnClientPlayer;
	}

	private void SetupViewBasedOnClientPlayer()
	{
		PlayerData clientPlayer = playersProvider.ClientPlayer;

		playerNickname.text = clientPlayer.Nickname;

		playerIcon.sprite = playerIconSprites[clientPlayer.PlayerId];
	}
}
