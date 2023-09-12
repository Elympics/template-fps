using Elympics;
using TMPro;
using UnityEngine;

public class NickNameSetter : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CursorController cursorController;
    [SerializeField] private PlayersProvider playersProvider;
    [SerializeField] private TMP_InputField inputField;

    private bool canChangeNickname;
    private bool isVisible;

    [ElympicsRpc(ElympicsRpcDirection.PlayerToServer)]
    public void SetNickname(string nickname,
        int id)
    {
        Debug.Log($"{nickname} on {id}");
        playersProvider.AllPlayersInScene[id].SetNickname(nickname);
    }

    public void SendNicknameButton() => canChangeNickname = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ChangeVisibility();
    }

    private void ChangeVisibility()
    {
        isVisible = !isVisible;
        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
        cursorController.SetCursorLock(!isVisible);
    }

    public void ElympicsUpdate()
    {
        if (canChangeNickname)
        {
            SetNickname(inputField.text, playersProvider.ClientPlayer.PlayerId);
            canChangeNickname = false;
        }
    }
}