using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Button settingButton;
    public GameObject settingMenu; // �ɼ� ��ư

    /*public float moveDuration = 1.0f; // �ɼ�â �̵��ð�
    private Vector3 startMenuPos; // �ɼ�â ��ġ����
    private Vector3 endMenuPos; // �ɼ�â ��ġ����
    private bool onSetting; // �ɼ� Ȱ��ȭ*/

    private void Start()
    {
        SettingMenuManager.Instance.InitializeOptionMenu(settingMenu);
        settingButton.onClick.AddListener(OnSettingMenu);
    }
    void OnSettingMenu()
    {
        SettingMenuManager.Instance.ToggleSettingMenu();
    }

    public void MainMenu() // ���θ޴��� �̵�
    {
        SceneManager.LoadScene("MainMenu");
        //LodingController.LoadNextScene("MainMenu");
    }
}
