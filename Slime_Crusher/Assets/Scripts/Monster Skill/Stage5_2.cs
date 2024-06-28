using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_2 : MonoBehaviour
{
    private MonsterController monsterController;

    public GameObject pos;
    public Vector3 boxSize;
    public float moveSpd;

    void Start()
    {
        monsterController = GetComponent<MonsterController>();
        pos = GameObject.Find("Stage7 SkillPos");
        boxSize = new Vector3(-13f, 5.5f, 0);
    }

    public void Attack()
    {
        monsterController.moved = true;
        float randomX = Random.Range(pos.transform.position.x - boxSize.x / 2, pos.transform.position.x + boxSize.x / 2);
        Vector3 targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        StartCoroutine(MoveToPosition(targetPosition));
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpd * Time.deltaTime);
            yield return null;
        }
        monsterController.moved = false;
    }
}
