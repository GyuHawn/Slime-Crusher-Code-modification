using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStart : MonoBehaviour
{
    public GameObject resetMenu; // ������ ���� �޴�
    public GameObject tutorialMenu; // Ʃ�丮�� �޴�
    public GameObject[] tutorials; // Ʃ�丮�� ������Ʈ ����
    public GameObject source; // ���۱� ���� ǥ�� �޴�
    public GameObject settingMenu; // �ɼ� �޴�

    public int tutorialNum; // ���� Ʃ�丮�� ��� ��

    // �ؽ�Ʈ ����
    public TMP_Text startMenuText;
    public TMP_Text settingMenuText;
    public TMP_Text tutorialMenuText;
    public TMP_Text exitMenuText;

    public Button settingButton;

    private void Start()
    {
        TextColorSetting(); // �ؽ�Ʈ �÷� ����

        // ���� �� ����
        tutorialNum = -1; // Ʃ�丮�� ������ ��

        SettingMenuManager.Instance.InitializeOptionMenu(settingMenu);
        settingButton.onClick.AddListener(OnSettingMenu);
    }

    void OnSettingMenu()
    {
        SettingMenuManager.Instance.ToggleSettingMenu();
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
