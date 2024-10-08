using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Stage2 : MonoBehaviour
{
    private MonsterController monsterController;
    private MonsterSpawn monsterSpwan;

    public GameObject subBoss;
    public GameObject bossEffect;
    public float onSkillHealth;
    private bool onSkill = false;
    public GameObject skillPos;

    private void Awake()
    {
        monsterController = gameObject.GetComponent<MonsterController>();
        if (!monsterSpwan)
            monsterSpwan = FindObjectOfType<MonsterSpawn>();
    }
    void Start()
    {
        skillPos = GameObject.Find("Stage2BossSkill");
    }
    
    void Update()
    {
        if(monsterController.currentHealth <= onSkillHealth && !onSkill)
        {
            Stage2bossSkill();
        }
    }

    public void Stage2bossSkill()
    {
        onSkill = true;
        GameObject skill = Instantiate(bossEffect, skillPos.transform.position, Quaternion.identity);
        GameObject subInstantiate = Instantiate(subBoss, skillPos.transform.position, Quaternion.identity);
        monsterSpwan.spawnedMonsters.Add(subInstantiate);
    }
}
