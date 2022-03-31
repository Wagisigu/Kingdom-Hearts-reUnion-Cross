using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyFight : MonoBehaviour, IPointerClickHandler
{
    public float strength = 1f;

    private Keyblade keyblade;
    private float health = 10f;

    // Start is called before the first frame update
    void Start()
    {
        keyblade = GameObject.FindObjectOfType<Keyblade>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        keyblade.Attack(this);
    }

    public float Hit(float strike)
    {
        health -= strike;
        return health;
    }
}
