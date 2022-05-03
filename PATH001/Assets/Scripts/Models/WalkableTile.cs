using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
# endif

public class WalkableTile : Tile
{
    public WalkableTile()
    {
        flags = TileFlags.None;
    }
#if UNITY_EDITOR
    // ��������Ӳ˵����Դ��� RoadTile ��Դ�� helper ����
    [MenuItem("Assets/Create/WalkableTile")]
    public static void CreateWalkableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save WalkableTile", "New WalkableTile", "Asset", "Save WalkableTile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WalkableTile>(), path);
    }
# endif
}