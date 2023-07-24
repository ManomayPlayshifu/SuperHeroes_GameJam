using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : NetworkBehaviour
{
    //private CharacterController _controller;
    private Rigidbody rigidbody;
    private Animator _animator;

    [Header("Player Detection")] [SerializeField]
    private float detectPlayerFromDistance = 1.5f;

    [Header("Move Options")] [SerializeField]
    private float roamingSpeed = 1.5f;

    [SerializeField] private float attackingMoveSpeed = 4f;
    private float speed = 0f;
    public MoveMode movemode = MoveMode.Roam;

    [Header("Move Options")] [SerializeField]
    private float rotationSpeed = 2f;

    private bool isRotating = false;
    private Quaternion rotationTarget = quaternion.identity;
    [HideInInspector] public GameObject waypointtarget;
    [HideInInspector] public GameObject playerTarget;

    [Header("Attack Options")] [SerializeField]
    private float attackSpeed = 1.5f;
    [SerializeField] private GameObject hitEffect;
    private GameObject hiteffectref;
    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }

    private bool isinEmoteAction = false;
    private bool isAttacking = false;
    private bool enableaxe = false;

    public bool EnableAxeCollider
    {
        get => enableaxe;
        set => enableaxe = value;
    }

    public bool Attacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }

    [SerializeField] private GameObject testTarget;
    private Coroutine attack;

    [Header("Health Options")] [SerializeField]
    private float enemyMaxHealth = 100f;

    [SerializeField] public Slider healthSlider;
    
    [SyncVar(hook = nameof(SetHealthUIHook))]
    [SerializeField] private float currentHealth = 100f;

    public float enemyHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    [SerializeField] private GameObject deatheffect;


    NetworkAnimator networkAnimator;

    public enum MoveMode
    {
        Roam,
        Attack
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!NetworkServer.active)
            return;

        healthSlider.maxValue = enemyMaxHealth;
        healthSlider.value = currentHealth;
        networkAnimator = GetComponent<NetworkAnimator>();
        //_controller = gameObject.GetComponent<CharacterController>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkServer.active)
            return;
        CheckEnemyHealth();
        CheckForPlayer();
        //currentHealth -=Time.deltaTime * 2f;
    }

    private void FixedUpdate()
    {
        if (!NetworkServer.active)
            return;

        if (playerTarget != null)
        {
            AttackMoveController();
            return;
        }

        if (waypointtarget != null)
            RoamMoveController();
    }

    void CheckForPlayer()
    {
        if (RoomData.instance.playerObjs.Count <= 0)
        {
            return;
        }

        var dic = new Dictionary<int, float>();
        var list = RoomData.instance.playerObjs;
        for (int i = 0; i < list.Count; i++)
        {
            dic.Add(i, Vector3.Distance(list[i].transform.position, this.transform.position));
        }

        var playerobjIndex = FindSmallestValue(dic).Key;

        testTarget = RoomData.instance.playerObjs[playerobjIndex];

        if (testTarget == null)
            return;

        if (Vector3.Distance(testTarget.transform.position, this.transform.position) < 10f)
        {
            if (playerTarget != testTarget)
            {
                movemode = MoveMode.Attack;
                playerTarget = testTarget;
                SawEnemy();
            }
        }
        else if (Vector3.Distance(testTarget.transform.position, this.transform.position) > 15f)
        {
            movemode = MoveMode.Roam;
            playerTarget = null;
        }
    }

    public KeyValuePair<int, float> FindSmallestValue(Dictionary<int, float> dictionary)
    {
        var sortedDictionary = dictionary.OrderBy(x => x.Value);
        KeyValuePair<int, float> smallestEntry = sortedDictionary.First();

        return smallestEntry;
    }

    void RoamMoveController()
    {
        if (movemode == MoveMode.Roam)
        {
            LookTowards(waypointtarget);
            if (ShouldMove())
            {
                speed = roamingSpeed;
                //_controller.Move(transform.forward * speed * Time.deltaTime);
                rigidbody.velocity = transform.forward * speed;
            }
        }

        SetMoveAnimation(rigidbody.velocity.z);
    }

    void AttackMoveController()
    {
        if (movemode == MoveMode.Attack)
        {
            LookTowards(playerTarget);

            if (ShouldMove())
            {
                speed = attackingMoveSpeed;
                //_controller.Move(transform.forward * speed * Time.deltaTime);
                rigidbody.velocity = transform.forward * speed;
            }
        }

        SetMoveAnimation(rigidbody.velocity.z);
    }


    void SetMoveAnimation(float speed)
    {
        var modspeed = Mathf.Abs(speed);
        _animator.SetFloat("speed", modspeed);
    }

    void LookTowards(GameObject obj)
    {
        Vector3 directionToTarget = obj.transform.position - transform.position;
        directionToTarget.Normalize();
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        lookRotation.eulerAngles = new Vector3(0f, lookRotation.eulerAngles.y, 0f);
        if (lookRotation == rotationTarget)
            return;

        rotationTarget = lookRotation;
        if (!isRotating)
            StartCoroutine(RotateToTarget(lookRotation));
    }

    IEnumerator RotateToTarget(Quaternion targetrotation)
    {
        var targetRotation = targetrotation;
        var initialRotation = transform.rotation;
        initialRotation.Normalize();
        targetRotation.Normalize();

        isRotating = true;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }

    void SawEnemy()
    {
        StartCoroutine(Sawplayer());
    }

    IEnumerator Sawplayer()
    {
        isinEmoteAction = true;
        _animator.SetLayerWeight(2, 1);
        if (networkAnimator != null)
        {
            networkAnimator.SetTrigger("taunt");
        }
        else
        {
            _animator.SetTrigger("taunt");
        }

        yield return new WaitForSeconds(2f);
        _animator.SetLayerWeight(2, 0);
        isinEmoteAction = false;
    }

    bool ShouldMove()
    {
        if (isinEmoteAction || isAttacking)
        {
            return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkServer.active)
            return;

        if (other.gameObject.GetComponent<SuperCharacterController>())
        {
            SetMoveAnimation(0f);
            if (attack == null)
                attack = StartCoroutine(PlayAttackAnim());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!NetworkServer.active)
            return;

        if (other.gameObject.GetComponent<SuperCharacterController>())
        {
            StartCoroutine(WaitForAttackFinish());
            StopCoroutine(PlayAttackAnim());
        }
    }

    IEnumerator WaitForAttackFinish()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1f);
        StopAttack();
    }

    IEnumerator PlayAttackAnim()
    {
        isAttacking = true;
        _animator.SetLayerWeight(1, 1f);
        var count = 0;
        while (isAttacking)
        {
            // var v = Random.Range(0, 2);

            var v = count % 2;
            count++;
            if (v == 0)
            {
                Attack1();
                Debug.Log("attack1");
            }
            else
            {
                Attack2();
                Debug.Log("attack1");
            }

            EnableAxeCollider = true;
            yield return new WaitForSeconds(attackSpeed);
            EnableAxeCollider = false;
        }

        attack = null;
    }

    void Attack1()
    {
        if (networkAnimator != null)
        {
            networkAnimator.SetTrigger("attack");
            return;
        }

        _animator.SetTrigger("attack");
    }

    void Attack2()
    {
        if (networkAnimator != null)
        {
            networkAnimator.SetTrigger("attack2");
            return;
        }

        _animator.SetTrigger("attack2");
    }

    void StopAttack()
    {
        isAttacking = false;
        _animator.SetLayerWeight(1, 0f);
    }

    public void DecreaseEnemyHealth(float value)
    {
        CMDDecreaseEnemyHealth(value);
        
    }


    [Command(requiresAuthority = false)]
    public void CMDDecreaseEnemyHealth(float value)
    {
        

        if (!NetworkServer.active)
        {
            Debug.LogError("CallingOnClient");
            return;
        }

        if (networkAnimator != null)
        {
            _animator.SetLayerWeight(1, 1f);
            networkAnimator.SetTrigger("gethit");
            StartCoroutine(ReduceLayerWeight());
        }

        currentHealth -= value;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

       
    }




    IEnumerator ReduceLayerWeight()
    {
        yield return new WaitForSeconds(1f);
        if (!isAttacking)
            _animator.SetLayerWeight(1, 0f);
    }

    public void IncreaseEnemyHealth(float value)
    {
        if (NetworkClient.active)
        {
            Debug.LogError("CallingOnClient");
            return;
        }

        currentHealth += value;
        if (currentHealth >= enemyMaxHealth)
        {
            currentHealth = enemyMaxHealth;
        }
    }

    private void CheckEnemyHealth()
    {
        if (currentHealth <= 0)
        {
            EnemyDied();
        }
    }

    void EnemyDied()
    {
        GameObject obj = Instantiate(deatheffect, transform.position, Quaternion.identity);
        NetworkServer.Spawn(obj);
        Debug.Log($"{this.gameObject.name} has been destroyed");
        NetworkServer.Destroy(this.gameObject);
    }
    
    void SetHealthUIHook(float oldvalue,float newvalue)
    {
        healthSlider.value = currentHealth;
        if (currentHealth / enemyMaxHealth <= 0.3f)
        {
            healthSlider.image.color = Color.red;
        }
    }

    public void SpawnHitEffect(Transform t)
    {
        hiteffectref = Instantiate(hitEffect, t.position, Quaternion.identity);
        NetworkServer.Spawn(hiteffectref);
        StartCoroutine(SpawnHitEffectForSeconds(hiteffectref));
    }

    IEnumerator SpawnHitEffectForSeconds(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        NetworkServer.Destroy(obj);
    }
}