using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    private StageManager stageManager;
    private ItemSkill itemSkill;
    private AudioManager audioManager;
    private CharacterSkill charaterSkill;
    private Combo combo;

    public GameObject[] playerHealthUI; // ü�� ǥ��
    public GameObject healthEffect; // �ǰ� ǥ�� ����Ʈ
    public int playerHealth; // ���� ü��
    public GameObject gameover; // ���� ���� ǥ��
    public bool die; // ��� �Ͽ�����

    public int damage; // ������
    public int comboDamage; // �޺��� ������ ������
    public bool comboDamageUP; // ������ ���� ���� ���� ����
    public bool isAttacking = false; // ���� �ӵ� (���� ���ݱ��� �ð��� �ֱ�����)
    public bool hitBullet = true; // ���Ϳ��� ���� �޾Ҵ���
    public GameObject hubDamageText; // ������ �ð��� ǥ��

    public GameObject defenseEffect; // ��� ����Ʈ
    public bool defending; // ��� ������
    public float defenseTime; // ��� ��Ÿ��
    public GameObject defenseCoolTime; // ��Ÿ�� ǥ��
    public TMP_Text defenseCoolTimeText;

    public int money;

    public float gameTime; // �ѽð� ǥ��
    public TMP_Text gameTimeText;

    public bool isDragging = false; // �巡�� ������

    public GameObject attckEffect; // ���� ����Ʈ
    public GameObject dragEffect;

    public bool stage5Debuff = false; //���� ��ų ����
    public bool isStageHit = true; 

    public Vector3 lastClickPosition; // ������ Ŭ����ġ

    private void Awake()
    {
        stageManager = GameObject.Find("Manager").GetComponent<StageManager>();
        itemSkill = GameObject.Find("Manager").GetComponent<ItemSkill>();
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
        charaterSkill = GameObject.Find("Manager").GetComponent<CharacterSkill>();
        combo = GameObject.Find("Manager").GetComponent<Combo>();
    }

    void Start()
    {
        die = false;

        defending = false;

        damage = 10;
        playerHealth = 8;
        UpdateHealthUI();
        comboDamageUP = false;

        gameTime = 0f;
        lastClickPosition = Vector3.zero;
    }

    void Update()
    {
        UpdateHealthUI();

        // ��� ��Ÿ�� ����
        if (defenseTime >= 0)
        {
            defenseCoolTimeText.text = ((int)defenseTime).ToString();
            defenseTime -= Time.deltaTime;
        }
        else
        {
            defenseCoolTime.SetActive(false);
        }

        // �� �ð� ǥ��
        gameTime += Time.deltaTime;
        gameTimeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(gameTime / 60), gameTime % 60);
        
        // �巡�� ���� ����
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

        // �巡�� ���� ����Ʈ
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragEffect.transform.position = new Vector3(mousePosition.x, mousePosition.y, dragEffect.transform.position.z);
        }

        // ���� ����
        if (isDragging && !isAttacking)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            lastClickPosition = worldPoint;

            // ���� ��
            if (hit.collider != null)
            {
                MonsterController monsterController = hit.collider.GetComponent<MonsterController>();

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
                    if(playerHealth >= 8)
                    {
                        Destroy(hit.collider.gameObject);
                    }
                    else
                    {
                        playerHealth++;
                        Destroy(hit.collider.gameObject);
                    }
                }

                // ���� ���ݿ� ������
                if (hit.collider.CompareTag("Bullet") && hitBullet)
                {
                    audioManager.HitAudio();

                    playerHealth -= 1; // ü�� ����
                    combo.comboNum = 0; // �޺� �ʱ�ȭ
                    Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f); // �ǰ� ����Ʈ
                    GameObject effect = Instantiate(healthEffect, transform.position, Quaternion.identity);
                    StartCoroutine(BulletHitCooldown(0.2f)); // �� �ǰݰ��ɱ��� �ð�
                }

                // ���� ���ݽ�
                if (monsterController != null)
                {
                    if (monsterController.boss1Defending)
                    {
                        // 1�������� ���� ��ų�� ���� ���� �Ұ�
                    }
                    else if (monsterController.moved && hitBullet) // �̵����� ���� ����
                    {
                        playerHealth -= 1;
                        combo.comboNum = 0;
                        Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f);
                        GameObject effect = Instantiate(healthEffect, transform.position, Quaternion.identity);
                        StartCoroutine(BulletHitCooldown(0.2f));
                    }
                    else
                    {
                        StartCoroutine(AttackMonster(monsterController)); // ���� ���� ����
                    }
                }
            }
        }

        // �޺� ������ ������ ����
        if (comboDamageUP)
        {
            if(combo.comboNum >= 5) 
            {
                comboDamage += 5;
                comboDamageUP = false;
            }
            else if(combo.comboNum < 5)
            {
                comboDamage = 0;
            }
        }
    }

    // �÷��̾� �� �ǰݰ��ɱ��� �ð�
    IEnumerator BulletHitCooldown(float damageCooldown)
    {
        hitBullet = false;
        yield return new WaitForSeconds(damageCooldown);
        hitBullet = true;
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
        combo.comboNum = 0;
        Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6f);
        GameObject effect = Instantiate(healthEffect, effectPos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        isStageHit = true;
    }

    // ���� ���ݽ� ���� ������ ���� ����Ʈ ����
    void AttackEffect(Vector3 targetPosition)
    {
        float xOffset = Random.Range(-0.5f, 0.5f);
        float yOffset = Random.Range(-0.2f, 0.2f);
        Vector3 spawnPosition = targetPosition + new Vector3(xOffset, yOffset, -5);

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
        Destroy(effectTransform.gameObject);
    }

    // ���� ����
    IEnumerator AttackMonster(MonsterController monsterController)
    {
        isAttacking = true;

        if (monsterController.defense) // ���Ͱ� ������϶� ���� �Ұ�
        {
            isAttacking = false;
            yield break;
        }

        audioManager.AttackAudio();

        // ���� ���� ���ɽ� ���ݼ���
        if (monsterController.playerTakeDamage)
        {
            AttackEffect(monsterController.gameObject.transform.position); // ���� ����Ʈ ����
            monsterController.currentHealth -= damage + comboDamage; // ���� ü�� ����
            PlayerDamageText(monsterController); // ������ ǥ��
        }

        itemSkill.SetCurrentAttackedMonster(monsterController.gameObject);

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

        // ���콺�� ���ų� 0.2�ʵ� ���ݰ���
        float startTime = Time.time;
        yield return new WaitUntil(() => !Input.GetMouseButton(0) || Time.time - startTime > 0.2f);

        isAttacking = false; // ������ ����
    }

    // ü�� ����
    void UpdateHealthUI()
    {
        for (int i = 0; i < playerHealthUI.Length; i++) // ������ ü�� ��Ȱ��ȭ
        {
            playerHealthUI[i].SetActive(false);
        }

        if (playerHealth >= 0) // ü�� ǥ��
        {
            playerHealthUI[playerHealth].SetActive(true);
        }

        // ���
        if (playerHealth <= 0 && !die)
        {
            die = true;
            Handheld.Vibrate(); // ����� ����

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); // ���� ���� ��ü ����
            GameObject boss = GameObject.FindWithTag("Boss");
            foreach(GameObject monster in monsters)
            {
                Destroy(monster);
            }
            Destroy(boss);

            playerHealthUI[0].SetActive(true);
            Time.timeScale = 0f; // ���� ���߱�
            stageManager.Reward(); // ���� ���
            isDragging = false; 
            dragEffect.SetActive(false);
            stageManager.gameStart = false; // �������� ���·� ����
            gameover.SetActive(true); // ���â ǥ��
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

        audioManager.DefenseAudio();

        defenseEffect.SetActive(true);
        defenseCoolTime.SetActive(true);
        defenseTime = 6;

        yield return new WaitForSeconds(3f);

        defending = false;
        defenseEffect.SetActive(false);
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
        damegeText.GetComponent<DamageText>().damege = (int)charaterSkill.waterDamage;
        }
    }
    public void CRockDamageText(MonsterController monsterController)
    {
        if (monsterController != null)
        {
        GameObject damegeText = Instantiate(hubDamageText, monsterController.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        damegeText.GetComponent<DamageText>().damege = (int)charaterSkill.rockDamage;
        }
    }
}
