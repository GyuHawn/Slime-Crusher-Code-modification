using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float moveSpd; // �ؽ�Ʈ �̵� �ӵ�
    public float alphaSpd; // �ؽ�Ʈ ����ȭ �ӵ�
    public float destroyTime; // �ؽ�Ʈ ���� �ð�
    private TextMeshPro text;

    Color alpha; 
    public int damege;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damege.ToString(); // �������� ���� ����
        alpha = text.color;
        Invoke("DestroyText", destroyTime); // �����ð� ���� ����
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpd * Time.deltaTime, 0)); // �̵� �� ����
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpd); // ����ȭ �� ����
        text.color = alpha;
    }

    void DestroyText()
    {
        Destroy(gameObject);
    }
}
