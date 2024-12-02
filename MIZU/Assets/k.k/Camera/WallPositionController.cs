using UnityEngine;

public class WallPositionController : MonoBehaviour
{
    public Camera mainCamera;        // ���C���J����
    public Transform leftWall;       // �����̕� (Cube)
    public Transform rightWall;      // �E���̕� (Cube)
    public float wallOffset = 0.5f;  // �ǂ̈ʒu�����p�I�t�Z�b�g�iCube�̕��ɍ��킹�Ē����j

    void Update()
    {
        // �J�����̍��[�ƉE�[�̃��[���h���W���擾
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));

        // �ǂ̈ʒu����ʒ[�ɐݒ�
        leftWall.position = new Vector3(leftEdge.x - wallOffset, leftWall.position.y, leftWall.position.z);
        rightWall.position = new Vector3(rightEdge.x + wallOffset, rightWall.position.y, rightWall.position.z);
    }
}
