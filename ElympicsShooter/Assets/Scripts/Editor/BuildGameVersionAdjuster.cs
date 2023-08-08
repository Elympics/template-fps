using UnityEditor;
using UnityEngine;
using Elympics;
using System;

public class BuildGameVersionAdjuster : MonoBehaviour
{
    private const string BUILD_VERSION_ENV_VARIABLE = "VERSION_BUILD_VAR";

    public static string AdjustGameVersion()
    {
        var elympicsConfig = ElympicsConfig.Load();
        var gameIndex = 0;
        string version = GetGameVersion();
        SetGameVersion(elympicsConfig.AvailableGames[gameIndex], version);

        return version;
    }

    private static void SetGameVersion(ElympicsGameConfig game, string version)
    {
        game.GameVersion = version;
        EditorUtility.SetDirty(game);
        AssetDatabase.SaveAssetIfDirty(game);
    }

    private static string GetGameVersion()
    {
        if (BuildCommand.TryGetEnv(BUILD_VERSION_ENV_VARIABLE, out string value))
        {
            if (int.TryParse(value, out int version))
            {
                Console.WriteLine($":: {BUILD_VERSION_ENV_VARIABLE} env var detected, set the version to {value}.");
                return version.ToString();
            }
            else
                Console.WriteLine($":: {BUILD_VERSION_ENV_VARIABLE} env var detected but the version value \"{value}\" is not an integer.");
        }

        throw new ArgumentNullException(nameof(value), $":: Error finding {BUILD_VERSION_ENV_VARIABLE} env var");
    }
}
