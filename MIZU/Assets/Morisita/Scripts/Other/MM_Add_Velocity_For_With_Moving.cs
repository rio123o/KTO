using UnityEngine;

public class MM_Add_Velocity_For_With_Moving : MonoBehaviour
{
    // ëOÇ∆ÇÃPositionÇÃç∑ÇéÊÇ¡ÇƒÇ´ÇΩï˚Ç™ê≥ämÇ…ìÆÇ≠Ç¡Ç€Ç¢ÅH
    [SerializeField]
    private Rigidbody otherRigidbody;
    [SerializeField]
    private bool isOnMoveGround;
    [SerializeField]
    private Vector3 addVelocity;

    string MOVE_GROUND = "MoveGround";
    
    Vector3 oldPosition;
    Rigidbody _rb;
    float power = 20;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Init();
    }
    private void OnEnable()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (isOnMoveGround)
        {
            CalcAddVelocity();
            //_rb.AddForce(otherRigidbody.velocity*(1/Time.fixedDeltaTime), ForceMode.Acceleration);
            //_rb.AddForce(addVelocity.normalized * ((addVelocity.x - _rb.velocity.x) * power), ForceMode.Acceleration);
            _rb.MovePosition(_rb.position+AddVelocity()*Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("TriggerName=" + other.name);

        if (other.gameObject.CompareTag(MOVE_GROUND))
        {
            print("ê⁄ë±");
            isOnMoveGround = true;
            otherRigidbody = other.gameObject.GetComponent<MM_Get_Parent_Rigidbody>().rb;
            oldPosition = otherRigidbody.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("TriggerName=" + other.name);
        if (other.gameObject.CompareTag(MOVE_GROUND))
        {
            print("âèú");

            Init();
        }
    }

    void Init()
    {
        isOnMoveGround = false;
        addVelocity = Vector3.zero;
        otherRigidbody=null;
    }

    private void CalcAddVelocity()
    {
        //addVelocity = (otherRigidbody.position - oldPosition);
        addVelocity = (otherRigidbody.position - oldPosition) / Time.deltaTime;
        addVelocity = new(addVelocity.x, 0f, 0f);
        oldPosition = otherRigidbody.position;
    }

    public Vector3 AddVelocity()
    {
        return addVelocity;
    }
}
