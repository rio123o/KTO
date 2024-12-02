using UnityEngine;

public class WallPositionController : MonoBehaviour
{
    public Camera mainCamera;        // メインカメラ
    public Transform leftWall;       // 左側の壁 (Cube)
    public Transform rightWall;      // 右側の壁 (Cube)
    public float wallOffset = 0.5f;  // 壁の位置調整用オフセット（Cubeの幅に合わせて調整）

    void Update()
    {
        // カメラの左端と右端のワールド座標を取得
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));

        // 壁の位置を画面端に設定
        leftWall.position = new Vector3(leftEdge.x - wallOffset, leftWall.position.y, leftWall.position.z);
        rightWall.position = new Vector3(rightEdge.x + wallOffset, rightWall.position.y, rightWall.position.z);
    }
}
