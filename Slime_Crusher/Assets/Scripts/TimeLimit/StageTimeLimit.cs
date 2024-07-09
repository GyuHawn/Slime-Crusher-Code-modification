using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeLimit : MonoBehaviour
{
    public StageManager stageManager;

    public Image timeImage; // ���ѽð� �ð��� ������ �̹���

    public float stageTime; // ���ѽð�
    public float stageFail; // ����� �ð�
    public TMP_Text timeText; // �ð� �ؽ�Ʈ

    private StageState currentState;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    private void Start()
    {
        stageTime = 20; // ���ѽð� ����
        SetState(new GameRunningState()); // �ʱ� ���� ����
    }

    void Update()
    {
        // ���� �ð� ǥ��
        timeText.text = ((int)(stageTime - stageFail)).ToString();

        if (stageManager.gameStart)
        {
            currentState.UpdateTime(this); // ���� ���¿� ���� �ð� ������Ʈ
        }
    }

    public void SetState(StageState newState)
    {
        currentState = newState;
    }
}
