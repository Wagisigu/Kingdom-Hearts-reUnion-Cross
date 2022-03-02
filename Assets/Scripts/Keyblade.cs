using UnityEngine;
using System.Collections.Generic;

public class Keyblade : MonoBehaviour
{
    public GameObject player;
    public float enemeyHealth;
    public float[] medals;
    public TMPro.TextMeshProUGUI healthText;
    public List<TMPro.TextMeshProUGUI> healthTexts;
    public GameObject enemyPrefab;

    private List<GameObject> targets;
    private GameObject target;
    private bool spin;
    private float lastZAngle;
    private float currentZAngle;
    private int medalNum;
    private bool slain;
    private EnemyFight e;
    // Start is called before the first frame update
    void Start()
    {
        healthTexts = new System.Collections.Generic.List<TMPro.TextMeshProUGUI>();
        spin = false;
        slain = false;
        lastZAngle = 45f;
        currentZAngle = 45f;
        medalNum = 0;
        Debug.Log($"Enemy has {enemeyHealth} remaining"); 
        
        int tempCount = PlayerPrefs.GetInt("enemyCount");
        targets = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < tempCount; i++)
        {
            Vector3 tempVect = new Vector3(0f, 0f, 0f);
            tempVect.x = PlayerPrefs.GetFloat("enemyX" + i);
            tempVect.y = PlayerPrefs.GetFloat("enemyY" + i);
            targets.Add(Instantiate(enemyPrefab, tempVect, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !spin)
        {
            spin = true;
            if (e == null)
            {
                EnemyFight ef = targets[0].GetComponent<EnemyFight>();
                enemeyHealth = ef.Hit(medals[medalNum]);
                target = targets[0];
            }
            else
            {
                enemeyHealth = e.Hit(medals[medalNum]);
                target = e.gameObject;
            }
            healthText = target.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            healthText.color = new Color(255f, 0f, 0f, 255f);
            healthText.transform.localPosition = new Vector3(0f, 2.5f, 0f);
            healthText.text = $"-{medals[medalNum]}";
            if (enemeyHealth > 0) Debug.Log($"Enemy has {enemeyHealth} remaining");
            else
            {
                Debug.Log("Enemy slain");
                slain = true;
            }
            medalNum = (medalNum + 1) % 5;
            player.transform.Translate(new Vector3(3f, 0f, 0f));
            healthTexts.Add(healthText);
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
                    if (targets.Count == 1) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    else
                    {
                        target.gameObject.layer--;
                        target.gameObject.GetComponent<Renderer>().enabled = false;
                        targets.Remove(target);
                        slain = false;
                    }
                }
            }
            e = null;
        }
        for (int i = healthTexts.Count - 1; i >= 0; i--)
        {
            healthText = healthTexts[i];
            healthText.transform.Translate(0f, 0.3f * Time.deltaTime, 0f);
            healthText.color = Color.Lerp(healthText.color, new Color(255f, 0f, 0f, 0f), 2f * Time.deltaTime);
            if (healthText.color.a <= 0)
            {
                healthTexts.RemoveAt(i);
                Destroy(healthText.gameObject);
            }
        }
    }

    public void Attack(EnemyFight enemyFight)
    {
        e = enemyFight;
    }
}
