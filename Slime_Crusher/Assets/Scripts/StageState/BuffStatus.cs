using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpStatus : StageStatus
{
    private int saveDamage;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "�÷��̾��� �⺻ ���ݷ��� �����մϴ�.";

        saveDamage = stageStatusManager.playerController.damage;
        stageStatusManager.playerController.damage += (int)(stageStatusManager.playerController.damage * 0.5f);
    }

    public void Reset(StageStatusManager stageStatusManager)
    {
        stageStatusManager.playerController.damage = saveDamage;
    }
}
public class MonsterHealthDownStatus : StageStatus
{
    private bool isMonsterHealthDown;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "������ �⺻ ü���� �����մϴ�.";

        isMonsterHealthDown = true;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        
        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            monsterController.currentHealth -= (monsterController.currentHealth * 0.3f);
        }
    }

    public void Reset(StageStatusManager stageStatusManager) {}
}
public class TimeUpStatus : StageStatus
{
    private bool isTimeUp;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "�������� ���� �ð��� �����մϴ�.";

        isTimeUp = true;
        stageStatusManager.stageTimeLimit.stageTime += 10;
    }

    public void Reset(StageStatusManager stageStatusManager)
    {
        isTimeUp = true;
        stageStatusManager.stageTimeLimit.stageTime -= 10;
    }
}
public class PercentUpStatus : StageStatus
{
    private bool isPercentUp;
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "������ �ߵ� Ȯ���� �����մϴ�.";

        isPercentUp = true;

        stageStatusManager.itemSkill.firePercent += 5f;
        stageStatusManager.itemSkill.fireShotPercent += 5f;
        stageStatusManager.itemSkill.holyShotPercent += 5f;
        stageStatusManager.itemSkill.holyWavePercent += 5f;
        stageStatusManager.itemSkill.rockPercent += 5f;
        stageStatusManager.itemSkill.posionPercent += 5f;
        stageStatusManager.itemSkill.meleePercent += 5f;
        stageStatusManager.itemSkill.sturnPercent += 5f;
    }

    public void Reset(StageStatusManager stageStatusManager)
    {
        isPercentUp = true;
        
        stageStatusManager.itemSkill.firePercent -= 5f;
        stageStatusManager.itemSkill.fireShotPercent -= 5f;
        stageStatusManager.itemSkill.holyShotPercent -= 5f;
        stageStatusManager.itemSkill.holyWavePercent -= 5f;
        stageStatusManager.itemSkill.rockPercent -= 5f;
        stageStatusManager.itemSkill.posionPercent -= 5f;
        stageStatusManager.itemSkill.meleePercent -= 5f;
        stageStatusManager.itemSkill.sturnPercent -= 5f;
    }
}
public class MonsterDieStatus : StageStatus
{
    public void Apply(StageStatusManager stageStatusManager)
    {
        stageStatusManager.buffText.text = "���� �ð����� ���Ͱ� ����մϴ�.";

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null && monsterController.currentHealth > 0)
            {
                monsterController.currentHealth = 0;
                break;
            }
        }
    }

    public void Reset(StageStatusManager stageStatusManager){}
}
