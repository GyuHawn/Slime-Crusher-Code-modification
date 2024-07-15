using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class StageManager : MonoBehaviour, Observer
{
    private MonsterSpawn monsterSpawn;
    private SelectItem selectItem;
    private SelectPass selectPass;
    private StageTimeLimit stageTimeLimit;
    private PlayerController playerController;
    private CharacterSkill characterSkill;
    private Character character;
    private ItemSkill itemSkill;
    private StageStatusManager stageStatusManager;
    private StageChange stageChange;
    private Combo combo;
    private GameTimeUI gameTimeUI;

    public bool gameStart = false; // ���ӽ��� ����

    public GameObject[] map;
    public int mainStage; // ���ν������� (1, 2, 3...)
    public int subStage; // ���꽺������ (1-1, 1-2...)
    private bool isNextStageAvailable = true;
    public TMP_Text stageText;

    public int base0Monster; // �������� ���� [0]�� ��
    public int base1Monster; // �������� ���� [1]�� ��
    public int base2Monster; // �������� ���� [2]�� ��
    public int base3Monster; // �������� ���� [3]�� ��
    public int bossMonster; // �������� ��
    public bool allMonstersSpawned = false; // ��� ���� ��ȯ Ȯ��

    public float timeLimit; // ���������� ���ѽð�

    public bool selectingPass; // �нú� ����������

    public float totalTime; // �� �ð�
    public int rewardMoney; // ȹ�� �Ӵ�
    public TMP_Text totalTimeText;
    public TMP_Text rewardMoneyText;
    public TMP_Text finalWaveText;
    public TMP_Text maxComboText;
    public TMP_Text clearText;

    public bool passing; // �������� ��ȯ ǥ�� ��

    public int comboNum;
    public int maxComboNum;

    public Canvas canvas;

    private void Awake()
    {
        if (!monsterSpawn)
            monsterSpawn = FindObjectOfType<MonsterSpawn>();
        if (!selectItem)
            selectItem = FindObjectOfType<SelectItem>();
        if (!selectPass)
            selectPass = FindObjectOfType<SelectPass>();
        if (!stageTimeLimit)
            stageTimeLimit = FindObjectOfType<StageTimeLimit>();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
        if (!characterSkill)
            characterSkill = FindObjectOfType<CharacterSkill>();
        if (!character)
            character = FindObjectOfType<Character>();
        if (!itemSkill)
            itemSkill = FindObjectOfType<ItemSkill>();
        if (!stageStatusManager)
            stageStatusManager = FindObjectOfType<StageStatusManager>();
        if (!stageChange)
            stageChange = FindObjectOfType<StageChange>();
        if (!combo)
            combo = FindObjectOfType<Combo>();

        if (gameTimeUI != null)
        {
            gameTimeUI.RegisterObserver(FindObjectOfType<GameUIManager>());
        }
    }

    void Start()
    {
        // 1-1 ���� ���� �� ���� ����
        if (!gameStart)
        {
            InitialStageSetting(); // �������� �ʹݼ���
            StageMonsterSetting();
            SpawnMonsters();
        }       
    }
    void InitialStageSetting() // �������� �ʹ� ����
    {
        mainStage = 1;
        subStage = 1;
        selectingPass = false;
        gameStart = true;

        totalTime = 0;
        rewardMoney = 0;
    }

    void ClearSetting() // Ŭ���� �� ����
    {
        clearText.text = "���� Ŭ����!!";
        clearText.fontSize = 40;
        Time.timeScale = 0f;
        Reward();
        gameStart = false;
        playerController.gameover.SetActive(true);
    }
    void Update()
    {
        // ���� Ŭ����
        if(mainStage > 20)
        {
            ClearSetting(); // Ŭ���� �� ����
        }

        UpdateMap(); // �������� �� ����
        UpdateStageText(); // �������� ǥ�� �ؽ�Ʈ ����
        TimeOver(); // ���ѽð� �ʰ��� ��������
        GameOver();  // �������� ��� ǥ��, ����
        StageAudio(); // �������������   
    }
    void UpdateMap() // �������� �� ����
    {
        for (int i = 0; i < Mathf.Min(8, mainStage); ++i)
        {
            map[i].SetActive(i == mainStage - 1);
        }
    }

    void UpdateStageText() // �������� ǥ�� �ؽ�Ʈ ����
    {
        if (mainStage <= 7)
        {
            stageText.text = mainStage <= 7 ? $"�������� {mainStage}-{subStage}" : $"�������� {mainStage}";
        }
    }

    void TimeOver() // ���ѽð� �ʰ��� ��������
    {     
        if (stageTimeLimit.stageFail >= stageTimeLimit.stageTime)
        {
            playerController.gameover.SetActive(true);
            gameStart = false;
        }
    }


    public void UpdateTime(float gameTime)
    {
        totalTime = gameTime; // ���� �ð��� �����Ͽ� totalTime���� ����
    }
    public void UpdateCombo(int combo, int maxCombo)
    {
        maxComboNum = maxCombo;
    }
    void GameOver()  // �������� ��� ǥ��, ����
    {
        totalTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(totalTime / 60), totalTime % 60);
        finalWaveText.text = mainStage < 8 ? $"{mainStage} - {subStage} Wave" : $"{mainStage} Wave";
        maxComboText.text = maxComboNum.ToString();
        rewardMoneyText.text = rewardMoney.ToString();
    }

    // ������ �Ӵ� ȹ�� (��� �Ӵ� ������ ȹ��)
    public void Reward()
    {
        if (gameStart)
        {
            rewardMoney += (int)totalTime + maxComboNum;
            int playerMoney = PlayerPrefs.GetInt("GameMoney", 0) + rewardMoney;
            PlayerPrefs.SetInt("GameMoney", playerMoney);
            PlayerPrefs.Save();
        }       
    }

    // �������� ���� �� ����
    public void StageMonsterSetting()
    {
        if (mainStage <= 7) // 1~7 �������� ����
        {          
            switch (subStage)
            {
                case 1:
                    base0Monster = 2;
                    break;
                case 2:
                    base0Monster = 2;
                    base1Monster = 2;
                    break;
                case 3:
                    base0Monster = 2;
                    base1Monster = 2;
                    base2Monster = 3;
                    break;
                case 4:
                    base0Monster = 2;
                    base1Monster = 2;
                    base2Monster = 3;
                    base3Monster = 3;
                    break;
                case 5:
                    base0Monster = 1;
                    base1Monster = 2;
                    base2Monster = 3;
                    base3Monster = 3;
                    bossMonster = 1;
                    break;
            }
        }
        else  // 8 �������� ����
        {    
            base0Monster = 2;
            base1Monster = 1;
            base2Monster = 1;
            base3Monster = mainStage - 7; // 8 �������� ���ĺ��� ���� ������ ���� ��  
            bossMonster = 1;
        }
    }

    // ���� �� �ʱ�ȭ
    void NextMonsterNumSetting()
    {
        base0Monster = 1;
        base1Monster = 0;
        base2Monster = 0;
        base3Monster = 0;
        bossMonster = 0;
        
        stageTimeLimit.stageFail = 0f;
    }

    // ���� ����
    void SpawnMonsters()
    {
        monsterSpawn.MonsterInstantiate(base0Monster, base1Monster, base2Monster, base3Monster, bossMonster);
        allMonstersSpawned = true;
    }

    // �нú� ����
    void SelectPass()
    {
        selectPass.passMenu.SetActive(true);
        playerController.isAttacking = true;
        Time.timeScale = 0f;
    }

    // �������� ����
    public void NextStage()
    {
        if (isNextStageAvailable)
        {
            if (mainStage <= 20)
            {
                allMonstersSpawned = false;
                characterSkill.CharacterCoolTime();
                ResetStageState();
                NextStageCharacterReset(); // �������� ��ȯ�� ĳ���� ���� ����
                NextStageSetting(); // �������� ��ȯ ����
                NextSetting();
            }
            StartCoroutine(DelayNextStage());
        }
    }
    void NextStageCharacterReset() // �������� ��ȯ�� ĳ���� ���� ����
    {
        if (character.currentCharacter == 2)
        {
            characterSkill.useWaterSkill = false;
        }
    }
    void NextStageSetting() // �������� ��ȯ ����
    {
        if (mainStage < 8)
        {
            NextSubStage();

            if (mainStage >= 2)
            {
                if (subStage == 2)
                {
                    selectingPass = true;
                    SelectPass();
                }
            }

            if (subStage == 3)
            {
                selectItem.ItemSelect();
                StartCoroutine(DelayStage());
            }
            else if (subStage > 5)
            {
                subStage = 1;
                NextMainStage();

                selectItem.ItemSelect();
                StartCoroutine(DelayStage());
            }
        }
        else
        {
            NextMainStage();
            if (mainStage == 12 || mainStage == 15 || mainStage == 18)
            {
                selectingPass = true;
                SelectPass();
            }
            else if (mainStage == 10 || mainStage == 16)
            {
                selectItem.ItemSelect();
                StartCoroutine(DelayStage());
            }
        }
    }

    void NextSetting()
    {
        if (mainStage >= 8)
        {
            if (mainStage == 8 || mainStage == 10 || mainStage == 12 || mainStage == 15 || mainStage == 16 || mainStage == 18)
            { }
            else
            {
                passing = true;
                NextStageInforSettingReady();
            }
        }
        else
        {
            if ((mainStage >= 2 && (subStage == 1 || subStage == 2)) || subStage == 3)
            { }
            else
            {
                passing = true;
                NextStageInforSettingReady();
            }
        }
    }

    // �������� ������ �ʱ�ȭ �� ����
    public void NextStageInforSettingReady()
    {
        if (mainStage <= 20 && passing)
        {
            StartCoroutine(NextStageInforSetting()); // �������� ���� ����
        }
    }
    
    IEnumerator NextStageInforSetting() // �������� ���� ����
    {
        stageTimeLimit.stageFail = 0f;
        yield return new WaitForSeconds(0.1f);

        stageChange.ChangeStageText();
        passing = false;
        yield return new WaitForSeconds(3f);

        stageStatusManager.ResetStatus(); // ���� �ʱ�ȭ

        NextMonsterNumSetting(); // �������� �̵��� ���ͼ� �ʱ�ȭ
        StageMonsterSetting(); // �������� ���� �� ����
        SpawnMonsters(); // ���� ����

        stageStatusManager.BuffStatus(allMonstersSpawned); // ���� ����
    }

    // �������� ���� �� �ʱ�ȭ �ð� 
    IEnumerator DelayNextStage()
    {
        isNextStageAvailable = false;
        yield return new WaitForSeconds(1f); 
        isNextStageAvailable = true; 
    }

    // �ʿ� �����ִ� ���� �������� ������ ����
    void ResetStageState()
    {
        GameObject[] skills = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject skill in skills)
        {
            if (skill.name == "BossSkill" || skill.name == "PlayerSkill" || skill.name == "MonsterAttack" || skill.name == "HealthUpItem")
            {
                Destroy(skill);
            }
        }

        itemSkill.holyWaving = false;
        playerController.stage5Debuff = false;
    }

    // �������� Ŭ����� �Ӵ� ȹ��
    void NextSubStage()
    {
        subStage++;
        rewardMoney += 10;
    }

    void NextMainStage()
    {
        mainStage++;
        rewardMoney += 100;
    }

    IEnumerator DelayStage()
    {
        yield return new WaitForSeconds(1f);
    }  

    void StageAudio()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (mainStage <= 8)
            {
                if (subStage == 5)
                {
                    AudioManager.Instance.PlayBossAudio();
                }
                else
                {
                    AudioManager.Instance.PlayStageAudio();
                }
            }
            else
            {
                AudioManager.Instance.PlayBossAudio();
            }
        }
    }
}
