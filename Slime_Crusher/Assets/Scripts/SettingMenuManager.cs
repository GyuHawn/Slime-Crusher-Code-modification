using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenuManager : MonoBehaviour
{
    public static SettingMenuManager Instance { get; private set; }

    public GameObject settingMenu; // �ɼ� �޴�
    public float moveDuration; // �ɼ�â �̵��ð�
    private Vector3 startMenuPos; // �ɼ�â ���� ��ġ
    private Vector3 endMenuPos; // �ɼ�â �̵� �� ��ġ
    private bool onSetting; // �ɼ� �޴� Ȱ��ȭ ����

    private Coroutine moveCoroutine; // �̵� �ڷ�ƾ ����

    public RectTransform settingMenuRectTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        moveDuration = 1.0f;
    }

    public void InitializeOptionMenu(GameObject menu)
    {
        settingMenuRectTransform = menu.GetComponent<RectTransform>(); // RectTransform ��������
        if (settingMenuRectTransform != null)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                // �ʱ� ��ġ ����
                startMenuPos = new Vector3(730f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                endMenuPos = new Vector3(389f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                onSetting = false;
            }
            else if(SceneManager.GetActiveScene().name == "Game")
            {
                // �ʱ� ��ġ ����
                startMenuPos = new Vector3(600f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                endMenuPos = new Vector3(0f, settingMenuRectTransform.localPosition.y, settingMenuRectTransform.localPosition.z);
                onSetting = false;
            }
        }
    }

    public void ToggleSettingMenu()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // ���� �̵� �ڷ�ƾ ����
        }
        moveCoroutine = StartCoroutine(MoveSettingMenu());
    }

    private IEnumerator MoveSettingMenu()
    {
        float elapsed = 0f;
        Vector3 targetPos = onSetting ? startMenuPos : endMenuPos;
        Vector3 startPos = onSetting ? endMenuPos : startMenuPos;

        while (elapsed < moveDuration)
        {
            settingMenu.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        settingMenu.transform.localPosition = targetPos;
        onSetting = !onSetting;
    }
}
