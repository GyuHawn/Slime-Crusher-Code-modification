using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectPass : MonoBehaviour
{
    private PlayerController playerController;
    private StageManager stageManager;
    private StageTimeLimit stageTimeLimit;

    public GameObject passMenu; // �нú� ���� UI

    public int selectedPass; // ������ �нú�

    // �нú� �̸�, ����
    public TMP_Text passNameText;
    public TMP_Text passExText;

    private void Awake()
    {
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
        if (!stageManager)
            stageManager = FindObjectOfType<StageManager>();
        if (!stageTimeLimit)
            stageTimeLimit = FindObjectOfType<StageTimeLimit>();
    }


    private void Start()
    {
        selectedPass = 0; // ������ �нú�
    }

    // �нú� ����, ����ǥ��
    public void powerUP()
    {
        selectedPass = 1;
        passNameText.text = "���ݷ� ���";
        passExText.text = "�÷��̾��� ���ݷ��� ���˴ϴ�.";
    }
    public void TimeUP()
    {
        selectedPass = 2;
        passNameText.text = "�ð� �߰�";
        passExText.text = "���������� ���ѽð��� Ȯ��˴ϴ�.";
    }
    public void fullHealth()
    {
        selectedPass = 3;
        passNameText.text = "ü��ȸ��";
        passExText.text = " ü���� ������ ȸ���˴ϴ�.";
    }
    public void GetMoney()
    {
        selectedPass = 4;
        passNameText.text = "�Ӵ� ȹ��";
        passExText.text = "���� �Ӵϸ� 100�� ȹ���մϴ�.";
    }

    void UpdateSelectPass() // ������ �нú��� �� ������Ʈ
    {
        switch (selectedPass)
        {
            case 1:
                playerController.damage += 5;
                break;
            case 2:
                stageTimeLimit.stageTime += 5;
                break;
            case 3:
                playerController.playerHealth = 8;
                break;
            case 4:
                PlayerPrefs.SetInt("GameMoney", PlayerPrefs.GetInt("GameMoney", 0) + 100);
                break;
        }
    }

    // �нú� ���� ����
    public void EnterPass()
    {
        UpdateSelectPass(); // ������ �нú��� �� ������Ʈ

        stageManager.selectingPass = false; // �нú� ���������� ���� (�нú� ������ - ��)
        playerController.isAttacking = false; // �ٽ� ���� ���� (������ ���� ����)
        Time.timeScale = 1f; // �ð��� �ٽ� ���� (������ �ð� ����)
        selectedPass = 0; // ������ �нú� �� �ʱ�ȭ
        passNameText.text = ""; // �нú� �̸�, ���� �ʱ�ȭ
        passExText.text = "";

        stageManager.passing = true; // �������� ����
        stageManager.NextSetting();

        passMenu.SetActive(false); // UI ��Ȱ��ȭ
    }
}
