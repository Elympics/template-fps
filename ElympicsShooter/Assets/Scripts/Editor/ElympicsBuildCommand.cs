using Elympics;
using System;
using UnityEngine;

public static class ElympicsBuildCommand
{
	private const string ELYMPICS_USERNAME_ENV_VARIABLE = "ELYMPICS_USERNAME";
	private const string ELYMPICS_PASSWORD_ENV_VARIABLE = "ELYMPICS_PASSWORD";

	public static void BuildAndUpload()
	{
		var version = BuildGameVersionAdjuster.AdjustGameVersion();
		Debug.Log($"Building and uploading server for version >>> {version} <<<");
		var username = Environment.GetEnvironmentVariable(ELYMPICS_USERNAME_ENV_VARIABLE);
		var password = Environment.GetEnvironmentVariable(ELYMPICS_PASSWORD_ENV_VARIABLE);
		ElympicsWebIntegration.BuildAndUploadServerInBatchmode(username, password);
	}
}
