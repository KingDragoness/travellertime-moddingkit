using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static ScriptableObject CreateAsset<T>(string path) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T>();

		//string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		//if (path == "")
		//{
		//	path = "Assets";
		//}
		//else if (Path.GetExtension(path) != "")
		//{
		//	path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
		//}

		var backupPath = Path.GetExtension(path);
		backupPath = path.Replace(Path.GetFileName(path), "");

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(backupPath + "/test1.asset");
		
		AssetDatabase.CreateAsset(asset, assetPathAndName);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;

		return asset;
	}
}