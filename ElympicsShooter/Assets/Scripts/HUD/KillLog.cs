using Elympics;
using TMPro;
using UnityEngine;

public class KillLog : ElympicsMonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private TextMeshProUGUI killerTextMesh;
    [SerializeField] private TextMeshProUGUI victimTextMesh;


    [ElympicsRpc(ElympicsRpcDirection.ServerToPlayers)]
    public void SetupKillLog(string killer,
        string victim)
    {
        canvasGroup.alpha = 1;
        killerTextMesh.text = killer;
        victimTextMesh.text = victim;

        Invoke(nameof(TurnOffVisibility), 3f);
    }

    private void TurnOffVisibility()
    {
        canvasGroup.alpha = 0;
    }
}