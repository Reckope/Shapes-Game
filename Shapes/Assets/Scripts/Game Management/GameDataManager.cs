/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used for managing all the storage data.
*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use a class instead of a structure, as values will be changed in the list. 
[Serializable]
public class LevelInfo
{
	public string levelID;
	public string levelName;
	public string description;
	public int buildIndex;
	public bool isUnlocked;
	public bool isActive;
	public bool isCompleted;
}

[Serializable]
public class PlayerStatsinfo
{
	public string ID;
	public string displayName;
	public float value;
}

// Store the levels from LevelData.json in a generic list.
[Serializable]
public class LevelDataCollection
{
	public List<LevelInfo> levels = new List<LevelInfo>();
}

[Serializable]
public class PlayerStatsCollection
{
	public List<PlayerStatsinfo> playerStats = new List<PlayerStatsinfo>();
}

// Make it static so data persists between scenes. 
[Serializable]
public static class GameData
{	
	public enum PlayerStatIDs
	{
		TimePlayed,
		TotalDeaths,
		DynamoKilled,
		CinderKilled,
		PriwenKilled,
		AegisKilled
	}

	public static List<LevelInfo> levels = new List<LevelInfo>();
	public static List<PlayerStatsinfo> playerStats = new List<PlayerStatsinfo>();

	public static string LevelFilePathLocation { get { return Application.dataPath + "/LevelData.json"; } }
	public static string PlayerStatsFilePathLocation { get { return Application.dataPath + "/PlayerStats.json"; } }

	public static string ActiveLevelName { get; set; }
	public static int ActiveLevelIndex { get; set; }
	public static bool LevelIsActive { get; set; }
	public static float TimePlayedValue { get; set; }

	public static LevelDataCollection levelData;
	public static PlayerStatsCollection playerStatsData;

	public static void DeleteExistingSave()
	{
		File.Delete(LevelFilePathLocation);
		File.Delete(PlayerStatsFilePathLocation);
	}

	public static void LoadLevelDataFromResources()
	{
		TextAsset levelDataText = (TextAsset)Resources.Load("LevelData", typeof(TextAsset));
		TextAsset playerStatsText = (TextAsset)Resources.Load("PlayerStats", typeof(TextAsset));

		String dataLevels = levelDataText.text;
		String dataPlayerStats = playerStatsText.text;

		levelData = JsonUtility.FromJson<LevelDataCollection>(dataLevels);
		playerStatsData = JsonUtility.FromJson<PlayerStatsCollection>(dataPlayerStats);

		levels = new List<LevelInfo>(levelData.levels);
		playerStats = new List<PlayerStatsinfo>(playerStatsData.playerStats);
	}

	public static void LoadGame()
	{
		if(File.Exists(LevelFilePathLocation) && File.Exists(PlayerStatsFilePathLocation))
		{
			levelData = JsonUtility.FromJson<LevelDataCollection>(File.ReadAllText(LevelFilePathLocation));
			playerStatsData = JsonUtility.FromJson<PlayerStatsCollection>(File.ReadAllText(PlayerStatsFilePathLocation));
			Debug.Log("Levels Loaded from: " + LevelFilePathLocation);
			Debug.Log("Player Stats Loaded from: " + PlayerStatsFilePathLocation);
		}
	}

	public static void SaveGame()
	{
		string dataLevels = JsonUtility.ToJson(levelData);
		string dataPlayerStats = JsonUtility.ToJson(playerStatsData);

		File.WriteAllText(LevelFilePathLocation, dataLevels);
		File.WriteAllText(PlayerStatsFilePathLocation, dataPlayerStats);
		Debug.Log("Saved to: " + Application.dataPath);
	}

	public static void IncrementPlayerStatsData(PlayerStatIDs statID)
	{
		string _statID = statID.ToString();
		PlayerStatsinfo stat = playerStatsData.playerStats.Find((x) => x.ID == _statID);
		stat.value ++;
	}

	public static void IncrementTimePlayed()
	{
		PlayerStatsinfo stat = playerStatsData.playerStats.Find((x) => x.ID == PlayerStatIDs.TimePlayed.ToString());
		stat.value += Time.deltaTime;
	}
}
