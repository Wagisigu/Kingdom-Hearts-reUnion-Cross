using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    private Vector3 moveTo;
    private Vector3 playerYOffset;
    private bool move;
    // Start is called before the first frame update
    void Start()
    {
        playerYOffset = new Vector3(0.0f, ((BoxCollider2D)gameObject.GetComponent("BoxCollider2D")).offset.y, 0.0f);
        moveTo = transform.position + playerYOffset * 0.5f;
        move = 0 == SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (move) {
            //if (Input.touchCount > 0)
            if (Input.GetMouseButtonDown(0))
            {
                //Touch touch = Input.GetTouch(0);
                //Vector2 pos = touch.position;
                Vector3 pos = Input.mousePosition;
                pos = UnityEngine.Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                moveTo = new Vector3(pos.x, pos.y, 0.0f);
            }
            transform.position = Vector2.MoveTowards(transform.position, moveTo + new Vector3(0f, GetComponent<Renderer>().bounds.size.y * 2 / 5, 0f), 3.0f * Time.deltaTime);
            if (Vector3.Distance(transform.position, moveTo) < 0.001f)
            {
                // Swap the position of the cylinder.
                moveTo = transform.position;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (Mathf.Abs(transform.position.x - moveTo.x) < 0.001f)
        {
            moveTo = transform.position - playerYOffset;
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (Mathf.Abs(transform.position.x - moveTo.x) < 0.001f)
        {
            moveTo = transform.position - playerYOffset;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(1);
    }
}
