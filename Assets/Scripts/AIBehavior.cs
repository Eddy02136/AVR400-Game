using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AIBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isFleeing;
    private bool isChasing;
    public bool isRabbit;
    public bool isBear;
    private Transform player;
    private PlayerStats playerStats;
    private float damage;
    private float attackRange;
    private float attackCooldown;
    private float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(SetTarget());
        player = GameObject.FindWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        damage = 15f;
        attackRange = 1.5f;
        attackCooldown = 1f;
        lastAttackTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity == Vector3.zero)
        {
            agent.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        } else if (isFleeing || isChasing)
        {
            if (isRabbit)
            {
                agent.gameObject.GetComponent<Animator>().SetInteger("State", 1);
            } else
            {
                agent.gameObject.GetComponent<Animator>().SetInteger("State", 2);
            }
            agent.speed = 5;
        } else
        {
            agent.gameObject.GetComponent<Animator>().SetInteger("State", 1);
            if (isRabbit)
            {
                agent.speed = 2;
            } else
            {
                agent.speed = 1;
            }
        }

        if (isBear)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                agent.gameObject.GetComponent<Animator>().SetInteger("State", 3);
                lastAttackTime = Time.time;
                playerStats.TakeDamage(damage);
            }
        }
    }

    IEnumerator SetTarget()
    {
        while (true)
        {
            Vector3 target = agent.transform.position + Random.insideUnitSphere * 30;
            if (NavMesh.SamplePosition(target, out var hit, 30, NavMesh.AllAreas))
            {
                agent.destination = hit.position;
            }
            yield return new WaitForSeconds(30);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        StopCoroutine(SetTarget());
        if (other.CompareTag("Player"))
        {
            if (isBear)
            {
                isChasing = true;
                agent.destination = other.transform.position;
            }
            else
            {
                isFleeing = true;
                Vector3 fleeDirection = transform.position - other.transform.position;
                Vector3 fleeTarget = transform.position + fleeDirection.normalized * 30;

                if (NavMesh.SamplePosition(fleeTarget, out var hit, 30, NavMesh.AllAreas))
                {
                    agent.destination = hit.position;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isFleeing = false;
        isChasing = false;
        StartCoroutine(SetTarget());
    }
}
