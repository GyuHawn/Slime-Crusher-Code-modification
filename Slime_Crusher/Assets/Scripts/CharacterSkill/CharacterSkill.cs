using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    private ItemSkill itemSkill;
    private PlayerController playerController;

    // ĳ���� ����
    public int rockLevel;
    public int rockDamage;
    public int rockTime; // ��Ÿ��
    public GameObject rockCoolTime;
    public TMP_Text rockCoolTimeText;

    public bool useWaterSkill = false; // ��ų ������ 
    public int waterLevel;
    public int waterDamage;
    public int waterTime;
    public List<GameObject> MonsterList = new List<GameObject>();

    public GameObject waterEffect;
    public GameObject waterCoolTime;
    public TMP_Text waterCoolTimeText;

    public int sturnLevel;
    public float sturnDuration; // ���ӽð�
    public float sturnTime;
    public GameObject sturnCoolTime;
    public TMP_Text sturnCoolTimeText;

    public int luckLevel;

    public Dictionary<GameObject, GameObject> monsterToSturnImage = new Dictionary<GameObject, GameObject>(); // �� ���Ϳ� ���� �̹��� �Բ� ���� (���� ���Ž� �̹����� �Բ� ����)

    private C_Skill rockSkill;
    private C_Skill sturnSkill;
    private C_Skill waterSkill;
    
    private void Awake()
    {
        if (!itemSkill)
            itemSkill = FindObjectOfType<ItemSkill>();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    { 
        // ĳ���� ���� �޾ƿ���
        rockLevel = PlayerPrefs.GetInt("rockLevel", 1);
        waterLevel = PlayerPrefs.GetInt("waterLevel", 1);
        sturnLevel = PlayerPrefs.GetInt("lightLevel", 1);
        luckLevel = PlayerPrefs.GetInt("luckLevel", 1);

        rockSkill = new C_RockSkill(itemSkill, playerController);
        sturnSkill = new C_SturnSkill(itemSkill);
        waterSkill = new C_WaterSkill(itemSkill, playerController);
    }

    void Update()
    {
        SkillValueSetting(); // ��ų ��ġ ����
        SkillCoolTime(); // ��ų ��Ÿ��
    }

    void SkillValueSetting()
    {
        rockDamage = (int)((playerController.damage + playerController.comboDamage) + (playerController.damage * (0.1f * rockLevel)));
        waterDamage = (int)((playerController.damage + playerController.comboDamage) * (0.1f * waterLevel));
        sturnDuration = 3 + (0.1f * sturnLevel);
    }

    void SkillCoolTime() // ��ų ��Ÿ��
    {
        CoolTimeManager(rockTime, rockCoolTime, rockCoolTimeText);
        CoolTimeManager(waterTime, waterCoolTime, waterCoolTimeText);
        CoolTimeManager(sturnTime, sturnCoolTime, sturnCoolTimeText);
    }
    void CoolTimeManager(float time, GameObject coolTime, TMP_Text coolTimeText)
    {
        if(time > 0)
        {
            coolTime.SetActive(true);
            coolTimeText.text = time.ToString();
        }
    }


    // ��ų���
    public void Rock()
    {
        rockSkill.Execute(this);
    }

    // ��ų���
    public void Sturn()
    {
        sturnSkill.Execute(this);
    }

    // ��ų���
    public void Water()
    {
        waterSkill.Execute(this);
    }

    // ĳ���� ��ų�� ���� ������ Ȯ�������� ������ ���
    public void ItemSkill(MonsterController monsterController)
    {
        if (itemSkill.isFire && Random.Range(0f, 100f) <= itemSkill.firePercent)
        {
            itemSkill.Fire(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isFireShot && Random.Range(0f, 100f) <= itemSkill.fireShotPercent)
        {
            itemSkill.FireShot(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isHolyWave && Random.Range(0f, 100f) <= itemSkill.holyWavePercent)
        {
            itemSkill.HolyWave();
        }
        if (itemSkill.isHolyShot && Random.Range(0f, 100f) <= itemSkill.holyShotPercent)
        {
            itemSkill.HolyShot(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isPosion && Random.Range(0f, 100f) <= itemSkill.posionPercent)
        {
            itemSkill.Posion(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isRock && Random.Range(0f, 100f) <= itemSkill.rockPercent)
        {
            itemSkill.Rock(monsterController.gameObject.transform.position);
        }
        if (itemSkill.isSturn && Random.Range(0f, 100f) <= itemSkill.sturnPercent)
        {
            itemSkill.Sturn();
        }
    }

    // ��Ÿ�� ���� (�������� �̵��� ���)
    public void CharacterCoolTime()
    {
        if (rockTime > 0)
        {
            rockTime--;
        }
        else if (waterTime > 0)
        {
            waterTime--;
        }
        else if (sturnTime > 0)
        {
            sturnTime--;
        }
    }
}
