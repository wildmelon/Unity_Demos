using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
# endif

public class ImwalkableTile : Tile
{
    public ImwalkableTile()
    {
        flags = TileFlags.None;
    }

#if UNITY_EDITOR
    // ��������Ӳ˵����Դ��� RoadTile ��Դ�� helper ����
    [MenuItem("Assets/Create/ImwalkableTile")]
    public static void CreateImwalkableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save ImwalkableTile", "New ImwalkableTile", "Asset", "Save ImwalkableTile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ImwalkableTile>(), path);
    }
# endif
}