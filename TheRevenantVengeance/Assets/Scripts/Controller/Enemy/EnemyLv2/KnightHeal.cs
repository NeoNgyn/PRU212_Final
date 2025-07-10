﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemy.EnemyLv2
{
    public class KnightHeal : EnemyController
    {
        [SerializeField] private GameObject heartObject;
        [SerializeField] private float healValue = 20f;
        private Animator animator;
        private bool isAttacking = false;
        private readonly float attackCooldown = 1f;
        private float lastAttackTime = 0f;
        protected bool isDead = false;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip takeHitSound;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip deathSound;
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (isDead) return;
            base.Update();
            if (player != null && !isAttacking)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < 1.5f && Time.time - lastAttackTime > attackCooldown)
                {
                    Attack();
                }
            }
        }

        private void Attack()
        {
            if (isDead) return;
            isAttacking = true;
            lastAttackTime = Time.time;

            animator?.SetTrigger("Attack");
            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
            // Gây sát thương sau 0.5s (tùy theo frame animation)
            Invoke(nameof(DealDamageToPlayer), 0.5f);

            // Cho phép attack lại sau cooldown
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        private void DealDamageToPlayer()
        {
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
            {
                player.TakeDamage(enterDamage); // Hoặc attackDamage riêng
            }
        }

        private void ResetAttack()
        {
            isAttacking = false;
        }

        public override void TakeDamage(float damage, Vector2 knockbackDirection)
        {
            base.TakeDamage(damage, knockbackDirection);
            if (currentHp > 0)
            {
                animator?.SetTrigger("TakeHit");
                if (audioSource != null && takeHitSound != null)
                {
                    audioSource.PlayOneShot(takeHitSound);
                }
            }
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

        protected override void Die()
        {
            isDead = true;
            animator?.SetTrigger("Die");
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
            if (heartObject != null)
            {
                GameObject heart = Instantiate(heartObject, transform.position, Quaternion.identity);
                Destroy(heart, 5f);
            }

            Destroy(gameObject, 2f);
        }

        private void HealPlayer()
        {
            if (player != null)
                player.Heal(healValue);
        }
    }

}
