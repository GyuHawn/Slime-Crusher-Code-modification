using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    public int comboNum; // ���� �޺���
    public int maxComboNum; // �ִ� �޺���

    // �޺��� ����� �ؽ�Ʈ ũ�� ����
    public TMP_Text comboText; // �޺� �ؽ�Ʈ
    public float maxScale; // ũ�� ����
    public float minScale; // ũ�� ����
    public float scaleSpeed; // ���� �ӵ�

    void Start()
    {
        // �ؽ�Ʈ ũ�� �� �޺��� �ʱ�ȭ
        maxScale = 100f;
        minScale = 55f;
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


    public void ComboUp() // �޺� ����
    {
        comboNum++; // �޺� �� ����

        StartCoroutine(ScaleComboText()); // �޺��� ����� �ؽ�Ʈ�� ũ�Ⱑ Ű���ٰ� �پ��

        // �ִ� �޺��� ����
        if (comboNum > maxComboNum)
        {
            maxComboNum = comboNum;
        }
    }

    // �޺��� ����� �ؽ�Ʈ�� ũ�Ⱑ Ű���ٰ� �پ��
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
