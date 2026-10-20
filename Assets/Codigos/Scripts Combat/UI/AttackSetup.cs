using UnityEngine;

public class AttackSetup : MonoBehaviour
{
    public GameObject owner;
    [SerializeField]
    private string animationName;
    [SerializeField]
    private bool magicAttack;
    [SerializeField]
    private float magicCost;
    [SerializeField]
    private float MinATTMult;
    [SerializeField]
    private float MaxATTMult;
    [SerializeField]
    private float MinDefMult;
    [SerializeField]
    private float MaxDefMult;

    private FighterStats attackerStats;
    private FighterStats targetStats;
    private float damage = 0.0f;
    private float xMagicNewScale;
    private Vector2 magicScale;
    private void Start()
    {
        // magicScale = GameObject.Find("HeroMagicFill").GetComponent<RectTransform>().localScale;
    }
    public void Attack(GameObject victim, GameObject aggressor)
    {
        owner = aggressor;
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = owner.GetComponent<FighterStats>();
        if (attackerStats.Sp >= magicCost) {
            float multiplier = Random.Range(MinATTMult, MaxATTMult);
            damage = multiplier * attackerStats.ATT;
            if (magicAttack) {
                damage = multiplier * attackerStats.MAGIC;
                attackerStats.Sp = attackerStats.Sp - magicCost;
                attackerStats.updateManaFill();
            }
            float defenseMultiplier = Random.Range(MinDefMult, MaxDefMult);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.DEF));
            owner.GetComponent<Animator>().Play(animationName);
            targetStats.HPMOD(-damage);
        }
    }
    public void Special(GameObject victim, GameObject aggressor)
    {
        owner = aggressor;
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = owner.GetComponent<FighterStats>();
        if (attackerStats.MAGIC >= magicCost)
        {
            float multiplier = Random.Range(MinATTMult, MaxATTMult);
            attackerStats.updateManaFill();
            damage = multiplier * attackerStats.ATT;
            if (magicAttack)
            {
                damage = multiplier * attackerStats.MAGIC;
                attackerStats.MAGIC = attackerStats.MAGIC - magicCost;
            }
            float defenseMultiplier = Random.Range(MinDefMult, MaxDefMult);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.DEF));
            owner.GetComponent<Animator>().Play(animationName);
            targetStats.HPMOD(-damage);
        }
    }
    public void Item(GameObject victim, GameObject aggressor)
    {
        owner = aggressor;
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = owner.GetComponent<FighterStats>();
        if (attackerStats.MAGIC >= magicCost)
        {
            float multiplier = Random.Range(MinATTMult, MaxATTMult);
            attackerStats.updateManaFill();
            damage = multiplier * attackerStats.ATT;
            if (magicAttack)
            {
                damage = multiplier * attackerStats.MAGIC;
                attackerStats.MAGIC = attackerStats.MAGIC - magicCost;
            }
            float defenseMultiplier = Random.Range(MinDefMult, MaxDefMult);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.DEF));
            owner.GetComponent<Animator>().Play(animationName);
            targetStats.HPMOD(damage);
        }
    }
}
