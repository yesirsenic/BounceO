using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    public float speed = 1.5f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        LaunchRandom();
        InvokeRepeating("VelocityIncrease", 1f, 1f);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    void LaunchRandom()
    {
        Vector2[] dirs =
        {
            new Vector2( 1, 0),
            new Vector2(-1, 0),
            new Vector2( 0, 1),
            new Vector2( 0,-1),
            new Vector2( 1, 1),
            new Vector2(-1, 1),
            new Vector2( 1,-1),
            new Vector2(-1,-1),
        };

        Vector2 dir = dirs[Random.Range(0, dirs.Length)].normalized;
        rb.linearVelocity = dir * speed;
    }

    void VelocityIncrease()
    {
        speed += 0.01f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        Vector2 incoming = rb.linearVelocity.normalized;

        Vector2 reflected = Vector2.Reflect(incoming, normal);

        float pushStrength = 2f;
        Vector2 finalDir = (reflected + normal * pushStrength).normalized;

        rb.linearVelocity = finalDir * speed;
    }


}
