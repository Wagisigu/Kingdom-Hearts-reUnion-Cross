using UnityEngine;

public class Keyblade : MonoBehaviour
{
    public GameObject player;
    public GameObject target;
    public float medal1;
    public float medal2;
    public float medal3;
    public float medal4;
    public float medal5;
    public float enemeyHealth;

    private bool spin;
    private float lastZAngle;
    private float currentZAngle;
    private int medalNum;
    private float[] medalStrength;
    private bool slain;
    // Start is called before the first frame update
    void Start()
    {
        spin = false;
        slain = false;
        lastZAngle = 45f;
        currentZAngle = 45f;
        medalNum = 0;
        medalStrength = new float[] {
            medal1,
            medal2,
            medal3,
            medal4,
            medal5
        };
        Debug.Log($"Enemy has {enemeyHealth} remaining");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !spin)
        {
            spin = true;
            enemeyHealth -= medalStrength[medalNum];
            if (enemeyHealth > 0) Debug.Log($"Enemy has {enemeyHealth} remaining");
            else
            {
                Debug.Log("Enemy slain");
                slain = true;
            }
            medalNum = (medalNum + 1) % 5;
            player.transform.Translate(new Vector3(3f, 0f, 0f));
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
                player.transform.Translate(new Vector3(-3f, 0f, 0f));
                if (slain)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
            }
        }
    }
}
