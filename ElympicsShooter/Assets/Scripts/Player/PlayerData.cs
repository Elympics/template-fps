using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Parameters:")]
    [SerializeField] private int playerId = 0;
    [SerializeField] private string nickname = "";
    [SerializeField] private Color themeColor = Color.white;
    [SerializeField] private Material themeMaterial = null;
    [SerializeField] private Material themeMaterialMetallic = null;

    [Header("References:")]
    [SerializeField] private StatsController statsController = null;
    [SerializeField] private DeathController deathController = null;
    [SerializeField] private LoadoutController loadoutController = null;

    public int PlayerId => playerId;
    public string Nickname => nickname;
    public Color ThemeColor => themeColor;
    public Material ThemeMaterial => themeMaterial;
    public Material ThemeMaterialMetallic => themeMaterialMetallic;
    public DeathController DeathController => deathController;
    public StatsController StatsController => statsController;
    public LoadoutController LoadoutController => loadoutController;

    public void SetNickname(string nickname) => this.nickname = nickname;
}