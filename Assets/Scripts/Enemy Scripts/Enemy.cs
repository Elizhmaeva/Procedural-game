using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    walk,
    attack,
    idle,
    stunned
}

public class Enemy : MonoBehaviour
{
    public EnemyState currentState;

    public float waitTime;
    public float startWaitTime;
    public float decisionWaitTime;
    public float startDecisionWaitTime;
    public float moveSpeed;
    public Vector2 moveDirection;
    public int health;
    public string enemyName;
    public int baseAttack;
    public Rigidbody2D rb;

    public bool isIdle;
    public bool isWalk;
    public bool isAttack;
    public bool isStunned;

    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public SpriteRenderer mySprite;

    private void Idle()
    {

    }

    void Attack()
    {

    }

    void Walk()
    {

    }

    void Stunned()
    {

    }

    public void GetHit()
    {

    }
}
