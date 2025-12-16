using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    public float speed = 1.5f;

    Rigidbody2D rb;

    //시간 관련
    float lastScoreTime = -999f;
    float scoreCooldown = 0.1f;

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
        speed += 0.05f;
    }

    void ScoreUp()
    {
        if (Time.time - lastScoreTime < scoreCooldown)
            return;

        lastScoreTime = Time.time;

        GameManager.Instance.Score++;
    }

    public void InitSpeed()
    {
        speed = 1.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bar")
        {
            Vector2 normal = collision.contacts[0].normal;

            Vector2 incoming = rb.linearVelocity.normalized;

            Vector2 reflected = Vector2.Reflect(incoming, normal);

            float pushStrength = 2f;
            Vector2 finalDir = (reflected + normal * pushStrength).normalized;

            rb.linearVelocity = finalDir * speed;

            AudioManager.Instance.Play("Collider");

            ScoreUp();
        }

        if (collision.gameObject.tag == "GameEnd")
        {
            this.gameObject.SetActive(false);

            this.gameObject.transform.position = new Vector2(0, 1);

            AudioManager.Instance.Play("GameOver");

            GameManager.Instance.GameEnd();
        }




    }


}
