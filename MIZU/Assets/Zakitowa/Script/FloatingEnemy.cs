using UnityEngine;

public class FloatingEnemy : MonoBehaviour
{
    public float moveSpeed = 2.0f; // �������̈ړ����x
    public float floatAmplitude = 0.5f; // �㉺�̐U��
    public float floatFrequency = 2.0f; // �㉺�̎��g��

    private Vector3 startPosition;

    void Start()
    {
        // �����ʒu���L�^
        startPosition = transform.position;
    }

    void Update()
    {
        // �������Ɉړ�
        Vector3 horizontalMovement = new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        // �㉺�ɕ�������
        Vector3 floatOffset = new Vector3(0, Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, 0);

        // �V�����ʒu��ݒ�
        transform.position = startPosition + floatOffset + horizontalMovement;

        // X���W���X�V���ăX�N���[���𑱂���
        startPosition.x += moveSpeed * Time.deltaTime;
    }
}
