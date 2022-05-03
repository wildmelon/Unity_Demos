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
    // 下面是添加菜单项以创建 RoadTile 资源的 helper 函数
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