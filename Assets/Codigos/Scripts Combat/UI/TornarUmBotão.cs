using UnityEngine;
using UnityEngine.UI;
public class TornarUmBotão : MonoBehaviour
{
    [SerializeField]
    private bool physical;
    private GameObject player;
    void Start()
    {
        string temp = gameObject.name;
        gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallback(temp));
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void AttachCallback(string btn) {
        
        if (btn.CompareTo("ATTackBtn") == 0)
        {
            player.GetComponent<FighterAction>().SelectAttack("ATTack");
        }
        else if (btn.CompareTo("SpecialBtn") == 0)
        {
            player.GetComponent<FighterAction>().SelectAttack("Special");
        }
        else if (btn.CompareTo("ItemBtn") == 0)
        {
            player.GetComponent<FighterAction>().SelectAttack("Item");
        }
        else {
            Debug.Log("Run Button Pressed");
        }
    }
}
