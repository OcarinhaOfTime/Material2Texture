using UnityEngine;
using UnityEditor;
using System.IO;

public class Material2Texture : EditorWindow {
    private Material mat;
    private string texPath;
    private int width = 1024;
    private int height = 1024;
    private static Material2Texture window;

    [MenuItem("Window/Material to texture")]
    static void Init() {
        Debug.Log(AssetDatabase.GetAssetPath(Selection.activeObject));

        window = GetWindow<Material2Texture>();
        window.Setup();
        window.Show();
    }

    public void Setup() {
        if (Selection.activeObject is Material) {
            mat = (Material)Selection.activeObject;
        }
    }

    void OnGUI() {
        GUILayout.Label("Material to Texture Settings", EditorStyles.boldLabel);
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        mat = (Material)EditorGUILayout.ObjectField("Material", mat, typeof(Material), false);

        if (GUILayout.Button("Save to texture")) {
            var dir = AssetDatabase.GetAssetPath(Selection.activeObject);
            dir = Path.GetDirectoryName(dir);
            var path = EditorUtility.SaveFilePanel("Save to texture", dir, mat.name, "png");
            SaveToTexture(path);
            AssetDatabase.Refresh();
            window.Close();
        }
    }

    public void SaveToTexture(string path) {
        RenderTexture rt = new RenderTexture(width, height, 0);
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Graphics.Blit(rt, rt, mat);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }
}