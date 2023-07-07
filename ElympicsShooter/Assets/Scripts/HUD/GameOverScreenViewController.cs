using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenViewController : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private TextMeshProUGUI winnerInfoText = null;
	[SerializeField] private int mainMenuSceneIndex = 0;

	public void ShowGameOverScreen(PlayerData winnerData)
	{
		canvasGroup.alpha = 1.0f;

		winnerInfoText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(winnerData.ThemeColor)}>Player {winnerData.PlayerId}</color> won the game!";
	}

	public void OnBackToMenuClicked()
	{
		SceneManager.LoadScene(mainMenuSceneIndex);
	}
}
