using UnityEngine;
using TMPro;
public class MudarTextNaTela : MonoBehaviour
{
    [SerializeField]
    public FighterStats Char;
    //public TextMeshProUGUI Name;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI SP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {

        //Name.text = Char.Name;
        HP.text = Char.Hp.ToString()+ "/" + Char.HpMax.ToString();
        SP.text = Char.Sp.ToString() + "/" + Char.SpMax.ToString();
    }
}
