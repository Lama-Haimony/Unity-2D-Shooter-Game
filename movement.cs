using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] GameObject bullet;
    public Vector2 gunOffset;
    public float bulletSpeed = 10f;

    [Header("Player Settings")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpPower = 10f;

    bool moveLeft;
    bool moveRight;
    bool jump;
    bool canJump;

    [Header("Player Lives")]
    public GameObject[] lifeSquares;
    private int currentLives;

    public static bool movingRight;

    void Start()
    {
        canJump = true;
        moveLeft = false;
        moveRight = false;
        jump = false;
        currentLives = lifeSquares.Length;
    }

    void Update()
    {
        // حركة اليمين واليسار
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movingRight = true;
            moveRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else moveRight = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movingRight = false;
            moveLeft = true;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else moveLeft = false;

        // القفز
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jump = true;
            canJump = false;
        }

        // إطلاق الرصاصة
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = transform.position + new Vector3(movingRight ? gunOffset.x : -gunOffset.x, gunOffset.y, 0);
            GameObject newBullet = Instantiate(bullet, spawnPos, Quaternion.identity);
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.gravityScale = 0;
                bulletRb.velocity = new Vector2(movingRight ? bulletSpeed : -bulletSpeed, 0);
            }
        }
    }

    void FixedUpdate()
    {
        // حركة أفقية
        if (moveRight)
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else if (moveLeft)
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);

        // القفز
        if (jump)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            jump = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            canJump = true;
        }
        else if (collision.gameObject.CompareTag("enemy"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (currentLives > 0)
        {
            currentLives--;
            Destroy(lifeSquares[currentLives]);

            if (currentLives <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        // إعادة تحميل نفس المشهد
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
