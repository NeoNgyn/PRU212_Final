﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemy.EnemyLv2
{
    internal class ArcherBoss : EnemyController
    {
        private Animator animator;
        private bool isAttacking = false;
        private float attackCooldown = 1.5f;
        private float lastAttackTime = 0f;
        //protected bool isDead = false;
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private Transform firePoint;

        [SerializeField] private GateTriggerBoss gateTrigger;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }
        protected override void Update()
        {
            if (IsDead()) return;
            base.Update();
            if (player != null && !isAttacking)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < 20f && Time.time - lastAttackTime > attackCooldown)
                {
                    Shoot();
                }
            }
        }
        protected override void MoveToPlayer()
        {
            if (player == null) return;
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < 5f)
            {
                Vector2 dirAway = (transform.position - player.transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position,
                                                         (Vector2)transform.position + dirAway,
                                                         enemySpeed * Time.deltaTime);
            }
            else
            {
                base.MoveToPlayer();
            }
        }
        private void Shoot()
        {
            if (IsDead()) return;
            isAttacking = true;
            lastAttackTime = Time.time;

            animator?.SetTrigger("Shoot");

            Invoke(nameof(SpawnArrow), 0.5f);

            Invoke(nameof(ResetAttack), attackCooldown);
        }

        private void SpawnArrow()
        {
            if (arrowPrefab != null && player != null && firePoint != null)
            {
                GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

                Vector2 direction = (player.transform.position - firePoint.position).normalized;

                Arrow arrowScript = arrow.GetComponent<Arrow>();
                if (arrowScript != null)
                {
                    arrowScript.SetDirection(direction);

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    Debug.LogWarning("Prefab ko cc script Arrow gan vao");
                }
            }
            else
            {
                Debug.LogWarning("arrowPrefab, player hoac firePoint bi null!");
            }
        }


        private void ResetAttack()
        {
            isAttacking = false;
        }

        protected override void Die()
        {
            //isDead = true;
            animator?.SetTrigger("Death");
            if (gateTrigger != null)
            {
                gateTrigger.OpenGate();
            }
            base.Die();
            //Destroy(gameObject, 2f);
        }
    }
}
