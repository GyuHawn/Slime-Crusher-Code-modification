using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SturnSkill : C_Skill
{
    private ItemSkill itemSkill;

    public C_SturnSkill(ItemSkill itemSkill)
    {
        this.itemSkill = itemSkill;
    }

    public void Execute(CharacterSkill characterSkill)
    {
        if (characterSkill.sturnTime <= 0)
        {
            AudioManager.Instance.PlayStunAudio();
            SturnAttack(characterSkill);
        }
    }

    private void SturnAttack(CharacterSkill characterSkill)
    {
        characterSkill.sturnTime = 4; // ��Ÿ�� ����

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // ��� ���� Ȯ��

        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            if (monsterController != null)
            {
                GameObject sturnInstance = GameObject.Instantiate(itemSkill.sturnEffect, monster.transform.position, Quaternion.identity); // ���� ����Ʈ ����
                GameObject sturnimageInstance = GameObject.Instantiate(itemSkill.sturnImage, monsterController.sturn.transform.position, Quaternion.identity); // ���� �̹��� ����

                monsterController.stop = true; // ���� ���� ����
                monsterController.attackTime += 5; // ���� ���� �ð� �ø���
                characterSkill.monsterToSturnImage[monster] = sturnimageInstance; // �� ���Ϳ� ���� �̹��� �Բ� ���� (���� ���Ž� �̹����� �Բ� ����)

                GameObject.Destroy(sturnimageInstance, 3f); // ���� �̹��� ����
            }
        }
        characterSkill.StartCoroutine(Removestun(characterSkill));
    }

    private IEnumerator Removestun(CharacterSkill character)
    {
        // ���� ���ӽð� ����� ������ ���� ���� ����
        yield return new WaitForSeconds(character.sturnDuration);
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            if (monsterController != null)
            {
                monsterController.stop = false;
            }
        }
    }
}
