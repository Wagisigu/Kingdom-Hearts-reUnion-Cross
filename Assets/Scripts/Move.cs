using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour
{
    public ContactFilter2D movementFilter;

    private Animator animator;
    private Vector2 moveTo;
    private bool move = false;
    private Rigidbody2D rb;
    private float moveSpeed = 2.0f;

    private List<RaycastHit2D> castCollsions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        moveTo = rb.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //if (Input.touchCount > 0)
        if (Input.GetMouseButtonDown(0))
        {
            //Touch touch = Input.GetTouch(0);
            //Vector2 pos = touch.position;
            Vector3 pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
            pos.z = 0;
            moveTo = new Vector2(pos.x, pos.y);
            move = true;
            animator.SetBool("isWalking", true);
            if (moveTo.x - rb.position.x > 0) transform.localScale = new Vector3(-1, 1, 1);
            else transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        if (move)
        {
            moveCharacter((moveTo - rb.position));
            if (Vector2.Distance(rb.position, moveTo) < 0.1f)
            {
                // Swap the position of the cylinder.
                move = false;
                animator.SetBool("isWalking", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int tempCount = other.GetComponentInParent<Enemy>().count;
        PlayerPrefs.SetInt("enemyCount", tempCount);
        for (int i = 0; i < tempCount; i++)
        {
            PlayerPrefs.SetFloat("enemyX" + i, other.GetComponentInParent<Enemy>().positions[i].x);
            PlayerPrefs.SetFloat("enemyY" + i, other.GetComponentInParent<Enemy>().positions[i].y);
        }
        SceneManager.LoadScene(1);
    }

    private bool moveCharacter(Vector2 distance)
    {
        Vector2 direction = distance.normalized;
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollsions,
            moveSpeed * Time.fixedDeltaTime * 0.05f);

        if (count == 0)
        {
            Vector2 movementVector = direction * moveSpeed * Time.fixedDeltaTime;
            if (distance.magnitude < movementVector.magnitude) movementVector = distance;

            rb.MovePosition(rb.position + movementVector);
            return true;
        }
        return false;
    }
}
