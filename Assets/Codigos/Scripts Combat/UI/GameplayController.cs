using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameplayController : MonoBehaviour
{
    public bool TurnOrder;
    private List<FighterStats> fighterStats;
    [SerializeField]
    private GameObject battleMenu;
    [SerializeField]
    private GameObject Menu;

    public IEnumerator NextTurn()
    {
        TurnOrder = !TurnOrder;
        if (TurnOrder == false){ yield return new WaitForSeconds(1.0f); battleMenu.GetComponent<MenuFlutuante>()._indiceSelecionado = Random.Range( 0,  2); battleMenu.GetComponent<MenuFlutuante>().ConfirmarAcaoEnemy();  };
    }
}
