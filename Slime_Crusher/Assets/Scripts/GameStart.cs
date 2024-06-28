 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using UnityEditor;
using TMPro;

public class GameStart : MonoBehaviour
{
    public GameObject settingMenu;
    public float moveDuration = 1.0f; // �ɼ�â �̵��ð�
    private Vector3 startMenuPos; // �ɼ�â ��ġ����
    private Vector3 endMenuPos;
    private bool onSetting; // �ɼ� Ȱ��ȭ

    public GameObject resetMenu; // ������ ���� �޴�

    public GameObject source; // ���۱� ���� ǥ�� �޴�

    public GameObject tutorialMenu;
    public GameObject[] tutorials; // Ʃ�丮�� ������Ʈ ����
    public int tutorialNum; // ���� Ʃ�丮�� ��� ��

    // �ؽ�Ʈ ����
    public TMP_Text startMenuText;
    public TMP_Text settingMenuText;
    public TMP_Text tutorialMenuText;
    public TMP_Text exitMenuText;

    private void Start()
    {
        // ���� �� ����
        onSetting = false;

        tutorialNum = -1;

        startMenuPos = new Vector3(870f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
        endMenuPos = new Vector3(540f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);

        startMenuText.color = Color.black;
        settingMenuText.color = Color.black;
        tutorialMenuText.color = Color.black;
        exitMenuText.color = Color.black;
    }

    private void Update()
    {
        // Ʃ�丮�� ���� ���� ��� ����
        for (int i = 0; i < tutorials.Length; i++)
        {
            tutorials[i].SetActive(i == tutorialNum);
        }

        // Ʃ�丮�� �ؽ�Ʈ �÷� ����
        if(tutorialNum >= 6)
        {
            tutorialMenuText.color = Color.black;
            tutorialMenu.SetActive(false);
        }
    }

    public void NewGame()
    {
        // ��ư Ŭ�� �ð��� ǥ�� �� �� �̵�
        startMenuText.color = Color.white;
        LodingController.LoadNextScene("Character");
    }

    public void OnSettingMenu()
    {
        StartCoroutine(MoveSettingMenu());
    }

    IEnumerator MoveSettingMenu()
    {
        // ��ư Ŭ�� �ð��� ǥ�� �� �� �̵�
        settingMenuText.color = Color.white;

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

        settingMenuText.color = Color.black;
    }

    // ���� �޴� ����
    public void OnResetMenu()
    {
        resetMenu.SetActive(!resetMenu.activeSelf);
    }

    // ������ ����
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        resetMenu.SetActive(false);
    }

    // Ʃ�丮�� ����
    public void OnTutorial()
    {
        tutorialMenuText.color = Color.white;
        tutorialMenu.SetActive(true);
        tutorialNum = 0;
    }

    // ���� Ʃ�丮��� �̵�
    public void NextTutorial()
    {
        if(tutorialNum < tutorials.Length)
        {
            tutorialNum++;
        }
    }

    // ���� Ʃ�丮��� �̵�
    public void BeforeTutorial()
    {
        if (tutorialNum > 0)
        {
            tutorialNum--;
        }     
    }

    // ���۱� ���� â ����
    public void OnSource()
    {
        source.SetActive(!source.activeSelf);
    }

    // ��������
    public void Exit()
    {
        exitMenuText.color = Color.black;
        Application.Quit();
    }
}
