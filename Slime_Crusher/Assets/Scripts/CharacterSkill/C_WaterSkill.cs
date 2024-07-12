using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_WaterSkill : C_Skill
{
    private ItemSkill itemSkill;
    private PlayerController playerController;

    public C_WaterSkill(ItemSkill itemSkill, PlayerController playerController)
    {
        this.itemSkill = itemSkill;
        this.playerController = playerController;
    }

    public void Execute(CharacterSkill characterSkill)
    {
        if (characterSkill.waterTime <= 0)
        {
            characterSkill.useWaterSkill = true;
            characterSkill.StartCoroutine(WaterSkillRoutine(characterSkill));
        }
    }

    private IEnumerator WaterSkillRoutine(CharacterSkill characterSkill)
    {
        if (characterSkill.useWaterSkill)
        {
            characterSkill.waterTime = 4; // ��Ÿ�� ����

            characterSkill.MonsterList = new List<GameObject>(); // ���� Ȯ��

            for (int i = 0; i < 20; i++)
            {
                WaterAttack(characterSkill);

                if (characterSkill.MonsterList.Count > 0)
                {
                    // ����Ʈ �ʱ�ȭ
                    characterSkill.MonsterList.Clear();
                }

                if (!characterSkill.useWaterSkill)
                {
                    // ��ų ��Ȱ��ȭ�� ��ų��� ����
                    yield break;
                }

                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    private void WaterAttack(CharacterSkill character)
    {
        List<GameObject> selectedMonsters = new List<GameObject>(); // ���� �����ϴ� ���� �� Ȯ��
        if (character.MonsterList.Count == 0)
        {
            // ��� ����, ���� Ȯ��
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            GameObject[] bossMonsters = GameObject.FindGameObjectsWithTag("Boss");

            // ���� ����Ʈ�� �Ҵ�
            List<GameObject> allMonsters = new List<GameObject>(monsters);
            allMonsters.AddRange(bossMonsters);

            if (allMonsters.Count > 0)
            {
                // ���� ���� ����
                selectedMonsters.Add(allMonsters[Random.Range(0, allMonsters.Count)]);
            }

            character.MonsterList.AddRange(selectedMonsters);
        }
        else
        {
            if (character.MonsterList.Count > 0)
            {
                selectedMonsters.Add(character.MonsterList[Random.Range(0, character.MonsterList.Count)]);
            }
        }

        foreach (GameObject monster in selectedMonsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null)
            {
                // ����Ʈ ���� ��ġ
                Vector3 waterPosition = new Vector3(monsterController.gameObject.transform.position.x, monsterController.gameObject.transform.position.y - 0.2f, monsterController.gameObject.transform.position.z);
                GameObject waterInstance = GameObject.Instantiate(character.waterEffect, waterPosition, Quaternion.Euler(90, 0, 0)); // ����Ʈ ����
                AudioManager.Instance.PlayWaterAudio(); // ����� ����

                if (monsterController.pWaterTakeDamage)
                {
                    playerController.CWaterDamageText(monsterController); // ������ �ؽ�Ʈ ����
                    monsterController.currentHealth -= character.waterDamage; // ������ ����
                    monsterController.PlayerWaterDamegeCoolDown(0.5f, 0.1f); // �ǰ� �ð� �� �ð��� ȿ��
                }

                character.ItemSkill(monsterController); // ������ ���

                GameObject.Destroy(waterInstance, 2f); // ����Ʈ ����
            }
        }
    }
}