using UnityEngine;

public class WaterControl : MonoBehaviour
{
    public GameObject water; // �㉺���鐅�I�u�W�F�N�g
    public float ascendAmount = 1.0f; // �㏸���̈ړ���
    public float descendAmount = 1.0f; // ���~���̈ړ���
    public float moveSpeed = 2.0f; // �ړ����x
    public bool isAscending = true; // �㏸���邩���~���邩(True�Ȃ�オ��)

    private bool isMoving = false; // ���݈ړ������ǂ���
    private Vector3 targetPosition; // ���̖ڕW�ʒu

    void Update()
    {
        if (isMoving)
        {
            // ���݂̈ʒu��ڕW�ʒu�Ɍ����Ĉړ�
            water.transform.position = Vector3.MoveTowards(
                water.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // �ڕW�ʒu�ɓ��B������ړ����~
            if (Vector3.Distance(water.transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving) // �ړ����łȂ��ꍇ�̂ݓ���
        {
            // �ړ��ڕW�ʒu��ݒ�
            float direction = isAscending ? ascendAmount : -descendAmount;
            targetPosition = water.transform.position + new Vector3(0, direction, 0);

            // �ړ����J�n
            isMoving = true;

            // �R���C�_�[���ꎞ�I�ɖ�����
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
