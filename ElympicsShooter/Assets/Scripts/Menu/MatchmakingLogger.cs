using Elympics;
using System.Linq;
using TMPro;
using UnityEngine;

// Caution! Attaching this script to ElympicsLobby game object may cause problems with connecting to the second and next matches.
// The reason behind this is that ElympicsLobby is a persistent singleton gameobject and reloading the scene would cause references missmatch
// because of second instance which spawns when reloading the scene being destroyed.
public class MatchmakingLogger : MonoBehaviour
{
    [SerializeField] private MenuController menuController = null;
    [SerializeField] private TextMeshProUGUI matchmakingStatus = null;
    [SerializeField] private MatchmakingMessages[] messages = null;

    private MatchmakingMessages _currentMessageSet;

    private void Start()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted += DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound += _ => DisplayMatchFound();
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += _ => DisplayMatchmakingError();
    }

    private void OnDestroy()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted -= DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound -= _ => DisplayMatchFound();
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed -= _ => DisplayMatchmakingError();
    }

    private void SetMessages()
    {
        _currentMessageSet = messages.Where(x => x.messagesType == menuController.CurrentQueueType).FirstOrDefault();
    }

    private void DisplayMatchmakingStarted()
    {
        SetMessages();

        menuController.ControlPlayAccess(false);

        matchmakingStatus.text = _currentMessageSet.startingMessage;
    }

    private void DisplayMatchFound()
    {
        matchmakingStatus.text = _currentMessageSet.finishedMessage;
    }

    private void DisplayMatchmakingError()
    {
        menuController.ControlPlayAccess(true);

        matchmakingStatus.text = _currentMessageSet.errorMessage;
    }
}


[System.Serializable]
public struct MatchmakingMessages
{
    public GameQueueMessagesType messagesType;
    public string startingMessage;
    public string finishedMessage;
    public string errorMessage;
}

public enum GameQueueMessagesType
{
    SoloMessages,
    DuelMessages
}