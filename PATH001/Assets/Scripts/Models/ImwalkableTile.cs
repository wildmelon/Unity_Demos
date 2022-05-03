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
    // 下面是添加菜单项以创建 RoadTile 资源的 helper 函数
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