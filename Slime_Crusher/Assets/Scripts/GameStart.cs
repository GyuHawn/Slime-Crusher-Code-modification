using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStart : MonoBehaviour
{
    public GameObject settingMenu; // 옵션 메뉴
    public GameObject resetMenu; // 데이터 리셋 메뉴
    public GameObject tutorialMenu; // 튜토리얼 메뉴
    public GameObject[] tutorials; // 튜토리얼 오브젝트 관리
    public GameObject source; // 저작권 관련 표시 메뉴

    public float moveDuration = 1.0f; // 옵션창 이동시간
    private Vector3 startMenuPos; // 옵션창 위치관리
    private Vector3 endMenuPos; // 옵션창 위치관리
    private bool onSetting; // 옵션 활성화

    public int tutorialNum; // 현재 튜토리얼 장면 값

    // 텍스트 관리
    public TMP_Text startMenuText;
    public TMP_Text settingMenuText;
    public TMP_Text tutorialMenuText;
    public TMP_Text exitMenuText;

    private void Start()
    {
        SettingMovePosition(); // 옵션 UI 위치값 설정
        TextColorSetting(); // 텍스트 컬러 설정

        // 기초 값 설정
        onSetting = false; // 옵션 비활성화 상태
        tutorialNum = -1; // 튜토리얼 페이지 값
    }
    void SettingMovePosition() // 옵션 UI 위치값 설정
    {
        startMenuPos = new Vector3(870f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
        endMenuPos = new Vector3(540f, settingMenu.transform.localPosition.y, settingMenu.transform.localPosition.z);
    }
    void TextColorSetting() // 텍스트 컬러 설정
    {
        startMenuText.color = Color.black;
        settingMenuText.color = Color.black;
        tutorialMenuText.color = Color.black;
        exitMenuText.color = Color.black;
    }

    private void Update()
    {
        UpdateTutorials(); // 튜토리얼 관리
    }

    void UpdateTutorials() // 튜토리얼 관리
    {
        // 튜토리얼 값에 따른 장면 관리
        for (int i = 0; i < tutorials.Length; i++)
        {
            tutorials[i].SetActive(i == tutorialNum);
        }

        // 튜토리얼 텍스트 컬러 관리
        if (tutorialNum >= 6)
        {
            tutorialMenuText.color = Color.black;
            tutorialMenu.SetActive(false);
        }
    }

    public void NewGame() // 버튼 클릭 시각적 표시 및 씬 이동
    {  
        startMenuText.color = Color.white;
        LodingController.LoadNextScene("Character");
    }

    public void OnSettingMenu() // 옵션메뉴 활성화
    {
        StartCoroutine(MoveSettingMenu()); // 옵션메뉴 이동
    }

    IEnumerator MoveSettingMenu() // 옵션메뉴 이동
    {
        // 버튼 클릭 시각적 표시
        settingMenuText.color = Color.white;

        // 옵션창 비활성화시 처음 위치로 이동
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
        else // 옵션창 활성화시 정해진 위치로 이동
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

    // 리셋 메뉴 관리
    public void OnResetMenu()
    {
        resetMenu.SetActive(!resetMenu.activeSelf);
    }

    // 데이터 리셋
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        resetMenu.SetActive(false);
    }

    // 튜토리얼 시작
    public void OnTutorial()
    {
        tutorialMenuText.color = Color.white;
        tutorialMenu.SetActive(true);
        tutorialNum = 0;
    }

    // 다음 튜토리얼로 이동
    public void NextTutorial()
    {
        if(tutorialNum < tutorials.Length)
        {
            tutorialNum++;
        }
    }

    // 이전 튜토리얼로 이동
    public void BeforeTutorial()
    {
        if (tutorialNum > 0)
        {
            tutorialNum--;
        }     
    }

    // 저작권 관련 창 관리
    public void OnSource()
    {
        source.SetActive(!source.activeSelf);
    }

    // 게임종료
    public void Exit()
    {
        exitMenuText.color = Color.black;
        Application.Quit();
    }
}
