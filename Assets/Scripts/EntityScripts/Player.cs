using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.GameManage;
using GameFramework.TipScripts;
using UnityEngine;
using UnityEngine.Serialization;

public class CurrentMoveSnap
{
    public Vector2 CurrentVel;
    public float CurrentAngleVel;
}
public class Player : MonoBehaviour
{
    [Header("移动参数")]
    public float maxSpeed;
    public float currentSpeed;
    public float accelerate;
    public float deAccelerate;
    public Vector2 TargetVec => new Vector2(GameApp.inputSystem.x,GameApp.inputSystem.y).normalized;
    [Header("旋转参数")] 
    public float rotateSteer;
    public float angleVelocity;
    public float angle;
    public float offsetAngle;
    [Header("血量参数")]
    public float maxHealth;
    public float currentHealth;
    [Header("操纵参数")] 
    public bool underControll = true;

    [Header("快照")] private CurrentMoveSnap _moveSnap;
    private Rigidbody2D _rb;
    //玩家事件
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        _moveSnap = new();
    }

    public void FixedUpdate()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        if (!underControll)
        {
            LoseControll();
            return;
        }
        AdjustVec();
        Accelerate();
    }

    public void AdjustVec()
    {
        if (TargetVec == Vector2.zero)
        {
            _rb.angularVelocity = 0;
        }
        if (!GameApp.inputSystem.xy_Inspector())
        {
            return;
        }
        angle = Vector3.SignedAngle(transform.up, TargetVec,new Vector3(0,0,1));
        _rb.angularVelocity = Math.Sign(angle) * angleVelocity;
        if (Mathf.Abs(angle)< offsetAngle)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward,TargetVec);
            _rb.angularVelocity = 0;
        }
    }
    
    public void Accelerate()
    {
        _rb.velocity = transform.up * (currentSpeed * Time.deltaTime);
        if (TargetVec != Vector2.zero)
        {
            currentSpeed += accelerate * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deAccelerate * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0f,maxSpeed);
    }

    private void LoseControll()
    {
        UpdateCurrentMoveStateSnapshot();
        _rb.velocity = _moveSnap.CurrentVel;
        _rb.angularVelocity = _moveSnap.CurrentAngleVel;
    }
    
    public void UpdateCurrentMoveStateSnapshot()
    {
        _moveSnap.CurrentVel = _rb.velocity;
        _moveSnap.CurrentAngleVel = _rb.angularVelocity;
    }

    public void PlayerBounceBack(Vector2 dir)
    {
        dir = dir.normalized;
        float bounceAngle = Vector3.SignedAngle(transform.up, dir,new Vector3(0,0,1));
    }
}
