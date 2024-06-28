using System.Collections;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    private MonsterSpwan monsterSpawn;
    private SelectItem selectItem;
    private SelectPass selectPass;
    private StageTimeLimit stageTimeLimit;
    private PlayerController playerController;
    private CharacterSkill characterSkill;
    private Character character;
    private ItemSkill itemSkill;
    private StageStatus stageStatus;
    private StageChange stageChange;
    private Combo combo;

    public bool gameStart = false; // ���ӽ��� ����

    public GameObject[] map;
    public int mainStage; // ���ν������� (1, 2, 3...)
    public int subStage; // ���꽺������ (1-1, 1-2...)
    public TMP_Text stageText;
    private bool isNextStageAvailable = true;

    public int base0Monster; // �������� ���� [0]�� ��
    public int base1Monster; // �������� ���� [1]�� ��
    public int base2Monster; // �������� ���� [2]�� ��
    public int base3Monster; // �������� ���� [3]�� ��
    public int bossMonster; // �������� ��
    public bool allMonstersSpawned = false; // ��� ���� ��ȯ Ȯ��

    // public int monsterCount = 0; // ��ȯ�� ���� ��

    public float timeLimit; // ���������� ���ѽð�

    public bool selectingPass; // �нú� ����������
    
    public float totalTime; // �� �ð�
    public int rewardMoney; // ȹ�� �Ӵ�
    public TMP_Text totalTimeText;
    public TMP_Text rewardMoneyText;
    public TMP_Text finalWaveText;
    public TMP_Text maxComboText;
    public TMP_Text clearText;

    public bool passing; // �������� Ÿ�� ǥ�� ��

    public Canvas canvas;

    private void Awake()
    {
        monsterSpawn = GameObject.Find("Manager").GetComponent<MonsterSpwan>();
        selectItem = GameObject.Find("Manager").GetComponent<SelectItem>();
        selectPass = GameObject.Find("Manager").GetComponent<SelectPass>();
        stageTimeLimit = GameObject.Find("Manager").GetComponent<StageTimeLimit>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        characterSkill = GameObject.Find("Manager").GetComponent<CharacterSkill>();
        character = GameObject.Find("Manager").GetComponent<Character>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        stageStatus = GameObject.Find("Manager").GetComponent<StageStatus>();
        stageChange = GameObject.Find("Manager").GetComponent<StageChange>();
        combo = GameObject.Find("Manager").GetComponent<Combo>();
    }

    void Start()
    {
        // 1-1 ���� ���� �� ���� ����
        if (!gameStart)
        {
            mainStage = 1;
            subStage = 1;
            StageMonsterSetting();
            SpawnMonsters();
            selectingPass = false;
            gameStart = true;

            totalTime = 0;
            rewardMoney = 0;
        }   
    }

    void Update()
    {
        // ���� Ŭ����
        if(mainStage > 20)
        {
            clearText.text = "���� Ŭ����!!";
            clearText.fontSize = 40;
            Time.timeScale = 0f;
            Reward();
            gameStart = false;
            playerController.gameover.SetActive(true);
        }

        // �������� �� ����
        if (mainStage <= 8)
        {
            for (int i = 0; i < mainStage - 1; ++i)
            {
                map[i].SetActive(false);
            }

            map[mainStage - 1].SetActive(true);
        }
        else
        {
            for (int i = 0; i < 7; ++i)
            {
                map[i].SetActive(false);
            }

            map[7].SetActive(true);
        }

        // �������� ǥ�� �ؽ�Ʈ ����
        if (mainStage <= 7)
        {
            stageText.text = "�������� " + mainStage + "-" + subStage;
        }
        else if(mainStage >= 8)
        {
            stageText.text = "�������� " + mainStage;
        }

        // ���ѽð� �ʰ��� ��������
        if(stageTimeLimit.stageFail >= stageTimeLimit.stageTime)
        {
            playerController.gameover.SetActive(true);
            gameStart = false;
        }

        // �������� ��� ǥ��, ����
        totalTime = playerController.gameTime;
        totalTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(totalTime / 60), totalTime % 60);
        if(mainStage < 8)
        {
            finalWaveText.text = mainStage + " - " + subStage + " Wave";
        }
        else if(mainStage >= 8)
        {
            finalWaveText.text = mainStage + " Wave";
        }
        maxComboText.text = combo.maxComboNum.ToString();
        rewardMoneyText.text = rewardMoney.ToString();
    }

    // ������ �Ӵ� ȹ�� (��� �Ӵ� ������ ȹ��)
    public void Reward()
    {
        if (gameStart)
        {
            rewardMoney += (int)(totalTime * 1);
            rewardMoney += combo.maxComboNum;

            int playerMoney = PlayerPrefs.GetInt("GameMoney", 0);

            playerMoney += rewardMoney;

            PlayerPrefs.SetInt("GameMoney", playerMoney);
            PlayerPrefs.Save();
        }       
    }

    // �������� ���� �� ����
    public void StageMonsterSetting()
    {
        if (mainStage <= 7)
        {
            // 1~7 �������� ����
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
        else
        {
            // 8 �������� ����
            base0Monster = 2;
            base1Monster = 1;
            base2Monster = 1;
            base3Monster = mainStage - 7; // 8 �������� ���ĺ��� ���� ������ ���� ��  
            bossMonster = 1;
        }
    }

    // ���� �� �ʱ�ȭ
    void NextStageSetting()
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
        //monsterCount = base0Monster + base1Monster + base2Monster + base3Monster + bossMonster; // ���� �� ����
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

                if (character.currentCharacter == 2)
                {
                    characterSkill.useWaterSkill = false;
                }

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

                if (mainStage >= 8)
                {
                    if (mainStage == 8 || mainStage == 10 || mainStage == 12 || mainStage == 15 || mainStage == 16 || mainStage == 18)
                    {
                    }
                    else
                    {
                        passing = true;
                        NextSetting();
                    }
                }
                else
                {
                    if ((mainStage >= 2 && (subStage == 1 || subStage == 2)) || subStage == 3)
                    {
                    }
                    else
                    {
                        passing = true;
                        NextSetting();
                    }
                }

            }
            StartCoroutine(DelayNextStage());
        }
    }

    // �������� ���� �� �ʱ�ȭ �ð� 
    IEnumerator DelayNextStage()
    {
        isNextStageAvailable = false;
        yield return new WaitForSeconds(1f); 

        isNextStageAvailable = true; 
    }

    // �������� ������ �ʱ�ȭ �� ����
    public void NextSetting()
    {
        if(mainStage <= 20)
        {
            if (passing)
            {
                StartCoroutine(NextStageOrTile());
            }
        }
    }

    IEnumerator NextStageOrTile()
    {
        stageTimeLimit.stageFail = 0f;
        yield return new WaitForSeconds(0.1f);

        stageChange.ChangeStageText();
        passing = false;
        yield return new WaitForSeconds(3f);

        stageStatus.ResetStatus(); // ���� �ʱ�ȭ

        NextStageSetting(); // �������� �̵��� ���ͼ� �ʱ�ȭ
        StageMonsterSetting(); // �������� ���� �� ����
        SpawnMonsters(); // ���� ����

        stageStatus.BuffStatus(allMonstersSpawned); // ���� ����
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

        itemSkill.holyWave = false;
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
    /*
    //-----------[���� ����]------------
    #if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-4956833096962057~1338843571";
    #elif UNITY_IPHONE
      private string _adUnitId = "ca-app-pub-3940256099942544~1458002511";
    #else
      private string _adUnitId = "unused";
    #endif

    private InterstitialAd _interstitialAd;

    // ���� �ε�
    public void LoadInterstitialAd()
    {
        // ���� ���� ������ ����
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // ���� ��û ����
        AdRequest adRequest = new AdRequest();

        // ���� �ε� ��û
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load an ad with error: " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response: " + ad.GetResponseInfo());
            _interstitialAd = ad;

            RegisterEventHandlers(_interstitialAd);
        });
    }

    // ���� ǥ��
    public void ShowInterstitialAd()
    {
        StartCoroutine(showInterstitial());

        IEnumerator showInterstitial()
        {
            while (_interstitialAd == null && !_interstitialAd.CanShowAd())
            {
                yield return new WaitForSeconds(0.2f);
            }
            _interstitialAd.Show();
        }
        *//*if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }*//*
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // ���� ���� ��ü ȭ�� �������� �ݾ��� �� ȣ��˴ϴ�.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // ������ �� ���� �ٸ� ���� ������ �� �ֵ��� ���� �ٽ� �ε��մϴ�.
            LoadInterstitialAd();
        };

        // ���� ���� ��ü ȭ�� �������� ���� ������ �� ȣ��˴ϴ�.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);

            // ������ �� ���� �ٸ� ���� ������ �� �ֵ��� ���� �ٽ� �ε��մϴ�.
            LoadInterstitialAd();
        };
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            SceneManager.LoadScene("MainMenu"); // ������ �޴��� ���ư���
            LoadInterstitialAd(); // ���� ���� �� �� ���� �ε�
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error: " + error);
            LoadInterstitialAd(); // ���� ���� �� �� ���� �ε�
        };
    }

    // ���� ������ ���� ǥ�� 
    public void GameOver()
    {
        ShowInterstitialAd();
    }*/
}
