using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public int currentCharacter; // ���� ������ ĳ����
  
    // ĳ����
    public bool rock;
    public bool water;
    public bool lihgt;
    public bool luck;

    void Start()
    {
        currentCharacter = PlayerPrefs.GetInt("SelectChar");

        // ĳ���� �ʱ�ȭ
        rock = false;
        water = false;
        lihgt = false;
        luck = false;
    }

    
    void Update()
    {
        // ������ ĳ����
        if(currentCharacter == 1) 
        {
            rock = true;
        }
        else if(currentCharacter == 2)
        {
            water = true;
        }
        else if(currentCharacter == 3)
        {
            lihgt = true;
        }
        else if(currentCharacter==4)
        {
            luck = true;
        }
    }
}
