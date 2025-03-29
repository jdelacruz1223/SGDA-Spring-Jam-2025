#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

public class SeedAssetCreator
{
    [MenuItem("Assets/Create/Scriptable Objects/Auto-Increment Seed")]
    public static void CreateAutoIncrementSeed()
    {
        var allSeeds = AssetDatabase.FindAssets("t:Seeds")
            .Select(guid => AssetDatabase.LoadAssetAtPath<Seeds>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(seed => seed.seedData != null)
            .ToList();

        int nextId = allSeeds.Count > 0 ? allSeeds.Max(seed => seed.seedData.id) + 1 : 1;

        Seeds newSeed = ScriptableObject.CreateInstance<Seeds>();
        newSeed.seedData = new SeedModel();
        newSeed.seedData.id = nextId;

        string path = EditorUtility.SaveFilePanelInProject("Save New Seed", $"Seed_{nextId}", "asset", "Save your new seed asset");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(newSeed, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newSeed;
        }
    }
}
#endif
