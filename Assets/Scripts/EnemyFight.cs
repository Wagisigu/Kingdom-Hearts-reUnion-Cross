using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyFight : MonoBehaviour, IPointerClickHandler
{

    private Keyblade keyblade;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        keyblade = GameObject.FindObjectOfType<Keyblade>();
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        keyblade.Attack(this);
    }

    public float Hit(float strike)
    {
        health -= strike;
        return health;
    }
}
