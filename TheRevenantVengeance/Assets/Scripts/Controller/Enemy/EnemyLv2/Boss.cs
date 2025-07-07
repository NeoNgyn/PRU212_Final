using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controller.Enemy.EnemyLv2
{
    public class Boss : EnemyController
    {
        [SerializeField] private GameObject bulletPrefabs;
        [SerializeField] private Transform firePoint;
        //[SerializeField] private float normalAttackSpeed = 20f;
        [SerializeField] private float specialAttackSpeed = 5f;
        [SerializeField] private float hpValue = 20f;
        //[SerializeField] private GameObject miniEnemy;

        //[SerializeField] private float shotDelay = 0.2f;
        //private float nextShot;
        [SerializeField] private float skillCooldown = 10f;
        private float nextSkill;

        [SerializeField] private GameObject itemPrefab;

        private Animator animator;
        private bool isAttacking = false;
        private readonly float attackCooldown = 1f;
        private float lastAttackTime = 0f;
        protected bool isDead = false;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (isDead || player == null || player.currentHp <= 0) return;
            base.Update();

            NormalAttack();
            UseSkill();
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        player.TakeDamage(enterDamage);
        //    }
        //}

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        player.TakeDamage(stayDamage);
        //    }
        //}

        private void NormalAttack()
        {        
            if (isDead || isAttacking || player == null) return;

            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance > 4f) return;

            if (Time.time - lastAttackTime < attackCooldown) return;

            isAttacking = true;
            lastAttackTime = Time.time;

            animator?.SetTrigger("Attack");

            Invoke(nameof(ResetAttack), attackCooldown);
        }
        private void ResetAttack()
        {
            isAttacking = false;
        }
        private void DealDamageFromAttack1()
        {
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
                player.TakeDamage(enterDamage);
        }

        private void DealDamageFromAttack2()
        {
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
                player.TakeDamage(enterDamage * 1.5f);
        }
        private void DealDamageFromAttack3()
        {
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
                player.TakeDamage(enterDamage * 1.8f);
        }
        public override void TakeDamage(float damage, Vector2 knockbackDirection)
        {
            base.TakeDamage(damage, knockbackDirection);
            if (currentHp > 0)
            {
                animator?.SetTrigger("TakeHit");
            }
        }

        private void SpecialAttack()
        {
            const int bulletCount = 12;
            float angleStep = 360 / bulletCount;
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
                GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
                EnemyBulletController enemyBullet = bullet.AddComponent<EnemyBulletController>();
                enemyBullet.SetMovementDirection(bulletDirection * specialAttackSpeed);
            }

        }

        private void Heal(float hpAmount)
        {
            currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
            UpdateHealthBar();

        }

        //private void SpawnMiniEnemy()
        //{
        //    float radius = 2f;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        float angle = i * Mathf.PI * 2 / 5;
        //        Vector3 spawnPos = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        //        Instantiate(miniEnemy, spawnPos, Quaternion.identity);

        //    }
        //}

        private void Teleport()
        {
            if (player != null)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float distanceFromPlayer = 3f;

                Vector3 teleportPosition = player.transform.position + (Vector3)(randomDirection * distanceFromPlayer);

                transform.position = teleportPosition;
            }
        }

        private void ChooseRandomSkill()
        {
            int randomSkill = Random.Range(0, 3);
            switch (randomSkill)
            {
                case 0:
                    SpecialAttack();
                    break;
                case 1:
                    Heal(hpValue);
                    break;
                //case 2:
                //    SpawnMiniEnemy();
                //    break;
                case 2:
                    Teleport();
                    break;
            }
        }

        private void UseSkill()
        {
            if (Time.time >= nextSkill)
            {
                nextSkill = Time.time + skillCooldown;
                ChooseRandomSkill();
            }

        }

        protected override void Die()
        {
            isDead = true;
            animator?.SetTrigger("Die");
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 2f);
            //base.Die();
        }
    }
}
