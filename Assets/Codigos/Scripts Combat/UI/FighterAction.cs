using UnityEngine;
using UnityEngine.UI;

public class FighterAction : MonoBehaviour
{
    private GameObject enemy;
    private GameObject player;
    [SerializeField]
    private GameObject ATTackPrefab;
    [SerializeField]
    private GameObject SpecialPrefab;
    [SerializeField]
    private GameObject ItemPrefab;
    [SerializeField]
    private Sprite faceIcon;

    private GameObject currentAttack;
    private GameObject Attack;
    private GameObject SpecialAttack;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void SelectAttack(string btn) {
        GameObject victim = player;
        if (tag == "Player")
        {
            victim = enemy;
        }
        if (btn.CompareTo("ATTack") == 0) {
            
        }
        if (btn.CompareTo("Special") == 0)
        {
            Debug.Log("Special");
        }
        if (btn.CompareTo("Item") == 0)
        {
            Debug.Log("Item");
        }
    }

}
