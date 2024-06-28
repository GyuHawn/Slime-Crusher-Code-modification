using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    public GameObject comboObj;
    public int comboNum; // ���� �޺���
    public int maxComboNum; // �ִ� �޺���
    public TMP_Text comboText; // �޺� �ؽ�Ʈ

    // �޺��� ����� �ؽ�Ʈ ũ�� ����
    public float maxScale; // ũ�� ����
    public float minScale; // ũ�� ����
    public float scaleSpeed; // ���� �ӵ�

    void Start()
    {
        comboObj.SetActive(false); 

        // �ؽ�Ʈ ũ�� �� �޺��� �ʱ�ȭ
        maxScale = 100f;
        minScale = 55f;
        scaleSpeed = 400f;
        maxComboNum = 0;
    }

    void Update()
    {
        comboText.text = "x " + comboNum.ToString(); // �޺��� ����

        // �޺��� 0�Ͻ� ��Ȱ��ȭ
        if (comboNum <= 0)
        {
            comboObj.SetActive(false);
        }
        else
        {
            comboObj.SetActive(true);
        }
    }

    public void ComboUp()
    {
        StartCoroutine(ScaleComboText());

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
