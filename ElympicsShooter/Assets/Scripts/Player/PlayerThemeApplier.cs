using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class PlayerThemeApplier : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer fppThemeBasedRenderer = null;
    [SerializeField] private SkinnedMeshRenderer fppThemeBasedMetallicRenderer = null;
    [SerializeField] private SkinnedMeshRenderer tppMeshRenderer = null;

    private void Awake()
    {
        var playerData = GetComponent<PlayerData>();

        fppThemeBasedRenderer.material = playerData.ThemeMaterial;
        fppThemeBasedMetallicRenderer.material = playerData.ThemeMaterialMetallic;

        tppMeshRenderer.materials = new Material[]
            { playerData.ThemeMaterial, playerData.ThemeMaterialMetallic, tppMeshRenderer.materials[2] };
    }
}