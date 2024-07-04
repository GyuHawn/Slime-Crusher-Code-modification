using System.Collections.Generic;
using UnityEngine;

public class MonsterFactoryManager
{
    private Dictionary<int, MonsterFactory> factories;
    private GameObject[] infiniteMonsters;

    public MonsterFactoryManager(GameObject[] stage1Monsters, GameObject[] stage2Monsters, GameObject[] stage3Monsters,
                                 GameObject[] stage4Monsters, GameObject[] stage5Monsters, GameObject[] stage6Monsters,
                                 GameObject[] stage7Monsters, GameObject[] infiniteMonsters)
    {
        this.infiniteMonsters = infiniteMonsters;

        factories = new Dictionary<int, MonsterFactory>();

        // �� ���������� ���丮 ���� �� ���
        factories[1] = new Stage1MonsterFactory(stage1Monsters);
        factories[2] = new Stage2MonsterFactory(stage2Monsters);
        factories[3] = new Stage3MonsterFactory(stage3Monsters);
        factories[4] = new Stage4MonsterFactory(stage4Monsters);
        factories[5] = new Stage5MonsterFactory(stage5Monsters);
        factories[6] = new Stage6MonsterFactory(stage6Monsters);
        factories[7] = new Stage7MonsterFactory(stage7Monsters);
        factories[8] = new InfiniteMonsterFactory(infiniteMonsters);
    }

    public MonsterFactory GetFactory(int stage)
    {
        if (factories.ContainsKey(stage))
        {
            return factories[stage];
        }
        else
        {
            // 8�������� ���� InfiniteMonsters[] ���丮 ��ȯ
            return new InfiniteMonsterFactory(infiniteMonsters);
        }
    }
}
