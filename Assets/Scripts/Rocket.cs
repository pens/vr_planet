using UnityEngine;

public class Rocket : MonoBehaviour {
    public GameManager gameManager;
    public Vector3 target;
    Vector3 start;
    Vector3 dir;
    LineRenderer line;
    Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        gameManager.KillRocket(gameObject, other.name.StartsWith("Model"));
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = transform.GetChild(0).GetComponent<LineRenderer>();
        start = transform.localPosition;
        dir = transform.InverseTransformDirection(target - transform.position).normalized;
    }

    private void Update()
    {
        line.SetPosition(1, new Vector3(0, 0, -transform.TransformVector(transform.localPosition - start).magnitude));
        rb.velocity = rb.velocity.magnitude * transform.TransformDirection(dir);
        rb.AddRelativeForce(new Vector3(0, 0, gameManager.SPEED));
    }
}
