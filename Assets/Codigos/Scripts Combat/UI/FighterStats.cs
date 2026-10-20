using UnityEngine;
using UnityEngine.UI;
using System;

public class FighterStats: MonoBehaviour, IComparable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    //private GameObject hpFill;
    //[SerializeField]
    //private GameObject spFill;
    [Header("Stats")]
    public string Name;
    public float Hp;
    public float HpMax;
    public float Sp;
    public float SpMax;
    public float ATT;
    public float DEF;
    public float SPEED;
    public float EXP;
    public float MAGIC;
    public float SPECIAL;
    [HideInInspector]
    public int nextActTurn;

    private bool dead = false;
    // resize
    private Transform HPTRANSFORM;
    private Transform SPTRANSFORM;

    private Vector2 HPScale;
    private Vector2 SPScale;

    //private float xNewHPScale;
    //private float xNewSPScale;

    private void Start()
    {
       // HPTRANSFORM = hpFill.GetComponent<RectTransform>();
       // HPScale = hpFill.transform.localScale;
       // SPTRANSFORM = spFill.GetComponent<RectTransform>();
       // SPScale = spFill.transform.localScale;
        Hp = HpMax;
        Sp = SpMax;
    }
    // public void CalculateNextTurn(int currentTurn) { nextActTurn = currentTurn + Mathf.CeilToInt(100f / SPEED); }
    public bool GetDead() { return dead; }
    public void HPMOD(float value) {
        Hp = Hp + value;
        animator.Play("DAMAGE");
        if (Hp > HpMax) { Hp = HpMax; }
        if (Hp <= 0) { Hp = 0; dead = true; gameObject.tag = "Dead"; animator.Play("Dying"); }
        else {/* xNewHPScale = HPScale.x * (Hp / HpMax); hpFill.transform.localScale = new Vector2(xNewHPScale, HPScale.y);*/ }
    }
    public void updateManaFill() { /*xNewSPScale = SPScale.x * (Sp / SpMax); spFill.transform.localScale = new Vector2(xNewSPScale, SPScale.y); */}
    public int CompareTo(object otherStats) {
        int nex = nextActTurn.CompareTo(((FighterStats)otherStats).nextActTurn); return nex; }
}
