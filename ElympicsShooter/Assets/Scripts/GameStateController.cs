using Elympics;
using UnityEngine;

public class GameStateController : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("References:")]
    [SerializeField] private GameInitializer gameInitializer = null;
    [SerializeField] private PlayerScoresManager playerScoresManager = null;
    private int previousWinner;

    public ElympicsInt CurrentGameState { get; } = new ElympicsInt((int)GameState.Prematch);

    public void Initialize()
    {
        gameInitializer.InitializeMatch(() => ChangeGameState(GameState.GameplayMatchRunning));
    }

    private void ProcessGameStateBasedOnWinnerIdChanged(int newValue)
    {
        if (previousWinner == newValue)
            return;

        previousWinner = newValue;

        if (newValue >= 0)
        {
            ChangeGameState(GameState.MatchEnded);
        }
    }

    private void ChangeGameState(GameState newGameState)
    {
        CurrentGameState.Value = (int)newGameState;
    }

    public void ElympicsUpdate()
    {
        ProcessGameStateBasedOnWinnerIdChanged(playerScoresManager.WinnerPlayerId.Value);
    }
}