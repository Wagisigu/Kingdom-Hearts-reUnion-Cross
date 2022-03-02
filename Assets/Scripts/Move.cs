using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour
{
    public ContactFilter2D movementFilter;

    private Vector2 moveTo;
    private Vector2 playerYOffset;
    private Vector2 lastPosition;
    private bool move;
    private Rigidbody2D rb;
    private float moveSpeed = 5.0f;

    private List<RaycastHit2D> castCollsions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerYOffset = new Vector2(0.0f, ((CircleCollider2D)gameObject.GetComponent("CircleCollider2D")).offset.y)*0.5f;
        moveTo = rb.position + playerYOffset;
        lastPosition = moveTo;
        move = false;
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
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move)
        {
            Debug.Log("x");
            moveCharacter(((moveTo - playerYOffset) - rb.position));
            if (Vector2.Distance(rb.position, moveTo - playerYOffset) < 0.01f)
            {
                // Swap the position of the cylinder.
                move = false;
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
