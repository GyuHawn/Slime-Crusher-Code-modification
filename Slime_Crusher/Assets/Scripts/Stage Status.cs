using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageStatus : MonoBehaviour
{
    private StageTimeLimit stageTimeLimit;
    private PlayerController playerController;
    private ItemSkill itemSkill;
    private StageManager stageManager;

    public int buff; // ����, ����� ����
    public int status; // ���� ���� ����
    public Image stageStatus; 
    public GameObject statusPos;
    public int saveDamage; // ���ݷ� ���� ���� ������

    // ����
    public GameObject damageUp; // �⺻������ ����
    public bool isDamageUp; // �⺻������ ����
    public GameObject monsterHealthDown; // ���� ü�� ����
    public bool isMonsterHealthDown; // ���� ü�� ����
    public GameObject timeUp; // ���ѽð� ����
    public bool isTimeUp; // ���ѽð� ����
    public GameObject percentUp; // Ȯ�� ����
    public bool isPercentUp; // Ȯ�� ����
    public GameObject monsterDie; // �����ð� ���� ���� ����
    private float timer = 0f;
    private float delay = 10f;
    private List<GameObject> buffList = new List<GameObject>(); // ���� ����Ʈ

    // �����
    public GameObject damageDown; // �⺻������ ����
    public bool isDamageDown; // �⺻������ ����
    public GameObject monsterHealthUP; // ���� ü�� ����
    public bool isMonsterHealthUP; // ���� ü�� ����
    public GameObject timeDown; // ���ѽð� ����
    public bool isTimeDown; // ���ѽð� ����
    public GameObject percentDown; // Ȯ������
    public bool isPercentDown; // Ȯ������
    public GameObject monsterAttackSpdUp; // ����ü �ӵ� ����
    public bool isMonsterAttackSpdUp; // ����ü �ӵ� ����
    public GameObject monsterSizeDown;// ���� ũ�� ����
    public bool isMonsterSizeDown;// ���� ũ�� ����
    private List<GameObject> deBuffList = new List<GameObject>(); // ����� ����Ʈ

    private GameObject selectedEffect; // ���õ� ����
    public TMP_Text buffText;

    private void Awake()
    {
        stageTimeLimit = GameObject.Find("Manager").GetComponent<StageTimeLimit>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    void Start()
    {
        // ����Ʈ�� �߰�
        // ����
        buffList.Add(damageUp);
        buffList.Add(monsterHealthDown);
        buffList.Add(timeUp);
        buffList.Add(percentUp);
        buffList.Add(monsterDie);

        // �����
        deBuffList.Add(damageDown);
        deBuffList.Add(monsterHealthUP);
        deBuffList.Add(timeDown);
        deBuffList.Add(percentDown);
        deBuffList.Add(monsterAttackSpdUp);
        deBuffList.Add(monsterSizeDown);

        // ���� �������
        isDamageUp = false;
        isMonsterHealthDown = false;
        isTimeUp = false;
        isPercentUp = false;
        isDamageDown = false;
        isMonsterHealthUP = false;
        isTimeDown = false;
        isPercentDown = false;
        isMonsterAttackSpdUp = false;
        isMonsterSizeDown = false;
    }

    void Update()
    {
        // ���� ����
        if (buff == 1)
        {
            if (status == 1)
            {
                buffText.text = "�÷��̾��� �⺻ ���ݷ��� �����մϴ�.";
                if (!isDamageUp)
                {
                    DamageUP();
                }
            }
            else if (status == 2)
            {
                buffText.text = "������ �⺻ ü���� �����մϴ�.";
                if (!isMonsterHealthDown)
                {
                    MonsterHealthDown();
                }
            }
            else if (status == 3)
            {
                 buffText.text = "�������� ���� �ð��� �����մϴ�.";
                if (!isTimeUp)
                {
                    TimeUp();
                }
            }
            else if (status == 4)
            {
                buffText.text = "������ �ߵ� Ȯ���� �����մϴ�.";
                if (!isPercentUp)
                {
                    PercentUp();
                }
            }
            else if (status == 5)
            {
                buffText.text = "���� �ð����� ���Ͱ� ����մϴ�.";
                timer += Time.deltaTime;

                if (timer >= delay)
                {
                    MonsterDie();

                    timer = 0f;
                }
            }
        }
        else if (buff == 2) // ����� ����
        {
            if (status == 1)
            {
                buffText.text = "�÷��̾��� �⺻ ���ݷ��� �����մϴ�.";
                if (!isDamageDown)
                {
                    DamageDown();
                }
            }
            else if (status == 2)
            {
                buffText.text = "������ �⺻ ü���� �����մϴ�.";
                if (!isMonsterHealthUP)
                {
                    MonsterHealthUP();
                }
            }
            else if (status == 3)
            {
                buffText.text = "�������� ���� �ð��� �����մϴ�.";
                if (!isTimeDown)
                {
                    TimeDown();
                }
            }
            else if (status == 4)
            {
                buffText.text = "������ �ߵ� Ȯ���� �����մϴ�.";
                if (!isPercentDown)
                {
                    PercentDown();
                }
            }
            else if (status == 5)
            {
                buffText.text = "����ü �ӵ��� �����մϴ�.";
                if (!isMonsterAttackSpdUp)
                {
                    MonsterAttackSpdUp();
                }
            }
            else if (status == 6)
            {
                buffText.text = "������ ����� �����մϴ�.";
                if (!isMonsterSizeDown)
                {
                    MonsterSizeDown();
                }
            }
        }
    }

    // ����
    // �⺻����������
    public void DamageUP()
    {
        isDamageUp = true;
        saveDamage = playerController.damage;
        playerController.damage += (int)(playerController.damage * 0.5f);
    }
    // �⺻���������� ����
    public void ResetDamageUP()
    {
        isDamageUp = true;
        playerController.damage = saveDamage;
    }

    // ���� ü�� ����
    public void MonsterHealthDown()
    {
        isMonsterHealthDown = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach(GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            monsterController.currentHealth -= (monsterController.currentHealth * 0.3f);
        }
    }

    // ���ѽð� ����
    public void TimeUp()
    {
        isTimeUp = true;
        stageTimeLimit.stageTime += 10;
    }
    // ���ѽð� ���� ����
    public void ResetTimeUp()
    {
        isTimeUp = true;
        stageTimeLimit.stageTime -= 10;
    }

    // Ȯ�� ����
    public void PercentUp()
    {
        isPercentUp = true;

        itemSkill.firePercent += 5f;
        itemSkill.fireShotPercent += 5f;
        itemSkill.holyShotPercent += 5f;
        itemSkill.holyWavePercent += 5f;
        itemSkill.rockPercent += 5f;
        itemSkill.posionPercent += 5f;
        itemSkill.meleePercent += 5f;
        itemSkill.sturnPercent += 5f;
    }
    // Ȯ�� ���� ����
    public void ResetPercentUp()
    {
        isPercentUp = true;

        itemSkill.firePercent -= 5f;
        itemSkill.fireShotPercent -= 5f;
        itemSkill.holyShotPercent -= 5f;
        itemSkill.holyWavePercent -= 5f;
        itemSkill.rockPercent -= 5f;
        itemSkill.posionPercent -= 5f;
        itemSkill.meleePercent -= 5f;
        itemSkill.sturnPercent -= 5f;
    }

    // �����ð� ���� ���� ����
    public void MonsterDie()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null && monsterController.currentHealth > 0)
            {
                monsterController.currentHealth = 0;
                break;
            }
        }
    }

    // �����
    // �⺻������ ����
    public void DamageDown()
    {
        isDamageDown = true;
        saveDamage = playerController.damage;
        playerController.damage -= (int)(playerController.damage * 0.5f);
    }
    // �⺻������ ���� ����
    public void ResetDamageDown()
    {
        isDamageDown = true;
        playerController.damage = saveDamage;
    }

    // ���� ü�� ����
    public void MonsterHealthUP()
    {
        isMonsterHealthUP = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            monsterController.currentHealth = (monsterController.currentHealth * 1.5f);
        }
    }

    // ���ѽð� ����
    public void TimeDown()
    {
        isTimeDown = true;
        stageTimeLimit.stageTime -= 10;
    }
    // ���ѽð� ���� ����
    public void ResetTimeDown()
    {
        isTimeDown = true;
        stageTimeLimit.stageTime += 10;
    }

    // Ȯ������
    public void PercentDown()
    {
        isPercentDown = true;

        itemSkill.firePercent -= 5f;
        itemSkill.fireShotPercent -= 5f;
        itemSkill.holyShotPercent -= 5f;
        itemSkill.holyWavePercent -= 5f;
        itemSkill.rockPercent -= 5f;
        itemSkill.posionPercent -= 5f;
        itemSkill.meleePercent -= 5f;
        itemSkill.sturnPercent -= 5f;
    }
    // Ȯ������ ����
    public void ResetPercentDown()
    {
        isPercentDown = true;

        itemSkill.firePercent += 5f;
        itemSkill.fireShotPercent += 5f;
        itemSkill.holyShotPercent += 5f;
        itemSkill.holyWavePercent += 5f;
        itemSkill.rockPercent += 5f;
        itemSkill.posionPercent += 5f;
        itemSkill.meleePercent += 5f;
        itemSkill.sturnPercent += 5f;
    }

    // ����ü �ӵ� ����
    public void MonsterAttackSpdUp()
    {
        isMonsterAttackSpdUp = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonoBehaviour[] scripts = monster.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                if (script.GetType().Name.Contains("Stage"))
                {
                    System.Reflection.FieldInfo field = script.GetType().GetField("bulletSpd");

                    if (field != null)
                    {
                        field.SetValue(script, (float)field.GetValue(script) + 1);
                    }
                }
            }
        }
    }

    // ���� ũ�� ����
    public void MonsterSizeDown()
    {
        isMonsterSizeDown = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            Transform monsterTransform = monster.transform;
            Vector3 newScale = monsterTransform.localScale - new Vector3(0.05f, 0.05f, 0f);
            monsterTransform.localScale = newScale;
        }
    }

    // ���� ����
    public void BuffStatus(bool execution) 
    {
        if(execution)
        {
            List<GameObject> selectedList = (Random.Range(0, 2) == 0) ? buffList : deBuffList;

            if(selectedList == buffList)
            {
                buff = 1;
                Buff();
            }
            else if(selectedList == deBuffList)
            {
                buff = 2;
                DeBuff();
            }

            if (selectedList.Count > 0)
            {
                int randomIndex = Random.Range(0, selectedList.Count);

                status = randomIndex + 1;

                selectedEffect = selectedList[randomIndex];

                selectedEffect.transform.position = statusPos.transform.position;
            }
        }
    }

    // ���� ����
    public void ResetStatus()
    {
        if (selectedEffect != null)
        {
            selectedEffect.transform.position = new Vector3(100, 1500, 0);

            if (buff == 1)
            {
                if (status == 1)
                {
                    if (isDamageUp)
                    {
                        ResetDamageUP();
                    }
                }
                else if (status == 3)
                {
                    if (isTimeUp)
                    {
                        ResetTimeUp();
                    }
                }
                else if (status == 4)
                {
                    if (isPercentUp)
                    {
                        ResetPercentUp();
                    }
                }
            }
            else if (buff == 2)
            {
                if (status == 1)
                {
                    if (isDamageDown)
                    {
                        ResetDamageDown();
                    }
                }
                else if (status == 3)
                {
                    if (isTimeDown)
                    {
                        ResetTimeDown();
                    }
                }
                else if (status == 4)
                {
                    if (isPercentDown)
                    {
                        ResetPercentDown();
                    }
                }
            }

            isDamageUp = false;
            isMonsterHealthDown = false;
            isTimeUp = false;
            isPercentUp = false;
            isDamageDown = false;
            isMonsterHealthUP = false;
            isTimeDown = false;
            isPercentDown = false;
            isMonsterAttackSpdUp = false;
            isMonsterSizeDown = false;

            buff = 0;
            status = 0;

            selectedEffect = null;
        }
    }

    // ����, ����� �ð��� ǥ��
    void Buff()
    {
        stageStatus.color = new Color(0f, 0.49f, 1f);
    }

    void DeBuff()
    {
        stageStatus.color = Color.red;
    }
}
