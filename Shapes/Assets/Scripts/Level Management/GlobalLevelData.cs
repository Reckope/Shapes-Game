/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used as global stirage for the levels.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalLevelData
{	
	public static List<LevelInfo> levels = new List<LevelInfo>();

	public static string ActiveLevelName { get; set; }
	public static int ActiveLevelIndex { get; set; }
	public static bool LevelIsActive { get; set; }
}
