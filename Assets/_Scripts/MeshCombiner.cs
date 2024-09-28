using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering; // For IndexFormat

public class MeshCombiner : MonoBehaviour
{
    // Source Meshes you want to combine
    [SerializeField] private List<MeshFilter> listMeshFilter;

    // Make a new mesh to be the target of the combine operation
    [SerializeField] private MeshFilter TargetMesh;

    [ContextMenu("Combine Meshes")]
    private void CombineMesh()
    {
        if (listMeshFilter.Count == 0)
        {
            Debug.LogError("No meshes to combine!");
            return;
        }

        // Make an array of CombineInstance.
        var combine = new CombineInstance[listMeshFilter.Count];

        // Set Mesh and their Transform to the CombineInstance
        for (int i = 0; i < listMeshFilter.Count; i++)
        {
            combine[i].mesh = listMeshFilter[i].sharedMesh;
            combine[i].transform = listMeshFilter[i].transform.localToWorldMatrix;
        }

        // Create an Empty Mesh
        var mesh = new Mesh
        {
            // Set the index format to UInt32 to handle meshes with more than 65535 vertices
            indexFormat = IndexFormat.UInt32
        };

        // Call CombineMeshes and pass in the array of CombineInstances
        mesh.CombineMeshes(combine);

        // Assign the combined mesh to the mesh filter of the combination game object
        TargetMesh.mesh = mesh;

        // Save The Mesh To Location
        SaveMesh(TargetMesh.sharedMesh, gameObject.name, false, true);

        // Print Results
        print($"<color=#20E7B0>Combine Meshes was Successful!</color>");
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
#if UNITY_EDITOR
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");

        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
#endif
    }
}
