using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSkill : MonoBehaviour
{
    private SelectItem selectItem;
    private PlayerController playerController;
    private Character character;
    private CharacterSkill characterSkill;

    // fire
    public GameObject fireInstance;
    public GameObject fireEffect;
    public float fireDamage;
    public float fireDamagePercent;
    public float fireDuration;
    public float firePercent;
    public bool isFire;

    // fireShot
    public GameObject fireShotEffect;
    public GameObject fireShotSub;
    public int fireShotSubNum;
    public float fireShotDamage;
    public float fireShotDamagePercent;
    public float fireShotSubDamage;
    public float fireShotSubDamagePercent;
    public float fireShotPercent;
    public bool isFireShot;

    // holyWave
    public GameObject WaveInstance;
    public GameObject holyWaveEffect;
    public Transform holyWavePos;
    public bool holyWaving;
    public float holyWaveDuration;
    public float holyWaveDamage;
    public float holyWaveDamagePercent;
    public float holyWavePercent;
    public bool isHolyWave;

    // holyShot
    public GameObject holyShotInstance;
    public GameObject holyShotEffect;
    public float holyShotDuration;
    public float holyShotDamage;
    public float holyShotDamagePercent;
    public float holyShotPercent;
    public bool isHolyShot;

    // melee
    public GameObject meleeEffect;
    public int meleeNum;
    public float meleePercent;
    public bool isMelee;

    // posion
    public GameObject posionEffect;
    public float posionDuration;
    public float poisonDamage;
    public float poisonDamagePercent;
    public float posionPercent;
    public bool isPosion;

    // rock
    public GameObject rockEffect;
    public float rockDamage;
    public float rockDamagePercent;
    public float rockPercent;
    public bool isRock;

    // sturn
    public GameObject sturnEffect;
    public GameObject sturnImage;
    public float sturnDuration;
    public float sturnPercent;
    public bool isSturn;
    public GameObject currentAttackedMonster; // ���õ� ����
    public Dictionary<GameObject, GameObject> monsterToSturnImage = new Dictionary<GameObject, GameObject>();

    private I_Skill fire;
    private I_Skill fireShot;
    private I_Skill holyWave;
    private I_Skill holyShot;
    private I_Skill posion;
    private I_Skill rock;
    private I_Skill sturn;


    private void Awake()
    {
        if (!selectItem)
            selectItem = FindObjectOfType<SelectItem>();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
        if (!character)
            character = FindObjectOfType<Character>();
        if (!characterSkill)
            characterSkill = FindObjectOfType<CharacterSkill>();
    }

    void Start()
    {
        // ��� ����
        holyWaving = false;

        fire = new I_Fire(this);
        fireShot = new I_FireShot(this);
        holyWave = new I_HolyWave(this);
        holyShot = new I_HolyShot(this);
        posion = new I_Posion(this);
        rock = new I_Rock(this);
        sturn = new I_Sturn(this);
    }

    private void Update()
    {
        UpdateDamagePercents(); // ������ �ۼ�Ʈ ������Ʈ
        UpdateDamage(); // ������ ������Ʈ
        UpdateSkillCounts(); // ��ų ���� ������Ʈ
        UpdateSkillDurations(); // ���ӽð� ������Ʈ
        UpdateSkillPercents(); // �ߵ� Ȯ�� ������Ʈ
    }

    void UpdateDamagePercents() // ������ �ۼ�Ʈ ������Ʈ
    {
        fireDamagePercent = 0.4f + (0.1f * selectItem.fireLv);
        fireShotDamagePercent = 1.3f + (0.2f * selectItem.fireShotLv);
        fireShotSubDamagePercent = 0.3f + (0.2f * selectItem.fireShotLv);
        holyWaveDamagePercent = 0.25f + (0.05f * selectItem.holyWaveLv);
        holyShotDamagePercent = 0.5f + (0.2f * selectItem.holyShotLv);
        poisonDamagePercent = 0.35f + (0.05f * selectItem.posionLv);
        rockDamagePercent = 1.5f + (0.5f * selectItem.rockLv);
    }
    void UpdateDamage() // ������ ������Ʈ
    {
        fireDamage = (playerController.damage + playerController.comboDamage) * fireDamagePercent;
        fireShotDamage = (playerController.damage + playerController.comboDamage) * fireShotDamagePercent;
        fireShotSubDamage = (playerController.damage + playerController.comboDamage) * fireShotSubDamagePercent;
        holyWaveDamage = (playerController.damage + playerController.comboDamage) * holyWaveDamagePercent;
        holyShotDamage = (playerController.damage + playerController.comboDamage) * holyShotDamagePercent;
        poisonDamage = (playerController.damage + playerController.comboDamage) * poisonDamagePercent;
        rockDamage = (playerController.damage + playerController.comboDamage) * rockDamagePercent;
    }
    void UpdateSkillCounts() // ��ų ���� ������Ʈ
    {
        fireShotSubNum = 2 + (1 * selectItem.fireShotLv);
        meleeNum = 2 + (1 * selectItem.meleeLv);
    }
    void UpdateSkillDurations() // ���ӽð� ������Ʈ
    {
        fireDuration = 2.5f + (0.5f * selectItem.fireLv);
        holyShotDuration = 1.5f + (0.5f * selectItem.holyShotLv);
        holyWaveDuration = 3.5f + (0.5f * selectItem.holyWaveLv);
        posionDuration = 5f;
        sturnDuration = 2f + (1 * selectItem.sturnLv);
    }
    void UpdateSkillPercents() // �ߵ� Ȯ�� ������Ʈ
    {     
        if (character.currentCharacter == 4)
        {
            firePercent = 10f + (0.5f * characterSkill.luckLevel);
            fireShotPercent = 20f + (0.5f * characterSkill.luckLevel);
            holyShotPercent = 10f + (0.5f * characterSkill.luckLevel);
            holyWavePercent = 5f + (0.5f * characterSkill.luckLevel);
            rockPercent = 30f + (0.5f * characterSkill.luckLevel);
            posionPercent = 10f + (0.5f * characterSkill.luckLevel);
            meleePercent = 60f + (0.5f * characterSkill.luckLevel);
            sturnPercent = 30f + (0.5f * characterSkill.luckLevel);
        }
        else
        {
            firePercent = 10f;
            fireShotPercent = 20f;
            holyShotPercent = 10f;
            holyWavePercent = 5f;
            rockPercent = 30f;
            posionPercent = 10f;
            meleePercent = 60f;
            sturnPercent = 30f;
        }
    }



    // ������ ȹ��
    public void GetItem()
    {
        // Ư�� ������Ʈ�� ã�Ƽ� null�� �ƴϸ� true ��ȯ
        isFire = GameObject.Find("FirePltem") != null;
        isFireShot = GameObject.Find("Fire ShotPltem") != null;
        isHolyWave = GameObject.Find("Holy WavePltem") != null;
        isHolyShot = GameObject.Find("Holy ShotPltem") != null;
        isRock = GameObject.Find("RockPltem") != null;
        isPosion = GameObject.Find("PosionPltem") != null;
        isMelee = GameObject.Find("MeleePltem") != null;
        isSturn = GameObject.Find("SturnPltem") != null;
    }

    // fire
    public void Fire(Vector3 targetPosition)
    {
        fire.Execute(this, targetPosition, 0);    
    }

    // fireShot
    public void FireShot(Vector3 targetPosition)
    {
        fireShot.Execute(this, targetPosition, fireShotSubNum);
    } 

    // holyWave 
    public void HolyWave()
    {
        holyWave.Execute(this, new Vector3(0,0,0), 0);     
    }
    public void ExecuteHolyWave()
    {
        StartCoroutine(DestroyWave());
    }
    IEnumerator DestroyWave()
    {
        yield return new WaitForSeconds(holyWaveDuration);
        Destroy(WaveInstance);
        holyWaving = false;
    }

    // holyShot 

    public void HolyShot(Vector3 targetPosition)
    {
        holyShot.Execute(this, targetPosition, 0);
    }

    public void ExecuteHolyShot()
    {
        StartCoroutine(RotateHolyShot(holyShotInstance, 5f));
    }

    IEnumerator RotateHolyShot(GameObject holyShot, float duration)
    {
        if (holyShot == null)
        {
            yield break;
        }

        float elapsedTime = 0f;
        float rotationSpd = 360f / duration;

        while (elapsedTime < duration)
        {
            if (holyShot == null)
            {
                yield break;
            }

            holyShot.transform.Rotate(rotationSpd * Time.deltaTime, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    // melee 
    public void Melee(Vector3 targetPosition, int num)
    {
        StartCoroutine(MeleeInstantiate(targetPosition, num));
    }

    public IEnumerator MeleeInstantiate(Vector3 targetPosition, int numEffects)
    {
        for (int i = 0; i < numEffects; i++)
        {
            AudioManager.Instance.PlayMeleeAudio();

            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            Vector3 spawnPosition = targetPosition + randomOffset;

            GameObject meleeInstance = Instantiate(meleeEffect, spawnPosition, Quaternion.identity);
            meleeInstance.name = "PlayerSkill";
            Destroy(meleeInstance, 0.2f);

            yield return new WaitForSeconds(0.1f);
        }
    }

    // posion 
    public void Posion(Vector3 targetPosition)
    {
        posion.Execute(this, targetPosition, 0);
    }

    // rock 
    public void Rock(Vector3 targetPosition)
    {
        rock.Execute(this, targetPosition, 0);
    }  

    // sturn 
    public void Sturn()
    {
        rock.Execute(this, new Vector3(0, 0, 0), 0);
    }

    public void SetCurrentAttackedMonster(GameObject monster) // ���� ���ݹ޴� ���� ����
    {
        currentAttackedMonster = monster;
    }

    // �������� ���� ����� ���� �̹����� ����
    public void DestroyMonster(GameObject monster)
    {
        if (monsterToSturnImage.ContainsKey(monster))
        {
            Destroy(monsterToSturnImage[monster]);
            monsterToSturnImage.Remove(monster);
        }

        Destroy(monster);
    }
}
