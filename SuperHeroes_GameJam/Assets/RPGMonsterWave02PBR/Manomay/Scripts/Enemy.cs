using System;
using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.RigidbodyPhysics;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private CharacterController _controller;
    private Rigidbody rigidbody;
    private Animator _animator;
    [SerializeField] private float roamingSpeed = 1.5f;
    [SerializeField] private float attackingMoveSpeed = 4f;
    private float speed = 0f;
    
    [SerializeField] private float rotationSpeed = 2f;
    private bool isRotating = false;
    private Quaternion rotationTarget=quaternion.identity;
    public GameObject waypointtarget;
    public GameObject playerTarget;

    private bool isinEmoteAction = false;

    private bool isAttacking = false;
    [SerializeField] private GameObject testTarget;
    

    public enum MoveMode
    {
        Roam,
        Attack
    }

    public MoveMode movemode = MoveMode.Roam;
    // Start is called before the first frame update
    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckForPlayer();
        
        if (playerTarget != null)
        {
            AttackMoveController();
            return;
        }
        
        if(waypointtarget!=null)
            RoamMoveController();
        
    }

    void CheckForPlayer()
    {
        if (Vector3.Distance(testTarget.transform.position, this.transform.position) < 10f)
        {
            if (playerTarget != testTarget)
            {
                playerTarget = testTarget;
                SawEnemy();

            }
        }
    }

    void RoamMoveController()
    {
        if (movemode == MoveMode.Roam)
        {
            LookTowards(waypointtarget);
            if (ShouldMove())
            {
                speed = roamingSpeed;
                _controller.Move(transform.forward * speed * Time.deltaTime);
            }
        }

        SetMoveAnimation(_controller.velocity.z);
    }

    void AttackMoveController()
    {
        if (movemode == MoveMode.Attack)
        {
            LookTowards(playerTarget);

            if (ShouldMove())
            {
                speed = attackingMoveSpeed;
                _controller.Move(transform.forward * speed * Time.deltaTime);
            }
        }

        SetMoveAnimation(_controller.velocity.z);
    }


    void SetMoveAnimation(float speed)
    {
        var modspeed = Mathf.Abs(speed);
        _animator.SetFloat("speed",modspeed);
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
        if(!isRotating)
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
        movemode = MoveMode.Attack;
        StartCoroutine(Sawplayer());
    }

    IEnumerator Sawplayer()
    {
        isinEmoteAction = true;
        _animator.SetLayerWeight(2,1);
        _animator.SetTrigger("taunt");
        yield return new WaitForSeconds(2f);
        _animator.SetLayerWeight(2,0);
        isinEmoteAction = false;
    }

    bool ShouldMove()
    {
        if (isinEmoteAction|| isAttacking)
        {
            return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            isAttacking = true;
            SetMoveAnimation(0f);
            Attack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            isAttacking = false;
            _animator.SetLayerWeight(1,0);
           
        }
    }

    void Attack()
    {
        _animator.SetLayerWeight(1,1f);
        _animator.SetTrigger("attack");
    }
}
