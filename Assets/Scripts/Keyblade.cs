using UnityEngine;
using System.Collections.Generic;

public class Keyblade : MonoBehaviour
{
    public GameObject player;
    public float[] medals;
    public TMPro.TextMeshProUGUI healthText;
    public List<TMPro.TextMeshProUGUI> healthTexts;
    public GameObject enemyPrefab;
    public float playerHealth;

    private List<GameObject> targets;
    private GameObject target;
    private bool spin = false;
    private float lastZAngle = 45f;
    private float currentZAngle = 45f;
    private int medalNum = 0;
    private bool slain = false;
    private EnemyFight e;
    private Vector2 defaultPos;
    private Vector2 enemyAttackPos;
    private bool playerTurnOver = false;
    private int attackingEnemy = 0;
    private bool enemyHasStriked = false;
    private float sinceStrikeStarted;
    private Vector2 lastEnemyPos;
    private float enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        healthTexts = new List<TMPro.TextMeshProUGUI>();
        defaultPos = player.transform.position;
        enemyAttackPos = defaultPos + new Vector2(3f, 0f);
        
        int tempCount = PlayerPrefs.GetInt("enemyCount");
        targets = new List<GameObject>();
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
        if (playerTurnOver)
        {
            if (!enemyHasStriked)
            {
                float hitStrength = targets[attackingEnemy].GetComponent<EnemyFight>().strength;
                playerHealth -= hitStrength;
                Debug.Log($"Player has {playerHealth} remaining");
                healthText = player.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                healthText.color = new Color(255f, 0f, 0f, 255f);
                healthText.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                healthText.text = $"-{hitStrength}";
                healthTexts.Add(healthText);
                sinceStrikeStarted = 0;
                enemyHasStriked = true;
                lastEnemyPos = targets[attackingEnemy].transform.position;
                targets[attackingEnemy].transform.position = enemyAttackPos;

            }
            else
            {
                sinceStrikeStarted += Time.deltaTime;
                if (sinceStrikeStarted >= 2)
                {
                    targets[attackingEnemy].transform.position = lastEnemyPos;
                    enemyHasStriked = false;
                    if (attackingEnemy == targets.Count - 1)
                    {
                        attackingEnemy = 0;
                        playerTurnOver = false;
                    }
                    else
                    {
                        attackingEnemy++;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) && !spin)
            {
                spin = true;
                if (e == null) e = targets[0].GetComponent<EnemyFight>();
                enemyHealth = e.Hit(medals[medalNum]);
                target = e.gameObject;
                healthText = target.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                healthText.color = new Color(255f, 0f, 0f, 255f);
                healthText.transform.localPosition = new Vector3(0f, 2.5f, 0f);
                healthText.text = $"-{medals[medalNum]}";
                if (enemyHealth > 0) Debug.Log($"Enemy has {enemyHealth} remaining");
                else
                {
                    Debug.Log("Enemy slain");
                    slain = true;
                }
                medalNum = (medalNum + 1) % 5;
                player.transform.position = e.gameObject.transform.position;
                player.transform.Translate(new Vector3(-1f, 0f, 0f));
                healthTexts.Add(healthText);
            }
            if (spin)
            {
                float rot = -90f * Time.deltaTime;
                currentZAngle += rot;
                transform.Rotate(0f, 0f, rot, Space.Self);
                if (Mathf.Abs(lastZAngle - currentZAngle) >= 72f)
                {
                    playerTurnOver = medalNum == 4;
                    spin = false;
                    transform.Rotate(0f, 0f, (Mathf.Abs(lastZAngle - currentZAngle) - 72));
                    currentZAngle %= 360;
                    lastZAngle = currentZAngle;
                    player.transform.position = defaultPos;
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
