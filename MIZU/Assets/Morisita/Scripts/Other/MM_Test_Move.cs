using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Test_Move : MonoBehaviour
{
    [SerializeField]
    float x, y, z;
    [SerializeField]
    float _LimitXSpeed;
    [SerializeField]
    float _LimitYSpeed;

    [SerializeField]
    Vector3 _velocity;
    // Start is called before the first frame update
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       _velocity=_rb.velocity;
        _rb.AddForce(new(x, y, z), ForceMode.Acceleration);
        LimitedSpeed();
        
    }
    void LimitedSpeed()
    {
        // ���x�����A����𒴂��������܂ŉ�����
        if (GetAbsSpeed().x > _LimitXSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x / (GetAbsSpeed().x / _LimitXSpeed), _rb.velocity.y, _rb.velocity.z);
        }
        if (GetAbsSpeed().y > _LimitYSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y / (GetAbsSpeed().y / _LimitYSpeed), _rb.velocity.z);
        }

        // �v�Z�ł��؂�A���ȉ��Ȃ�0�ɂ���
        if (GetAbsSpeed().x < 1)
            _rb.velocity = new Vector3(0, _rb.velocity.y, _rb.velocity.z);
    }
    public Vector2 GetAbsSpeed()
    {
        var velo = _rb.velocity;

        velo.x = Mathf.Sqrt(Mathf.Pow(_rb.velocity.x, 2));
        velo.y = Mathf.Sqrt(Mathf.Pow(_rb.velocity.y, 2));

        return velo;
    }
}
