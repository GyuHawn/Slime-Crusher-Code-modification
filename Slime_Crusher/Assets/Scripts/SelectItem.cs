using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class SelectItem : MonoBehaviour
{
    private StageManager stageManager;
    private PlayerController playerController;
    private Character character;
    private ItemSkill itemSkill;

    public bool itemSelecting; // ������ ������

    public GameObject[] items; // ��ü ������
    public GameObject itemPos1; // ������ ���� ǥ�� ��ġ
    public GameObject itemPos2;
    public GameObject itemPos3;
    public List<GameObject> selectItems; // ������ ������ 3�� ���� ����Ʈ
    public List<GameObject> playerItems; // ȹ���� ������ ����Ʈ

    public int selectNum; // ������ ������

    public GameObject selectItemPos1; // ȹ���� ������ ǥ��
    public GameObject selectItemPos2;
    public GameObject selectItemPos3;
    public GameObject selectItemPos4;

    public TMP_Text itemName; // ������ ������ �̸��� ����
    public TMP_Text itemEx;
    public TMP_Text item1LvText; // ȹ���� ������ ���� ǥ��
    public TMP_Text item2LvText;
    public TMP_Text item3LvText;
    public TMP_Text item4LvText;
    
    public GameObject[] characters; // ĳ���� ����Ʈ
    public GameObject charPos; // ĳ���� ǥ�� ��ġ
    public bool selectChar;

    // ��������
    public int passLv;
    public int fireLv;
    public int fireShotLv;
    public int holyWaveLv;
    public int holyShotLv;
    public int meleeLv;
    public int posionLv;
    public int rockLv;
    public int sturnLv;

    // ������ ���� ����
    public bool fireSelect;
    public bool fireShotSelect;
    public bool holyWaveSelect;
    public bool holyShotSelect;
    public bool meleeSelect;
    public bool posionSelect;
    public bool rockSelect;
    public bool sturnSelect;

    public GameObject selectItemMenu; // ������ ���� �޴�

    public bool selectedItem; // �������� �����Ͽ����� Ȯ��

    public Canvas canvas;
  //  public GameObject getItemUIPos; // ������ ������ ������Ʈ �θ�� ������Ʈ

    private void Awake()
    {
        character = GameObject.Find("Manager").GetComponent<Character>();
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        playerController = GameObject.Find("Manager").GetComponent<PlayerController>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
    }

    private void Start()
    {
        itemSelecting = false;
        selectChar = false;
    }

    private void Update()
    {
        ItemLevelOpen();
        CharacterInstant();
    }

    // ������ ĳ���� ��ġ ����
    void CharacterInstant()
    {
        if (!selectChar)
        {
            characters[character.currentCharacter - 1].transform.position = charPos.transform.position;

            selectChar = true;
        }
    }

    // ������ ����
    public void ItemSelect()
    {
        stageManager.selectingPass = true;
        playerController.isAttacking = true;
        selectItemMenu.SetActive(true);
        selectItems.Clear();

        itemSelecting = true;
        selectedItem = false;

        if (playerItems.Count >= 4)
        {
            while (selectItems.Count < 3)
            {
                int randomIndex = UnityEngine.Random.Range(0, playerItems.Count);
                GameObject selectedItem = playerItems[randomIndex];

                if (!selectItems.Contains(selectedItem))
                {
                    selectItems.Add(selectedItem);

                    GameObject itemFromItems = Array.Find(items, item => (item.name + "Pltem") == selectedItem.name);

                    while (itemFromItems == null || selectItems.Contains(itemFromItems))
                    {
                        randomIndex = UnityEngine.Random.Range(0, playerItems.Count);
                        selectedItem = playerItems[randomIndex];

                        if (!selectItems.Contains(selectedItem))
                        {
                            selectItems.RemoveAt(selectItems.Count - 1);
                            selectItems.Add(selectedItem);

                            itemFromItems = Array.Find(items, item => (item.name + "Pltem") == selectedItem.name);
                        }
                    }

                    // ������ ������ �˸��� ��ġ ����
                    GameObject itemPos = null;

                    switch (selectItems.Count)
                    {
                        case 1:
                            itemPos = itemPos1;
                            break;
                        case 2:
                            itemPos = itemPos2;
                            break;
                        case 3:
                            itemPos = itemPos3;
                            break;
                        default:
                            itemPos = null;
                            break;
                    }


                    itemFromItems.transform.position = itemPos.transform.position;
                }
            }
        }
        else
        {
            while (selectItems.Count < 3)
            {
                int randomIndex = UnityEngine.Random.Range(0, items.Length);
                GameObject selectedItem = items[randomIndex];

                if (!selectItems.Contains(selectedItem) && !playerItems.Exists(item => item.name == selectedItem.name + "Pltem"))
                {
                    selectItems.Add(selectedItem);

                    // ������ ������ �˸��� ��ġ ����
                    GameObject itemPos = null;

                    switch (selectItems.Count)
                    {
                        case 1:
                            itemPos = itemPos1;
                            break;
                        case 2:
                            itemPos = itemPos2;
                            break;
                        case 3:
                            itemPos = itemPos3;
                            break;
                        default:
                            itemPos = null;
                            break;
                    }


                    selectedItem.transform.position = itemPos.transform.position;
                }
            }
        }

        selectNum = UnityEngine.Random.Range(0, selectItems.Count) + 1;

        Time.timeScale = 0f;
    }

    // ������ ���� ����
    public void CloseMenu()
    {
        if (selectedItem)
        {
            bool isItemExist = false;

            foreach (GameObject item in playerItems)
            {
                if (item.name == items[selectNum - 1].name + "")
                {
                    isItemExist = true;
                    break;
                }
            }

            if (!isItemExist && playerItems.Count < 4)
            {            
                InstantiateItem();
            }

            foreach (GameObject selectItem in selectItems)
            {
                if (!playerItems.Contains(selectItem))
                {
                    selectItem.transform.position = new Vector3(0, 2000, 0);
                }
            }
            foreach (GameObject item in items)
            {
                item.transform.position = new Vector3(0, 2000, 0);
            }

            foreach (GameObject playerItem in playerItems)
            {
                playerItem.SetActive(true);
            }

            ItemTextClear();
            ItemLevelUp();
            //itemSkill.ItemValueUp();
            itemSkill.GetItem();    
            itemSelecting = false;
            stageManager.selectingPass = false;

            stageManager.passing = true;
            stageManager.NextSetting();

            playerController.isAttacking = false;
            Time.timeScale = 1f;
            selectItemMenu.SetActive(false);
        }
    }

    // ȹ���� ������ ����
    void InstantiateItem()
    {
        GameObject newItem = Instantiate(items[selectNum - 1], selectItemPos1.transform.position, Quaternion.identity);

        int playerItemsCount = playerItems.Count;

        switch (playerItemsCount)
        {
            case 0:
                newItem.transform.SetParent(selectItemPos1.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
            case 1:
                newItem.transform.SetParent(selectItemPos2.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
            case 2:
                newItem.transform.SetParent(selectItemPos3.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
            case 3:
                newItem.transform.SetParent(selectItemPos4.transform, false);
                newItem.transform.localPosition = Vector3.zero;
                break;
        }

        playerItems.Add(newItem);
        newItem.name = newItem.name.Replace("(Clone)", "Pltem");
    }


    // ȹ���� �����۰� �´� ���� �Ҵ�
    int GetItemLevel(GameObject item)
    {
        switch (item.name)
        {
            case "FirePltem": return fireLv;
            case "Fire ShotPltem": return fireShotLv;
            case "Holy WavePltem": return holyWaveLv;
            case "Holy ShotPltem": return holyShotLv;
            case "MeleePltem": return meleeLv;
            case "PosionPltem": return posionLv;
            case "RockPltem": return rockLv;
            case "SturnPltem": return sturnLv;
            default: return 0;
        }
    }

    // ȹ���� �������� ����ǥ�� ����
    void ItemLevelOpen()
    {
        item1LvText.gameObject.SetActive(playerItems.Count > 0);
        item1LvText.text = playerItems.Count > 0 ? GetItemLevel(playerItems[0]).ToString() : "";

        item2LvText.gameObject.SetActive(playerItems.Count > 1);
        item2LvText.text = playerItems.Count > 1 ? GetItemLevel(playerItems[1]).ToString() : "";

        item3LvText.gameObject.SetActive(playerItems.Count > 2);
        item3LvText.text = playerItems.Count > 2 ? GetItemLevel(playerItems[2]).ToString() : "";

        item4LvText.gameObject.SetActive(playerItems.Count > 3);
        item4LvText.text = playerItems.Count > 3 ? GetItemLevel(playerItems[3]).ToString() : "";
    }

    // ������ ������
    public void ItemLevelUp()
    {
        switch (selectNum)
        {
            case 1:
                fireLv++;
                break;
            case 2:
                fireShotLv++;
                break;
            case 3:
                holyWaveLv++;
                break;
            case 4:
                holyShotLv++;
                break;
            case 5:
                meleeLv++;
                break;
            case 6:
                posionLv++;
                break;
            case 7:
                rockLv++;
                break;
            case 8:
                sturnLv++;
                break;
        }
    }
    
    // ������ ������ �ؽ�Ʈ �ʱ�ȭ

    void ItemTextClear()
    {
        itemName.text = "";
        itemEx.text = "";
    }

    // ������ ������ ���� ǥ��
    public void Fire()
    {
        if (itemSelecting)
        {
            //items[0]
            selectedItem = true;
            selectNum = 1;
            itemName.text = "��";
            itemEx.text = "����Ȯ���� ������ ��ġ�� �ұ���� ��ȯ�մϴ�.";
        }
    }
    public void FireShot()
    {
        if (itemSelecting)
        {
            //items[1]
            selectedItem = true;
            selectNum = 2;
            itemName.text = "��ź";
            itemEx.text = "����Ȯ���� ������ �������� �ְ� ������ ���� ������ �������� �����ϴ�.";
        }
    }
    public void HolyWave()
    {
        if (itemSelecting)
        {
            //items[2]
            selectedItem = true;
            selectNum = 3;
            itemName.text = "��";
            itemEx.text = "����Ȯ���� ���� �ĵ��� �Ϸ��̸�, ���� �ð� ���� ��ü ���鿡�� �������� ���ظ� �����ϴ�.";
        }
    }
    public void HolyShot()
    {
        if (itemSelecting)
        {
            //items[3]
            selectedItem = true;
            selectNum = 4;
            itemName.text = "����";
            itemEx.text = "����Ȯ���� ������ ������ ���� �����Ͽ� ���� �ð� ���� �������� �����ϴ�.";
        }
    }
    public void Melee()
    {
        if (itemSelecting)
        {
            //items[4]
            selectedItem = true;
            selectNum = 5;
            itemName.text = "��Ÿ";
            itemEx.text = "����Ȯ���� ������ �߰������� ������ �մϴ�.";
        }
    }
    public void Posion()
    {
        if (itemSelecting)
        {
            //items[5]
            selectedItem = true;
            selectNum = 6;
            itemName.text = "�͵�";
            itemEx.text = "����Ȯ���� ���Ϳ��� ���� �ð� ���� ���������� �������� �����ϴ�.";
        }
    }
    public void Rock()
    {
        if (itemSelecting)
        {
            //items[6]
            selectedItem = true;
            selectNum = 7;
            itemName.text = "����";
            itemEx.text = "����Ȯ���� ������ �������� �����ϴ�.";
        }
    }
    public void Sturn()
    {
        if (itemSelecting)
        {
            //items[7]
            selectedItem = true;
            selectNum = 8;
            itemName.text = "����";
            itemEx.text = "����Ȯ���� ���� �����Ͽ� ���� �ð� ���� ���� ���·� ����ϴ�.";
        }
    }
}
