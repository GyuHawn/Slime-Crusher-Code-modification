using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeLimit : MonoBehaviour
{
    private StageManager stageManager;

    public Image timeImage; // ���ѽð� �ð��� ������ �̹���

    public float stageTime; // ���ѽð�
    public float stageFail; // ����� �ð�
    public TMP_Text timeText; // �ð� �ؽ�Ʈ

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    private void Start()
    {
        stageTime = 20; // ���ѽð� ����
    }

    void Update()
    {
        // ���� �ð� ǥ��
        timeText.text = ((int)(stageTime - stageFail)).ToString();

        UpdateTime(); // �ð� �ð��� ������Ʈ
    }

    void UpdateTime() // �ð� �ð��� ������Ʈ
    {
        if (stageManager.gameStart)
        {
            if (!stageManager.selectingPass)
            {
                // �ð� ����
                if (stageFail > stageTime)
                {
                    stageFail = 0.0f;
                    timeImage.fillAmount = 0.0f;
                }
                else
                {
                    stageFail = stageFail + Time.deltaTime;
                    timeImage.fillAmount = 1.0f - (Mathf.Lerp(0, 100, stageFail / stageTime) / 100);
                }
            }
        }
    }
}
