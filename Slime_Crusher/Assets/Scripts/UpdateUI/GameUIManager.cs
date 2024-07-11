using UnityEngine;
using TMPro;
using System.Collections;

public class GameUIManager : MonoBehaviour, Observer
{
    public TMP_Text gameTimeText; // ����ð� �ؽ�Ʈ
    public TMP_Text comboText; // �޺� �ؽ�Ʈ

    public int comboNum; // ���� �޺���
    public int maxComboNum; // �ִ� �޺���
    public float maxScale; // ũ�� ����
    public float minScale; // ũ�� ����
    public float scaleSpeed; // ���� �ӵ�

    private Coroutine scaleCoroutine;

    void Start()
    {
        // �ؽ�Ʈ ũ�� �� �޺��� �ʱ�ȭ
        maxScale = 70f;
        minScale = 50f;
        scaleSpeed = 400f;
        maxComboNum = 0;

        comboText.gameObject.SetActive(false); // �޺� �ؽ�Ʈ Ȱ��ȭ
    }

    void Update()
    {
        comboText.text = "x " + comboNum.ToString(); // �޺��� ����

        ComboActivate(); // �޺��� 0�Ͻ� ��Ȱ��ȭ
    }
    void ComboActivate() // �޺��� 0�Ͻ� ��Ȱ��ȭ
    {
        if (comboNum <= 0)
        {
            comboText.gameObject.SetActive(false);
        }
        else
        {
            comboText.gameObject.SetActive(true);
        }
    }

    public void UpdateTime(float gameTime)
    {
        gameTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(gameTime / 60), gameTime % 60);
    }
    public void UpdateCombo(int comboNum, int maxComboNum)
    {
        this.comboNum = comboNum;
        this.maxComboNum = maxComboNum;

        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleComboText());

        if (comboNum <= 0)
        {
            comboText.gameObject.SetActive(false);
        }
        else
        {
            comboText.gameObject.SetActive(true);
        }

        comboText.text = "x " + comboNum.ToString();
    }

    IEnumerator ScaleComboText()
    {
        float currentScale = comboText.fontSize;

        while (currentScale < maxScale)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            comboText.fontSize = Mathf.Min(currentScale, maxScale);
            yield return null;
        }

        while (currentScale > minScale)
        {
            currentScale -= scaleSpeed * Time.deltaTime;
            comboText.fontSize = Mathf.Max(currentScale, minScale);
            yield return null;
        }
    }
}
