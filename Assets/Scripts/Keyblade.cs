using UnityEngine;

public class Keyblade : MonoBehaviour
{
    public GameObject player;
    public GameObject target;
    public float enemeyHealth;
    public float[] medals;
    public TMPro.TextMeshProUGUI healthText;

    private bool spin;
    private float lastZAngle;
    private float currentZAngle;
    private int medalNum;
    private bool slain;
    private float a;
    // Start is called before the first frame update
    void Start()
    {
        spin = false;
        slain = false;
        lastZAngle = 45f;
        currentZAngle = 45f;
        medalNum = 0;
        a = 255f;
        Debug.Log($"Enemy has {enemeyHealth} remaining");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !spin)
        {
            spin = true;
            a = 255f;
            enemeyHealth -= medals[medalNum];
            healthText.color = new Color(255f, 0f, 0f, 255f);
            healthText.transform.localPosition = new Vector3(230f, 30f, 0f);
            healthText.text = $"-{medals[medalNum]}";
            if (enemeyHealth > 0) Debug.Log($"Enemy has {enemeyHealth} remaining");
            else
            {
                Debug.Log("Enemy slain");
                slain = true;
                healthText.color = new Color(0f, 0f, 0f, 0f);
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
        if (a > 0f)
        {
            healthText.transform.Translate(0f, 25f * Time.deltaTime, 0f);
            healthText.color = Color.Lerp(healthText.color, new Color(255f, 0f, 0f, 0f), 5f * Time.deltaTime);
            a = healthText.color.a;
        }
    }
}
