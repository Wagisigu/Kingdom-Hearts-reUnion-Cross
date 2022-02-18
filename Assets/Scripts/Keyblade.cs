using UnityEngine;
using UnityEngine.SceneManagement;

public class Keyblade : MonoBehaviour
{
    public bool turn;

    private bool spin;
    private float lastZAngle;
    private float currentZAngle;
    // Start is called before the first frame update
    void Start()
    {
        spin = false;
        lastZAngle = 45f;
        currentZAngle = 45f; 
        turn = 1 == SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (turn && Input.GetMouseButtonUp(0) && !spin)
        {
            spin = true;
        }
        if (spin)
        {
            float rot = -90f * Time.deltaTime;
            currentZAngle += rot;
            transform.Rotate(0f, 0f, rot, Space.Self);
            if (Mathf.Abs(lastZAngle-currentZAngle)>=72f)
            {
                spin = false;
                transform.Rotate(0f, 0f, (Mathf.Abs(lastZAngle - currentZAngle) - 72));
                currentZAngle %= 360;
                lastZAngle = currentZAngle;
            }
        }
    }
}
