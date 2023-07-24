using UnityEngine;
using UnityEditor;

public class MeshColliderAdd : EditorWindow
{
    [MenuItem("Custom/Add Mesh Colliders to Mesh Renderers")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MeshColliderAdd));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add Mesh Colliders"))
        {
            AddMeshColliders();
        }
    }

    private void AddMeshColliders()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            AddMeshColliderToObject(obj);
        }
    }

    private void AddMeshColliderToObject(GameObject obj)
    {
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = obj.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
            }
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            AddMeshColliderToObject(child.gameObject);
        }
    }
}
