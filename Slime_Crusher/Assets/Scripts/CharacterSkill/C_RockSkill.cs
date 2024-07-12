using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_RockSkill : C_Skill
{
    private ItemSkill itemSkill;
    private PlayerController playerController;

    public C_RockSkill(ItemSkill itemSkill, PlayerController playerController)
    {
        this.itemSkill = itemSkill;
        this.playerController = playerController;
    }

    public void Execute(CharacterSkill characterSkill)
    {
        if (characterSkill.rockTime <= 0)
        {
            AudioManager.Instance.PlayRockAudio();
            RockAttack(characterSkill);
        }
    }

    private void RockAttack(CharacterSkill characterSkill)
    {
        characterSkill.rockTime = 4; // ��Ÿ�� ����

        // ��� ����, ���� Ȯ��
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject[] bossMonsters = GameObject.FindGameObjectsWithTag("Boss");

        // ���� ����Ʈ�� �Ҵ�
        List<GameObject> allMonsters = new List<GameObject>(monsters);
        allMonsters.AddRange(bossMonsters);

        foreach (GameObject monster in allMonsters)
        {
            MonsterController monsterController = monster.GetComponent<MonsterController>();

            if (monsterController != null && monsterController.pRockTakeDamage)
            {
                // ����Ʈ ����
                GameObject rockInstance = GameObject.Instantiate(itemSkill.rockEffect, monsterController.gameObject.transform.position, Quaternion.identity);

                playerController.CRockDamageText(monsterController); // ������ �ؽ�Ʈ ����
                monsterController.currentHealth -= characterSkill.rockDamage; // ������ ����
                monsterController.PlayerRockDamegeCoolDown(0.5f, 0.2f); // �ǰ� �ð� �� �ð��� ȿ��

                characterSkill.ItemSkill(monsterController); // ������ ���

                GameObject.Destroy(rockInstance, 2f); // ����Ʈ ����
            }
        }
    }
}
