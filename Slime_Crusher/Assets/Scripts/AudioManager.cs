 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private StageManager stageManager;

    // BGM
    public AudioSource bgmMainMenu; // ���θ޴�
    public AudioSource bgmCharacterMenu; // ĳ���͸޴�
    public AudioSource bgmStage; // ��������
    public AudioSource bgmBossStage; // ������������
    public AudioSource bgmSelectMenu; // �����ۼ��ø޴�
    public AudioSource bgmResultMenu; // ���

    // function
    public AudioSource attackAudio; // ����
    public AudioSource defenseAudio; // ���
    public AudioSource hitAudio; // �ǰ�
    public AudioSource monsterAttackAudio; // ���� ����
    public AudioSource buttonAudio; // ��ư
    public AudioSource startAudio; // ����

    // Item
    public AudioSource fireAudio; // ���̾�
    public AudioSource fireShotAudio; // ���̾
    public AudioSource holyShotAudio; // Ȧ����
    public AudioSource holyWaveAudio; // Ȧ�����̺�
    public AudioSource meleeAudio; // ����
    public AudioSource posionAudio; // ��
    public AudioSource rockAudio; // ��
    public AudioSource sturnAudio; // ����

    // Character
    public AudioSource water;

    public Slider bgmSlider;
    public Slider generalSlider;

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
    }

    void Start()
    {
        StopAudio();

        // ��ü ���� ����
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float genVolume = PlayerPrefs.GetFloat("GenVolume", 1.0f);

        bgmSlider.value = bgmVolume;
        generalSlider.value = genVolume;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            bgmMainMenu.volume = bgmVolume;

            startAudio.volume = genVolume;
            buttonAudio.volume = genVolume;
            bgmMainMenu.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Loding")
        {
            bgmMainMenu.volume = bgmVolume;
        }
        else if (SceneManager.GetActiveScene().name == "Character")
        {
            bgmCharacterMenu.volume = bgmVolume;

            startAudio.volume = genVolume;
            buttonAudio.volume = genVolume;
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            bgmStage.volume = bgmVolume;
            bgmBossStage.volume = bgmVolume;
            bgmSelectMenu.volume = bgmVolume;
            bgmResultMenu.volume = bgmVolume;
        }
    }

    void Update()
    {
        // ��ü ���� ����
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            bgmMainMenu.volume = bgmSlider.value;

            startAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;
        }
        else if (SceneManager.GetActiveScene().name == "Loding")
        {
            bgmMainMenu.volume = bgmSlider.value;
        }
        else if (SceneManager.GetActiveScene().name == "Character")
        {
            bgmCharacterMenu.volume = bgmSlider.value;

            startAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            bgmStage.volume = bgmSlider.value;
            bgmBossStage.volume = bgmSlider.value;
            bgmSelectMenu.volume = bgmSlider.value;
            bgmResultMenu.volume = bgmSlider.value;

            attackAudio.volume = generalSlider.value;
            defenseAudio.volume = generalSlider.value;
            hitAudio.volume = generalSlider.value;
           // monsterAttackAudio.volume = generalSlider.value;
            buttonAudio.volume = generalSlider.value;

            fireAudio.volume = generalSlider.value;
            fireShotAudio.volume = generalSlider.value;
            holyShotAudio.volume = generalSlider.value;
            holyWaveAudio.volume = generalSlider.value;
            meleeAudio.volume = generalSlider.value;
            posionAudio.volume = generalSlider.value;
            rockAudio.volume = generalSlider.value;
            sturnAudio.volume = generalSlider.value;

            water.volume = generalSlider.value;
        }

        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("GenVolume", generalSlider.value);
    }

    // �Ҹ� ���
    public void AttackAudio()
    {
        attackAudio.Play();
    }
    public void DefenseAudio()
    {
        defenseAudio.Play();
    }
    public void HitAudio()
    {
        hitAudio.Play();
    }
    public void MonsterAttackAudio()
    {
        monsterAttackAudio.Play();
    }
    public void ButtonAudio()
    {
        buttonAudio.Play();
    }
    public void StartAudio()
    {
        startAudio.Play();
    }

    // Item
    public void FireAudio()
    {
        fireAudio.Play();
    }
    public void FireShotAudio()
    {
        fireShotAudio.Play();
    }
    public void HolyShotAudio()
    {
        holyShotAudio.Play();
    }
    public void HolyWaveAudio()
    {
        holyWaveAudio.Play();
    }
    public void MeleeAudio()
    {
        meleeAudio.Play();
    }
    public void PosionAudio()
    {
        posionAudio.Play();
    }
    public void RockAudio()
    {
        rockAudio.Play();
    }
    public void SturnAudio()
    {
        sturnAudio.Play();
    }

    // Character
    public void WaterAudio()
    {
        water.Play();
    }

    // ���۽� �Ҹ� �ߺ� ���ſ�
    void StopAudio()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            buttonAudio.Stop();
            startAudio.Stop();
        }
        else if (SceneManager.GetActiveScene().name == "Character")
        {
            buttonAudio.Stop();
            startAudio.Stop();
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if (stageManager.mainStage <= 8)
            {
                if (stageManager.subStage == 5)
                {
                    bgmStage.Stop();
                    bgmBossStage.Play();
                }
                else
                {
                    bgmBossStage.Stop();
                    bgmStage.Play();
                }
            }
            else
            {
                bgmStage.Stop();
                bgmBossStage.Play();
            }

            bgmBossStage.Stop();
            bgmSelectMenu.Stop();
            bgmResultMenu.Stop();

            attackAudio.Stop();
            defenseAudio.Stop();
            hitAudio.Stop();
            monsterAttackAudio.Stop();
            buttonAudio.Stop();

            fireAudio.Stop();
            fireShotAudio.Stop();
            holyShotAudio.Stop();
            holyWaveAudio.Stop();
            meleeAudio.Stop();
            posionAudio.Stop();
            rockAudio.Stop();
            sturnAudio.Stop();

            water.Stop();
        }
    }
}