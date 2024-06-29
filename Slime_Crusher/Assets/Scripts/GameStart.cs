using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStart : MonoBehaviour
{
    public GameObject settingMenu; // �ɼ� �޴�
    public GameObject resetMenu; // ������ ���� �޴�
    public GameObject tutorialMenu; // Ʃ�丮�� �޴�
    public GameObject[] tutorials; // Ʃ�丮�� ������Ʈ ����
    public GameObject source; // ���۱� ���� ǥ�� �޴�

    public float moveDuration = 1.0f; // �ɼ�â �̵��ð�
    private Vector3 startMenuPos; // �ɼ�â ��ġ����
    private Vector3 endMenuPos; // �ɼ�â ��ġ����
    private bool onSetting; // �ɼ� Ȱ��ȭ

    public int tutorialNum; // ���� Ʃ�丮�� ��� ��

    // �ؽ�Ʈ ����
    public TMP_Text startMenuText;
    public TMP_Text settingMenuText;
    public TMP_Text tutorialMenuText;
    public TMP_Text exitMenuText;

    private void Start()
    {
        SettingMovePosition(); // �ɼ� UI ��ġ�� ����
        TextColorSetting(); // �ؽ�Ʈ �÷� ����

        // ���� �� ����
        onSetting = false; // �ɼ� ��Ȱ��ȭ ����
        tutorialNum = -1; // Ʃ�丮�� ������ ��
    }
    void SettingMovePosition() // �ɼ� UI ��ġ�� ����
    {
        startMenuPos = new Vector3(870f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
        endMenuPos = new Vector3(540f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
    }
    void TextColorSetting() // �ؽ�Ʈ �÷� ����
    {
        startMenuText.color = Color.black;
        settingMenuText.color = Color.black;
        tutorialMenuText.color = Color.black;
        exitMenuText.color = Color.black;
    }

    private void Update()
    {
        UpdateTutorials(); // Ʃ�丮�� ����
    }

    void UpdateTutorials() // Ʃ�丮�� ����
    {
        // Ʃ�丮�� ���� ���� ��� ����
        for (int i = 0; i < tutorials.Length; i++)
        {
            tutorials[i].SetActive(i == tutorialNum);
        }

        // Ʃ�丮�� �ؽ�Ʈ �÷� ����
        if (tutorialNum >= 6)
        {
            tutorialMenuText.color = Color.black;
            tutorialMenu.SetActive(false);
        }
    }

    public void NewGame() // ��ư Ŭ�� �ð��� ǥ�� �� �� �̵�
    {  
        startMenuText.color = Color.white;
        LodingController.LoadNextScene("Character");
    }

    public void OnSettingMenu() // �ɼǸ޴� Ȱ��ȭ
    {
        StartCoroutine(MoveSettingMenu()); // �ɼǸ޴� �̵�
    }

    IEnumerator MoveSettingMenu() // �ɼǸ޴� �̵�
    {
        // ��ư Ŭ�� �ð��� ǥ��
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
