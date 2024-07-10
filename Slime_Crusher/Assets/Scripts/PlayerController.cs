using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    private StageManager stageManager;
    private ItemSkill itemSkill;
    private CharacterSkill characterSkill;
    private Combo combo;

    // �÷��̾� ü�� ����
    public GameObject[] playerHealthUI; // ü�� ǥ��
    public GameObject hitEffect; // �ǰ� ǥ�� ����Ʈ
    public int playerHealth; // ���� ü��
    public GameObject gameover; // ���� ���� ǥ��
    public bool die; // ��� �Ͽ�����

    // �÷��̾� ���� ����
    public int damage; // ������
    public int comboDamage; // �޺��� ������ ������
    public bool comboDamageUP; // ������ ���� ���� ���� ����
    public bool isAttacking = false; // ���� �ӵ� (���� ���ݱ��� �ð��� �ֱ�����)
    public bool playerHit = true; // ���Ϳ��� ���� �޾Ҵ���
    public GameObject hubDamageText; // ������ �ð��� ǥ��
    public GameObject attckEffect; // ���� ����Ʈ
    public GameObject dragEffect;

    public Vector3 lastClickPosition; // ������ Ŭ����ġ
    public bool isDragging = false; // �巡�� ������

    // �÷��̾� ��� ����
    public GameObject defenseEffect; // ��� ����Ʈ
    public bool defending; // ��� ������
    public float defenseTime; // ��� ��Ÿ��
    public GameObject defenseCoolTime; // ��Ÿ�� ǥ��
    public TMP_Text defenseCoolTimeText;

    // ��
    public int money;

    // �������� ����
    public bool stage5Debuff = false; //���� ��ų ����
    public bool isStageHit = true; 

    private void Awake()
    {
        if (!stageManager)
            stageManager = FindObjectOfType<StageManager>();
        if (!itemSkill)
            itemSkill = FindObjectOfType<ItemSkill>();
        if (!characterSkill)
            characterSkill = FindObjectOfType<CharacterSkill>();
        if (!combo)
            combo = FindObjectOfType<Combo>();
    }

    void Start()
    {
        playerHealth = 8; // ü�� ����
        damage = 10; // ������ ����  

        die = false; // ��� ���� �ʱ�ȭ
        comboDamageUP = false; // ������ ���� ���� �ʱ�ȭ
        defending = false; // ��� �� ���� �ʱ�ȭ    

        lastClickPosition = Vector3.zero; // Ŭ�� ������ �ʱ�ȭ

        UpdateHealth(); // �÷��̾� ü�°���
    }

    public void UpdateHealth() // �÷��̾� ü�°���
    {
        for (int i = 0; i < playerHealthUI.Length; i++)
        {
            playerHealthUI[i].SetActive(i == playerHealth);
        }
    }

    void Update()
    {
        // ���
        if (playerHealth <= 0 && !die)
        {
            Die();
        }

        DefenseCoolTime(); // ��� ��Ÿ�� ����        
        PlayerAttack(); // �÷��̾� ���ݰ���
        ComboDamageUp(); // �޺� ������ ������ ����
    }

    void Die() // ���
    {
        die = true;
        Handheld.Vibrate(); // ����� ����

        // ��� ���� ����
        foreach(GameObject monster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            Destroy(monster);
        }
        Destroy(GameObject.FindWithTag("Boss"));

        playerHealthUI[0].SetActive(true);
        Time.timeScale = 0f; // ���� ���߱�
        stageManager.Reward(); // ���� ���
        isDragging = false;
        dragEffect.SetActive(false);
        stageManager.gameStart = false; // �������� ���·� ����
        gameover.SetActive(true); // ���â ǥ��
    }

    void DefenseCoolTime() // ��� ��Ÿ�� ����
    {      
        if (defenseTime > 0)
        {
            defenseCoolTimeText.text = ((int)defenseTime).ToString();
            defenseTime -= Time.deltaTime;
        }
        else
        {
            defenseCoolTime.SetActive(false);
        }
    }

    void OnDrag() // ���� ���� ����
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragEffect.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            dragEffect.SetActive(false);
        }
    }
    void AttackEffect()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragEffect.transform.position = new Vector3(mousePosition.x, mousePosition.y, dragEffect.transform.position.z);
        }
    }

    void PlayerAttack() // �÷��̾� ���ݰ���
    {
         OnDrag(); // ���� ���� ����
         AttackEffect(); // ���� ����Ʈ


        // ���� ����
        if (isDragging && !isAttacking)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            lastClickPosition = worldPoint;

            // ���� ��
            if (hit.collider != null)
            {
                var monsterController = hit.collider.GetComponent<MonsterController>();

                // ���� ��ų�� ������ �������
                if (hit.collider.CompareTag("Stage6") || hit.collider.CompareTag("Stage4") || hit.collider.CompareTag("Stage7"))
                {
                    if (isStageHit)
                    {
                        StartCoroutine(StageBossHit());
                    }
                }

                // ȸ�� ������ ȹ��
                if (hit.collider.CompareTag("HealthUp"))
                {
                    if (playerHealth < 8)
                    {
                        playerHealth++;
                    }
                    else
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }

                // ���� ���ݿ� ������
                if (hit.collider.CompareTag("Bullet") && playerHit)
                {
                    PlayerHit(); // (���� �Ѿ�) �÷��̾� �ǰ�
                }

                // ���� ���ݽ�
                if (monsterController != null)
                {
                    PlayerEtcHit(monsterController); // (���� �Ѿ� ����) �÷��̾� �ǰ�
                }
            }
        }
    }

    void PlayerHit() // �÷��̾� �ǰ�
    {
        AudioManager.Instance.PlayHitAudio();

        playerHealth --; // ü�� ����
        stageManager.comboNum = 0; // �޺� �ʱ�ȭ
        UpdateHealth(); // �÷��̾� �ǰ�

        Instantiate(hitEffect, transform.position, Quaternion.identity);
        StartCoroutine(BulletHitCooldown(0.2f)); // �� �ǰݰ��ɱ��� �ð�
    }
    void PlayerEtcHit(MonsterController monsterController) // (���� �Ѿ� ����) �÷��̾� �ǰ�
    {
        if (monsterController.boss1Defending) { } // 1�������� ���� ��ų�� ���� ���� �Ұ�
        else if (monsterController.moved && playerHit) // �̵����� ���� ����
        {
            PlayerHit();
        }
        else
        {
            StartCoroutine(AttackMonster(monsterController)); // ���� ���� ����
        }
    }


    void ComboDamageUp()// �޺� ������ ������ ����
    {      
        if (comboDamageUP)
        {
            if (stageManager.comboNum >= 5)
            {
                comboDamage += 5;
                comboDamageUP = false;
            }
            else if (stageManager.comboNum < 5)
            {
                comboDamage = 0;
            }
        }
    }
 
    // ���
    public void Defense()
    {
        if (defenseTime <= 0)
        {
            StartCoroutine(StartDefense());
        }
    }

    IEnumerator StartDefense()
    {
        defending = true;

        AudioManager.Instance.PlayDefenseAudio();

        defenseEffect.SetActive(true);
        defenseCoolTime.SetActive(true);
        defenseTime = 6;

        yield return new WaitForSeconds(3f);

        defending = false;
        defenseEffect.SetActive(false);
    }


    // �÷��̾� �� �ǰݰ��ɱ��� �ð�
    IEnumerator BulletHitCooldown(float damageCooldown)
    {
        playerHit = false;
        yield return new WaitForSeconds(damageCooldown);
        playerHit = true;
    }

    // �÷��̾ Ŭ������ ��ġ (�÷��̾� �巡�׸� ���󰡴� ���Ϳ�)
    public Vector3 GetLastClickPosition()
    {
        return lastClickPosition;
    }

    // ���� ��ų�� �ǰݽ�
    IEnumerator StageBossHit()
    {
        isStageHit = false;      
        playerHealth -= 1;
        stageManager.comboNum = 0;
        UpdateHealth(); // �÷��̾� �ǰ�

        Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f);
        Instantiate(hitEffect, effectPos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        isStageHit = true;
    }

    // ���� ���ݽ� ���� ������ ���� ����Ʈ ����
    void AttackEffect(Vector3 targetPosition)
    {
        var spawnPosition = targetPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), -5);
        GameObject monsterHit = Instantiate(attckEffect, spawnPosition, Quaternion.identity);
        StartCoroutine(RotateAndShrinkEffect(monsterHit.transform, 0.3f)); // 0.3�� �Ŀ� ���� ��Ʈ ����Ʈ ����
    }

    IEnumerator RotateAndShrinkEffect(Transform effectTransform, float destroyDelay)
    {
        float duration = 0.3f;
        float elapsedTime = 0f;

        Quaternion startRotation = effectTransform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, 180);

        Vector3 startScale = effectTransform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsedTime < duration)
        {
            effectTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            effectTransform.localScale = Vector3.Slerp(startScale, targetScale, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectTransform.rotation = targetRotation;
        effectTransform.localScale = targetScale;

        yield return new WaitForSeconds(destroyDelay);
    }

    // ���� ����
    IEnumerator AttackMonster(MonsterController monsterController)
    {
        isAttacking = true;
        AudioManager.Instance.PlayAttackAudio(); // ���� �����
        itemSkill.SetCurrentAttackedMonster(monsterController.gameObject); // ���� ���ݹ޴� ���� ����

        if (monsterController.defense) // ���Ͱ� ������϶� ���� �Ұ�
        {
            isAttacking = false;
            yield break;
        }

        // ���� ���� ���ɽ� ���ݼ���
        if (monsterController.playerTakeDamage)
        {
            AttackEffect(monsterController.gameObject.transform.position); // ���� ����Ʈ ����
            monsterController.currentHealth -= damage + comboDamage; // ���� ü�� ����
            PlayerDamageText(monsterController); // ������ ǥ��
        }

        // �÷��̾� ���� �ӵ� ���� (�������� 5���� ��ų - ���� �ӵ� ����)
        if (stage5Debuff)
        {
            monsterController.PlayerDamegeCoolDown(3f, 0.2f);
        }
        else
        {
            monsterController.PlayerDamegeCoolDown(0.2f, 0.2f);
        }

        // ���ݽ� Ȯ�������� ������ �ߵ�
        if (itemSkill.isFire && Random.Range(0f, 100f) <= itemSkill.firePercent)
        {
            itemSkill.Fire(monsterController.gameObject.transform.position);
        }

        if (itemSkill.isFireShot && Random.Range(0f, 100f) <= itemSkill.fireShotPercent)
        {
            itemSkill.FireShot(monsterController.gameObject.transform.position);
            if (monsterController.fireShotTakeDamage)
            {
                FireDamageText(monsterController);
                monsterController.currentHealth -= itemSkill.fireShotDamage;
                monsterController.FireShotDamegeCoolDown(0.5f, 0.2f);
            }
        }

        if (itemSkill.isHolyWave && Random.Range(0f, 100f) <= itemSkill.holyWavePercent)
        {
            itemSkill.HolyWave();
        }

        if (itemSkill.isHolyShot && Random.Range(0f, 100f) <= itemSkill.holyShotPercent)
        {
            itemSkill.HolyShot(monsterController.gameObject.transform.position);
        }

        if (itemSkill.isMelee && Random.Range(0f, 100f) <= itemSkill.meleePercent)
        {
            itemSkill.Melee(monsterController.gameObject.transform.position, itemSkill.meleeNum);

            StartCoroutine(MeleeAttack());
        }
        IEnumerator MeleeAttack()
        {
            for (int i = 0; i < itemSkill.meleeNum; i++)
            {
                monsterController.currentHealth -= damage + comboDamage;

                PlayerDamageText(monsterController);

                yield return new WaitForSeconds(0.15f);
            }
        }

        if (itemSkill.isPosion && Random.Range(0f, 100f) <= itemSkill.posionPercent)
        {
            itemSkill.Posion(monsterController.gameObject.transform.position);
        }

        if (itemSkill.isRock && Random.Range(0f, 100f) <= itemSkill.rockPercent)
        {
            itemSkill.Rock(monsterController.gameObject.transform.position);
            if (monsterController.rockTakeDamage)
            {
                RockDamageText(monsterController);
                monsterController.currentHealth -= itemSkill.rockDamage;
                monsterController.RockDamegeCoolDown(0.5f, 0.2f);
            }
        }

        if (itemSkill.isSturn && Random.Range(0f, 100f) <= itemSkill.sturnPercent && monsterController.CompareTag("Monster"))
        {
            itemSkill.Sturn();
        }

        
        StartCoroutine(AttackDelayTime()); // ���� �����ð�
    }

    IEnumerator AttackDelayTime() // ���������ð� (���콺�� ���ų� 0.2�ʵ� ���ݰ���)
    {
        float startTime = Time.time;
        yield return new WaitUntil(() => !Input.GetMouseButton(0) || Time.time - startTime > 0.2f);

        isAttacking = false; // ������ ����
    }


    // �÷��̾� ���� ������ �ؽ�Ʈ
    public void PlayerDamageText(MonsterController monsterController)
    {
        if(monsterController != null)
        {
            GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            damegeText.GetComponent<DamageText>().damege = (damage + comboDamage);
        }
    }

    public void FireDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.fireDamage;
        }
    }

    public void FireShotDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.fireShotDamage;
        }
    }
    public void FireShotSubDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.fireShotSubDamage;
        }
    }

    public void HolyShotDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.holyShotDamage;
        }
    }

    public void HolyWaveDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.holyWaveDamage;
        }
    }
    public void RockDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.rockDamage;
        }
    }
    public void PoisonDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)itemSkill.poisonDamage;
        }
    }

    public void CWaterDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)characterSkill.waterDamage;
        }
    }
    public void CRockDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)characterSkill.rockDamage;
        }
    }
}
