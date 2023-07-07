using Elympics;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ElympicsBuildCommand
{
	private const string ELYMPICS_USERNAME_ENV_VARIABLE = "ELYMPICS_USERNAME";
	private const string ELYMPICS_PASSWORD_ENV_VARIABLE = "ELYMPICS_PASSWORD";
	private const string BUILD_VERSION_ENV_VARIABLE = "VERSION_BUILD_VAR";

	public static void BuildAndUpload()
	{
		var elympicsConfig = ElympicsConfig.Load();
		var gameIndex = 0;
		string version = GetGameVersion();
		SetGameVersion(elympicsConfig.AvailableGames[gameIndex], version);
		Debug.Log($"Building and uploading server for version >>> {version} <<<");
		var username = Environment.GetEnvironmentVariable(ELYMPICS_USERNAME_ENV_VARIABLE);
		var password = Environment.GetEnvironmentVariable(ELYMPICS_PASSWORD_ENV_VARIABLE);
		ElympicsWebIntegration.BuildAndUploadServerInBatchmode(username, password);
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
