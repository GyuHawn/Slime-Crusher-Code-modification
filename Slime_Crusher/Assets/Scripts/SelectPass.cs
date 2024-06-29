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

    public int selecPass; // ������ �нú�

    // �нú� �̸�, ����
    public TMP_Text passName;
    public TMP_Text passEx;

    private void Awake()
    {
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        stageTimeLimit = GameObject.Find("Manager").GetComponent<StageTimeLimit>();
    }

    private void Start()
    {
        selecPass = 0; // ������ �нú�
    }

    // �нú� ����, ����ǥ��
    public void powerUP()
    {
        selecPass = 1;
        passName.text = "���ݷ� ���";
        passEx.text = "�÷��̾��� ���ݷ��� ���˴ϴ�.";
    }
    public void TimeUP()
    {
        selecPass = 2;
        passName.text = "�ð� �߰�";
        passEx.text = "���������� ���ѽð��� Ȯ��˴ϴ�.";
    }
    public void fullHealth()
    {
        selecPass = 3;
        passName.text = "ü��ȸ��";
        passEx.text = " ü���� ������ ȸ���˴ϴ�.";
    }
    public void GetMoney()
    {
        selecPass = 4;
        passName.text = "�Ӵ� ȹ��";
        passEx.text = "���� �Ӵϸ� 100�� ȹ���մϴ�.";
    }

    void UpdateSelectPass() // ������ �нú��� �� ������Ʈ
    {
        if (selecPass == 1)
        {
            playerController.damage += 5;
        }
        else if (selecPass == 2)
        {
            stageTimeLimit.stageTime += 5;
        }
        else if (selecPass == 3)
        {
            playerController.playerHealth = 8;
        }
        else if (selecPass == 4)
        {
            PlayerPrefs.SetInt("GameMoney", PlayerPrefs.GetInt("GameMoney", 0) + 100);
        }
    }

    // �нú� ���� ����
    public void EnterPass()
    {
        UpdateSelectPass(); // ������ �нú��� �� ������Ʈ

        stageManager.selectingPass = false; // �нú� ���������� ���� (�нú� ������ - ��)
        playerController.isAttacking = false; // �ٽ� ���� ���� (������ ���� ����)
        Time.timeScale = 1f; // �ð��� �ٽ� ���� (������ �ð� ����)
        selecPass = 0; // ������ �нú� �� �ʱ�ȭ
        passName.text = ""; // �нú� �̸�, ���� �ʱ�ȭ
        passEx.text = "";

        stageManager.passing = true; // �������� ����
        stageManager.NextSetting();

        passMenu.SetActive(false); // UI ��Ȱ��ȭ
    }
}
