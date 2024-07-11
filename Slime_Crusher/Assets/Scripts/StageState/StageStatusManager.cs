using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.Intrinsics;

public class StageStatusManager : MonoBehaviour
{
    public StageTimeLimit stageTimeLimit;
    public PlayerController playerController;
    private StageManager stageManager;
    public ItemSkill itemSkill;

    public int buff; // ����, ����� ����
    public int status; // ���� ���� ����
    public Image stageStatus; 
    public GameObject statusPos;

    // ����
    public bool isDamageUp; // �⺻������ ����
    public bool isMonsterHealthDown; // ���� ü�� ����
    public bool isTimeUp; // ���ѽð� ����
    public bool isPercentUp; // Ȯ�� ����
    public GameObject damageUp; // �⺻������ ����
    public GameObject monsterHealthDown; // ���� ü�� ����
    public GameObject timeUp; // ���ѽð� ����
    public GameObject percentUp; // Ȯ�� ����
    public GameObject monsterDie; // �����ð� ���� ���� ����
    private float timer = 0f;
    private float delay = 10f;
    private List<GameObject> buffList = new List<GameObject>(); // ���� ����Ʈ

    // �����
    public bool isDamageDown; // �⺻������ ����
    public bool isMonsterHealthUP; // ���� ü�� ����
    public bool isTimeDown; // ���ѽð� ����
    public bool isPercentDown; // Ȯ������
    public bool isMonsterAttackSpdUp; // ����ü �ӵ� ����
    public bool isMonsterSizeDown;// ���� ũ�� ����
    public GameObject damageDown; // �⺻������ ����
    public GameObject monsterHealthUP; // ���� ü�� ����
    public GameObject timeDown; // ���ѽð� ����
    public GameObject percentDown; // Ȯ������
    public GameObject monsterAttackSpdUp; // ����ü �ӵ� ����
    public GameObject monsterSizeDown;// ���� ũ�� ����
    private List<GameObject> deBuffList = new List<GameObject>(); // ����� ����Ʈ

    public GameObject selectedEffect; // ���õ� ����
    public TMP_Text buffText;

    private StageStatus currentStatus;

    private void Awake()
    {
        if (!stageTimeLimit)
            stageTimeLimit = FindObjectOfType<StageTimeLimit>();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
        if (!itemSkill)
            itemSkill = FindObjectOfType<ItemSkill>();
        if (!stageManager)
            stageManager = FindObjectOfType<StageManager>();
    }

    void Start()
    {
        // ���� ����Ʈ �߰�
        buffList.Add(damageUp);
        buffList.Add(monsterHealthDown);
        buffList.Add(timeUp);
        buffList.Add(percentUp);
        buffList.Add(monsterDie);

        // ����� ����Ʈ �߰�
        deBuffList.Add(damageDown);
        deBuffList.Add(monsterHealthUP);
        deBuffList.Add(timeDown);
        deBuffList.Add(percentDown);
        deBuffList.Add(monsterAttackSpdUp);
        deBuffList.Add(monsterSizeDown);

        // ���� ������� �ʱ�ȭ
        ResetState();
    }

    void Update()
    {
        if (buff == 1) // ���� ����
        {
            ApplyBuff();
        }
        else if (buff == 2) // ����� ����
        {
            ApplyDeBuff();
        }
    }

    void ApplyBuff() // ���� ����
    {
        switch (status)
        {
            case 1:
                if (!isDamageUp)
                {
                    isDamageUp = true;
                    currentStatus = new DamageUpStatus();
                }
                break;
            case 2:
                if (!isMonsterHealthDown)
                {
                    isMonsterHealthDown = true;
                    currentStatus = new MonsterHealthDownStatus();
                }
                break;
            case 3:
                if (!isTimeUp)
                {
                    isTimeUp = true;
                    currentStatus = new TimeUpStatus();
                }
                    break;
            case 4:
                if (!isPercentUp)
                {
                    isPercentUp = true;
                    currentStatus = new PercentUpStatus();
                }
                break;
            case 5:
                timer += Time.deltaTime;

                if (timer >= delay)
                {
                    currentStatus = new MonsterDieStatus();
                    timer = 0f;
                }
                break;
        }
        if (currentStatus != null)
        {
            currentStatus.Apply(this);
        }
    }

    void ApplyDeBuff() // ����� ����
    {
        if (currentStatus != null) return;

        switch (status)
        {
            case 1:
                if (!isDamageDown)
                {
                    isDamageDown = true;
                    currentStatus = new DamageDownStatus();
                }
                break;
            case 2:
                if (!isMonsterHealthUP)
                {
                    isMonsterHealthUP = true;
                    currentStatus = new MonsterHealthUPStatus();
                }
                break;
            case 3:
                if (!isTimeDown)
                {
                    isTimeDown = true;
                    currentStatus = new TimeDownStatus();
                }
                break;
            case 4:
                if (!isPercentDown)
                {
                    isPercentDown = true;
                    currentStatus = new PercentDownStatus();
                }
                break;
            case 5:
                if (!isMonsterAttackSpdUp)
                {
                    isMonsterAttackSpdUp = true;
                    currentStatus = new MonsterAttackSpdUpStatus();
                }
                break;
            case 6:
                if (!isMonsterSizeDown)
                {
                    isMonsterSizeDown = true;
                    currentStatus = new MonsterSizeDownStatus();
                }
                break;
        }
        if (currentStatus != null)
        {
            currentStatus.Apply(this);
        }
    }

    // ���� ����
    public void BuffStatus(bool execution) 
    {
        if (currentStatus != null) return;

        if (execution)
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
        if (currentStatus != null)
        {
            currentStatus.Reset(this);
            currentStatus = null;
            buffText.text = "";
            if (selectedEffect != null)
            {
                selectedEffect.transform.position = new Vector3(-300, -300, 1);
            }
        }
    }
    
    void ResetState() // ���� �ʱ�ȭ
    {
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
