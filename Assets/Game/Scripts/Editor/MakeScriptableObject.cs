using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject 
{
	[MenuItem("Assets/Create ScriptableObject/ShipsVariation")]
	public static void CreateShipsVariation()
	{
		ShipsVariation asset = ScriptableObject.CreateInstance<ShipsVariation>();

		AssetDatabase.CreateAsset(asset, "Assets/ShipsVariation.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}



}