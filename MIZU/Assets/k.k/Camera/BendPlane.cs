using UnityEngine;

public class BendPlane : MonoBehaviour
{
    public float archHeight = 2f; // �A�[�`�̍���
    public float archRadius = 5f; // �A�[�`�̔��a

    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null) return;

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            float distance = vertex.x; // x���W����ɕό`
            float angle = distance / archRadius; // �A�[�`�̊p�x���v�Z
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
