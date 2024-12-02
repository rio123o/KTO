using UnityEngine;

public class BendPlane : MonoBehaviour
{
    public float archHeight = 2f; // アーチの高さ
    public float archRadius = 5f; // アーチの半径

    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null) return;

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            float distance = vertex.x; // x座標を基準に変形
            float angle = distance / archRadius; // アーチの角度を計算
            vertices[i] = new Vector3(
                vertex.x,
                Mathf.Sin(angle) * archHeight,
                Mathf.Cos(angle) * archRadius - archRadius
            );
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
