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
using UnityEngine.SceneManagement;

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

// Store the levels from LevelData.json in a generic list.
[Serializable]
public class LevelDataCollection
{
	public List<LevelInfo> levels = new List<LevelInfo>();
}

// Make it static so data persists between scenes. 
[Serializable]
public static class GameData
{	
	public static List<LevelInfo> levels = new List<LevelInfo>();

	public static string FilePath { get { return Application.dataPath + "/LevelData.json"; } }
	public static string ActiveLevelName { get; set; }
	public static int ActiveLevelIndex { get; set; }
	public static bool LevelIsActive { get; set; }

	public static LevelDataCollection levelData;

	public static void DeleteExistingSave()
	{
		File.Delete(FilePath);
	}

	public static void LoadLevelDataFromResources()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("LevelData", typeof(TextAsset));
		String data = textAsset.text;
		levelData = JsonUtility.FromJson<LevelDataCollection>(data);
		levels = new List<LevelInfo>(levelData.levels);
	}

	public static void LoadGame()
	{
		if(File.Exists(FilePath))
		{
			levelData = JsonUtility.FromJson<LevelDataCollection>(File.ReadAllText(FilePath));
			Debug.Log("Loaded from: " + FilePath);
		}
	}

	public static void Savegame()
	{
		//if(File.Exists(Application.dataPath + "/LevelData.json"))
		//{
			string data = JsonUtility.ToJson(levelData);
			File.WriteAllText(FilePath, data);
			Debug.Log("Saved to: " + Application.dataPath);
		//}
	}
}
