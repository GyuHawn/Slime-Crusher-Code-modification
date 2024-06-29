using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public GameObject settingMenu; // �ɼ� ��ư

    public float moveDuration = 1.0f; // �ɼ�â �̵��ð�
    private Vector3 startMenuPos; // �ɼ�â ��ġ����
    private Vector3 endMenuPos; // �ɼ�â ��ġ����

    private bool onSetting; // �ɼ� Ȱ��ȭ
    
    private void Start()
    {
        SettingMovePosition(); // �ɼ� UI ��ġ�� ����

        // ���� �� ����
        onSetting = false;
    }
    void SettingMovePosition() // �ɼ� UI ��ġ�� ����
    {
        startMenuPos = new Vector3(870f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
        endMenuPos = new Vector3(540f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
    }

    public void OnSettingMenu() // �ɼǸ޴� Ȱ��ȭ
    {
        StartCoroutine(MoveSettingMenu()); // �ɼǸ޴� �̵�
    }

    IEnumerator MoveSettingMenu() // �ɼǸ޴� �̵�
    {
        // �ɼ�â ��Ȱ��ȭ�� ó�� ��ġ�� �̵�
        if (!onSetting)
        {
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                settingMenu.transform.localPosition = Vector3.Lerp(startMenuPos, endMenuPos, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            settingMenu.transform.localPosition = endMenuPos;
            onSetting = true;
        }
        else // �ɼ�â Ȱ��ȭ�� ������ ��ġ�� �̵�
        {
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                settingMenu.transform.localPosition = Vector3.Lerp(endMenuPos, startMenuPos, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            settingMenu.transform.localPosition = startMenuPos;
            onSetting = false;
        }
    }

    public void MainMenu() // ���θ޴��� �̵�
    {
        LodingController.LoadNextScene("MainMenu");
    }
}
