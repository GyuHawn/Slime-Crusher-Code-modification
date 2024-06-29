using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    // ĳ���� ����
    public GameObject rockEx;
    public GameObject waterEx;
    public GameObject lightEx;
    public GameObject luckEx;

    // ĳ���� ���� ����
    public bool waterChar = false;
    public bool lightChar = false;
    public bool luckChar = false;

    // ĳ���� ���� ���� UI
    public GameObject useWater;
    public GameObject useLight;
    public GameObject useLuck;

    // ������ ĳ����
    public int selectChar;
    public GameObject[] checkChar;

    // �÷��̾� �� ����
    public int playerMoney;
    public TMP_Text playerMoneyText;
    
    // ĳ���� ���� UI
    public GameObject waterOpenBtn;
    public GameObject lightOpenBtn;
    public GameObject luckOpenBtn;

    // ĳ���� ������ UI
    public GameObject rockLevelUPButton;
    public int rockLevel;
    public TMP_Text rockLevelText;
    public GameObject waterLevelUPButton;
    public int waterLevel;
    public TMP_Text waterLevelText;
    public GameObject lightLevelUPButton;
    public int lightLevel;
    public TMP_Text lightLevelText;
    public GameObject luckLevelUPButton;
    public int luckLevel;
    public TMP_Text luckLevelText;

    // ���ӽ���
    public bool enter;
    public TMP_Text enterText;

    private void Start()
    {
        LoadPlayerDatas(); // ĳ���� ������ ����

        // ���ӽ��� ����
        enter = false;
        enterText.color = Color.black;
    }

    void LoadPlayerDatas() // ĳ���� ������ ����
    {
        waterChar = PlayerPrefs.GetInt("WaterCharOpen", 0) == 1;
        lightChar = PlayerPrefs.GetInt("LightCharOpen", 0) == 1;
        luckChar = PlayerPrefs.GetInt("LuckCharOpen", 0) == 1;

        rockLevel = PlayerPrefs.GetInt("rockLevel", 1);
        waterLevel = PlayerPrefs.GetInt("waterLevel", 1);
        lightLevel = PlayerPrefs.GetInt("lightLevel", 1);
        luckLevel = PlayerPrefs.GetInt("luckLevel", 1);
    }

    void Update()
    {
        UpdatePlayerMoney(); // �÷��̾� �Ӵ� ����
        SelectCharacterText(); // ĳ���� ���� ���� �ؽ�Ʈ
        UpdateOpenCharacter(); // ĳ���� ���� ����
        UpdateCharacterLevelText(); // ĳ���� ���� �ؽ�Ʈ
        MaxCharacterLevel(); // ĳ���� ���� ����
    }

    void UpdatePlayerMoney() // �÷��̾� �Ӵ� ����
    {
        playerMoney = PlayerPrefs.GetInt("GameMoney", 0);
        playerMoneyText.text = playerMoney.ToString();
    }

    void SelectCharacterText() // ĳ���� ���� ���� �ؽ�Ʈ
    {   
        if (!enter)
        {
            enterText.fontSize = 35f;
            enterText.text = "ĳ���͸�" + "\n" + "������ �ּ���.";
        }
        else
        {
            enterText.fontSize = 50f;
            enterText.text = "���ӽ���";
        }
    }

    void UpdateOpenCharacter() // ĳ���� ���� ����
    {
        if (waterChar)
        {
            useWater.SetActive(false);
            waterOpenBtn.SetActive(false);
            waterLevelUPButton.SetActive(true);
            waterLevelText.gameObject.SetActive(true);
        }
        if (lightChar)
        {
            useLight.SetActive(false);
            lightOpenBtn.SetActive(false);
            lightLevelUPButton.SetActive(true);
            lightLevelText.gameObject.SetActive(true);
        }
        if (luckChar)
        {
            useLuck.SetActive(false);
            luckOpenBtn.SetActive(false);
            luckLevelUPButton.SetActive(true);
            luckLevelText.gameObject.SetActive(true);
        }
    } 

    void UpdateCharacterLevelText() // ĳ���� ���� �ؽ�Ʈ
    {
        rockLevelText.text = rockLevel.ToString();
        waterLevelText.text = waterLevel.ToString();
        lightLevelText.text = lightLevel.ToString();
        luckLevelText.text = luckLevel.ToString();
    }

    void MaxCharacterLevel() // ĳ���� ���� ����
    {
        if (rockLevel >= 20)
        {
            rockLevelUPButton.SetActive(false);
        }
        if (waterLevel >= 20)
        {
            waterLevelUPButton.SetActive(false);
        }
        if (lightLevel >= 20)
        {
            lightLevelUPButton.SetActive(false);
        }
        if (luckLevel >= 20)
        {
            luckLevelUPButton.SetActive(false);
        }
    }

    void CheckReset() // ĳ���� ���� UI �ʱ�ȭ
    {
        foreach(GameObject check in checkChar)
        {
            check.SetActive(false);
        }
    }

    // ĳ���� ����
    public void RockChar() // �� ĳ����
    {
        CheckReset(); // ĳ���� ���� UI �ʱ�ȭ
        checkChar[0].SetActive(true); // ĳ���� ���� UI Ȱ��ȭ

        // �� ĳ���� ���� Ȱ��ȭ, ������ ��Ȱ��ȭ
        rockEx.SetActive(true);
        waterEx.SetActive(false);
        lightEx.SetActive(false);
        luckEx.SetActive(false);

        enter = true; // ���� ���� ���� ����
        selectChar = 1; // ������ ĳ���� ��
    }

    public void WaterChar()
    {
        if (waterChar)
        {
            CheckReset();
            checkChar[1].SetActive(true);

            waterEx.SetActive(true);
            rockEx.SetActive(false);
            lightEx.SetActive(false);
            luckEx.SetActive(false);

            enter = true;
            selectChar = 2;
        }
    }

    public void LightChar()
    {
        if (lightChar)
        {
            CheckReset();
            checkChar[2].SetActive(true);

            lightEx.SetActive(true);
            rockEx.SetActive(false);
            waterEx.SetActive(false);
            luckEx.SetActive(false);

            enter = true;
            selectChar = 3;
        }
    }

    public void LuckChar()
    {
        if (luckChar)
        {
            CheckReset();
            checkChar[3].SetActive(true);

            luckEx.SetActive(true);
            rockEx.SetActive(false);
            waterEx.SetActive(false);
            lightEx.SetActive(false);

            enter = true;
            selectChar = 4;
        }
    }

    // ĳ���� ����
    public void OpenWater() // �� ĳ���� ����
    {
        if (playerMoney >= 100) // 100�� �̻��϶�
        {
            waterChar = true; // ĳ���� ����
            playerMoney -= 100; // ����� �� ����
            PlayerPrefs.SetInt("WaterCharOpen", 1);
            PlayerPrefs.SetInt("GameMoney", playerMoney);
        }
    }

    public void OpenLight()
    {
        if (playerMoney >= 500)
        {
            lightChar = true;
            playerMoney -= 500;
            PlayerPrefs.SetInt("LightCharOpen", 1);
            PlayerPrefs.SetInt("GameMoney", playerMoney);
        }
    }

    public void OpenLuck()
    {
        if (playerMoney >= 1000)
        {
            luckChar = true;
            playerMoney -= 1000;
            PlayerPrefs.SetInt("LuckCharOpen", 1);
            PlayerPrefs.SetInt("GameMoney", playerMoney);
        }
    }

    // ĳ���� ������
    public void LevelUpRock() // �� ĳ���� ������
    {
        if (rockLevel <= 20) // ������ 20�����϶�
        {
            if (playerMoney >= 1000) // 1000�� �̻��϶�
            {
                rockLevel++; // ������
                playerMoney -= 1000; // ����� 1000�� ����
                PlayerPrefs.SetInt("rockLevel", rockLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }
    public void LevelUpWater()
    {
        if (waterLevel <= 20)
        {
            if (playerMoney >= 1000) 
            {
                waterLevel++;
                playerMoney -= 1000;
                PlayerPrefs.SetInt("waterLevel", waterLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }
    public void LevelUpLight()
    {
        if (lightLevel <= 20)
        {
            if (playerMoney >= 1000)
            {
                lightLevel++;
                playerMoney -= 1000;
                PlayerPrefs.SetInt("lightLevel", lightLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }
    public void LevelUpLuck()
    {
        if (luckLevel <= 20)
        {
            if (playerMoney >= 1000)
            {
                luckLevel++;
                playerMoney -= 1000;
                PlayerPrefs.SetInt("luckLevel", luckLevel);
                PlayerPrefs.SetInt("GameMoney", playerMoney);
            }
        }
    }

    // ���ӽ���
    public void GameStart()
    {
        if (enter) // ���ӽ��� �����϶�
        {
            enterText.color = Color.white; // �ؽ�Ʈ �ð�ȭ
            PlayerPrefs.SetInt("SelectChar", selectChar);
            StartCoroutine(GameStartButton()); // ���ӽ���
        }
        else
        {
            StartCoroutine(EnterTextColor()); // ĳ���� ���ý� �ؽ�Ʈ (���ۺҰ�)
        }
    }

    // ĳ���� ���ý� �ؽ�Ʈ (���ۺҰ�)
    IEnumerator EnterTextColor()
    {
        enterText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        enterText.color = Color.black;
    }

    // ĳ���� ���ý� �����̵�
    IEnumerator GameStartButton()
    {
        yield return new WaitForSeconds(1f); // ������ ���� 1�� ���

        LodingController.LoadNextScene("Game");
    }
}
