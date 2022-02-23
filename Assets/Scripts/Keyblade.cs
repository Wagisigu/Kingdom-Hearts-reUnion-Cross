using UnityEngine;

public class Keyblade : MonoBehaviour
{
    public GameObject player;
    private GameObject[] targets;
    public float enemeyHealth;
    public float[] medals;
    public TMPro.TextMeshProUGUI healthText;
    public GameObject enemyPrefab;

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
        
        int tempCount = PlayerPrefs.GetInt("enemyCount");
        targets = new GameObject[tempCount];
        for (int i = 0; i < tempCount; i++)
        {
            Vector3 tempVect = new Vector3(0f, 0f, 0f);
            tempVect.x = PlayerPrefs.GetFloat("enemyX" + i);
            tempVect.y = PlayerPrefs.GetFloat("enemyY" + i);
            targets[i] = Instantiate(enemyPrefab, tempVect, Quaternion.identity);
        }
        healthText = targets[0].GetComponentInChildren<TMPro.TextMeshProUGUI>();
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
            healthText.transform.localPosition = new Vector3(0f, 2.5f, 0f);
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
                    if (targets.Length == 1) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    else
                    {
                        GameObject[] tempArray = new GameObject[targets.Length - 1];
                        System.Array.Copy(targets, 1, tempArray, 0, targets.Length - 1);
                        Destroy(targets[0]);
                        targets = tempArray;
                        healthText = targets[0].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                        slain = false;
                        enemeyHealth = 10;
                    }
                }
            }
        }
        if (a > 0f)
        {
            healthText.transform.Translate(0f, 1f * Time.deltaTime, 0f);
            healthText.color = Color.Lerp(healthText.color, new Color(255f, 0f, 0f, 0f), 5f * Time.deltaTime);
            a = healthText.color.a;
        }
    }
}
