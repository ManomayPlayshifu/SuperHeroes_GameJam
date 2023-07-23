using UnityEditor;
using UnityEngine;

public class ReplaceBoxColliderWithMeshCollider : EditorWindow
{
    [MenuItem("Custom Tools/Replace Box Collider with Mesh Collider")]
    static void Init()
    {
        ReplaceBoxColliderWithMeshCollider window = (ReplaceBoxColliderWithMeshCollider)EditorWindow.GetWindow(typeof(ReplaceBoxColliderWithMeshCollider));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace Box Collider with Mesh Collider", EditorStyles.boldLabel);

        if (GUILayout.Button("Replace"))
        {
            ReplaceColliders(Selection.activeGameObject);
        }
    }

    private void ReplaceColliders(GameObject obj)
    {
        if (obj == null)
            return;

        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();

        if (boxCollider != null && meshFilter != null && meshFilter.sharedMesh != null)
        {
            Undo.DestroyObjectImmediate(boxCollider);
            Undo.AddComponent<MeshCollider>(obj);
        }
        else
        {
            Debug.LogWarning("Object '" + obj.name + "' does not have a BoxCollider or MeshFilter with a valid Mesh.");
        }

        // Recursively check child objects
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            ReplaceColliders(obj.transform.GetChild(i).gameObject);
        }
    }
}
