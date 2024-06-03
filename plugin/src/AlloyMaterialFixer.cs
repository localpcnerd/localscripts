using UnityEngine;
using UnityEditor;
using System.Linq;
using Alloy;

public class AlloyMaterialFixer : EditorWindow
{
    public float MetallicLevel = 1f;
    public float SpecLevel = 1f;
    public float SpecTint = 0;
    public float RoughLevel = 1;
    public float OcclusionStrength = 1;
    public string PathString = "Assets/";

    [MenuItem("Tools/Mass Edit Alloy Materials")]
    private static void Init()
    {
        GetWindow<AlloyMaterialFixer>().Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Material Settings");
        GUILayout.Space(5);

        //metal
        GUILayout.Label("Metallic: " + MetallicLevel.ToString());
        float m = GUILayout.HorizontalSlider(MetallicLevel, 0, 1);
        MetallicLevel = m;
        MetallicLevel = Mathf.Round(MetallicLevel * 100f) / 100f;

        //spec
        GUILayout.Label("Specular: " + SpecLevel.ToString());
        float s = GUILayout.HorizontalSlider(SpecLevel, 0, 1);
        SpecLevel = s;
        SpecLevel = Mathf.Round(SpecLevel * 100f) / 100f;

        //spec tint
        GUILayout.Label("Specular Tint: " + SpecTint.ToString());
        float st = GUILayout.HorizontalSlider(SpecTint, 0, 1);
        SpecTint = st;
        SpecTint = Mathf.Round(SpecTint * 100f) / 100f;

        //rough
        GUILayout.Label("Roughness: " + RoughLevel.ToString());
        float r = GUILayout.HorizontalSlider(RoughLevel, 0, 1);
        RoughLevel = r;
        RoughLevel = Mathf.Round(RoughLevel * 100f) / 100f;

        //occlusion
        GUILayout.Label("Occlusion Strength: " + OcclusionStrength.ToString());
        float o = GUILayout.HorizontalSlider(OcclusionStrength, 0, 1);
        OcclusionStrength = o;
        OcclusionStrength = Mathf.Round(OcclusionStrength * 100f) / 100f;

        GUILayout.Space(2.5f);

        //folders to search
        GUILayout.Label("Folders To Search (Warning: Case Sensitive!)");
        PathString = GUILayout.TextField(PathString);

        if (GUILayout.Button("Find Alloy Materials"))
        {
            string[] paths = AssetDatabase.FindAssets("t:Material", new[] {PathString});
            if(paths.Length > 0)
            {
                Debug.Log("Materials found.");
            }
            else
            {
                Debug.LogWarning("Materials not found. Check your path for spelling errors.");
            }

            foreach(string guid in paths)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                var mat = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));

                Debug.Log(mat.name);

                mat.SetFloat("_Metal", MetallicLevel);
                mat.SetFloat("_Specularity", SpecLevel);
                mat.SetFloat("_SpecularTint", SpecTint);
                mat.SetFloat("_Roughess", RoughLevel);
                mat.SetFloat("_Occlusion", OcclusionStrength);
            }
        }
    }
}