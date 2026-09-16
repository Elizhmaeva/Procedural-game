using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : Enemy
{

    public Animator animator;
    public Transform target;
    public PolygonCollider2D playerTrigger;
    public GameObject heart;
    public float horizontal;
    public float vertical;
    public Vector2 playerPosition;
    private int wallBump;

    void Start()
    {
        isStunned = false;
        isAttack = false;
        isWalk = false;
        isIdle = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startWaitTime = 0.5f;
        moveSpeed = 150;
        waitTime = startWaitTime;
        moveDirection = new Vector2(0, 0);
        animator.SetBool("isWalking", false);
        flashDuration = 0.08f;
        numberOfFlashes = 3;
        currentState = EnemyState.idle;
        mySprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.walk:
                Walk();
                break;
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.attack:
                Attack();
                break;
            case EnemyState.stunned:
                Stunned();
                break;
            default:
                Debug.Log("do nothing");
                break;

        }
    }
    private void FixedUpdate()
    {
        if (currentState.Equals(EnemyState.walk) || currentState.Equals(EnemyState.attack) || currentState.Equals(EnemyState.stunned))
        {
            moveDirection = moveDirection.normalized;
            rb.linearVelocity = new Vector2((Mathf.Round((moveDirection.x * moveSpeed * Time.deltaTime) / 0.0625f) * 0.0625f), (Mathf.Round((moveDirection.y * moveSpeed * Time.deltaTime) / 0.0625f) * 0.0625f));
        }
        else
        {
            rb.linearVelocity = new Vector2(0, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!currentState.Equals(EnemyState.stunned))
        {
            if (other.gameObject.CompareTag("Player"))

            {

                isAttack = true;
                moveDirection = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y);

                currentState = EnemyState.attack;
            }
        }
        else
        {
            isAttack = false;
        }

    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (!currentState.Equals(EnemyState.stunned))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isAttack = true;
                moveDirection = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y);
                currentState = EnemyState.attack;
            }
        }
        else
        {
            isAttack = false;
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttack = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {

            wallBump++;
            Debug.Log("wall" + wallBump);
            isWalk = false;
            isAttack = false;
            moveSpeed = 150f;
            waitTime = 1f;
            animator.SetBool("isWalking", false);
            currentState = EnemyState.idle;
        }
    }

    void Stunned()
    {
        if (isStunned)
        {
            waitTime = 0.4f;
            animator.SetBool("isWalking", false);
            moveSpeed = 150f;
            isWalk = false;
            isAttack = false;
            isIdle = false;
            isStunned = false;
        }
        else
        {
            if (waitTime <= 0)
            {
                waitTime = 0.5f;
                rb.linearVelocity = Vector2.zero;
                isIdle = true;
                currentState = EnemyState.idle;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void Attack()
    {
        if (isAttack)
        {
            waitTime = 1f;
            animator.SetBool("isWalking", true);
            moveSpeed = 200f;
            wallBump = 0;
            horizontal = moveDirection.normalized.x;
            vertical = moveDirection.normalized.y;
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        } else
        {
            if (waitTime < 0)
            {
                isWalk = false;
                isAttack = false;
                moveSpeed = 150f;
                waitTime = 1f;
                wallBump = 0;
                currentState = EnemyState.idle;
            } else
            {
                waitTime -= Time.deltaTime;
            }
        }

    }
    void Walk()
    {
        if (waitTime<=0)
        {
            waitTime = 0.5f;
            currentState = EnemyState.idle;
            animator.SetBool("isWalking", false);
        } else
        {
            waitTime -= Time.deltaTime;
        }
    }

    void Idle()
    {
        if (waitTime <=0)
        {

            currentState = EnemyState.walk;
            animator.SetBool("isWalking", true);

            isIdle = false;
        } else
        {
            animator.SetBool("isWalking", false);
            waitTime -= Time.deltaTime;
            return;
        }
        if (!isIdle)
        {

            isIdle = true;
            if (wallBump==1)
            {
                waitTime = 0.5f;
                vertical = -vertical;
                horizontal = -horizontal;
                playerTrigger.transform.Rotate(new Vector3(0, 0, 180f));
            } else if (wallBump>1)
            {
                waitTime = 0.5f;
                vertical = horizontal;
                horizontal = vertical;
                float rotation;
                if (horizontal!=0)
                {
                    rotation = (horizontal > 0.01f) ? 90f : 270f;
                } else
                {
                    rotation = (vertical > 0.01f) ? 180f : 0f;
                }

                playerTrigger.transform.localEulerAngles = new Vector3(0f, 0f, rotation);
                wallBump = 0;
            } else
            {
                wallBump = 0;
                switch (Random.Range(0, 4))
                {
                    case 0:
                        playerTrigger.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                        vertical = 1;
                        horizontal = 0;
                        break;
                    case 1:
                        playerTrigger.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        vertical = 0;
                        horizontal = 1;
                        break;
                    case 2:
                        playerTrigger.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                        vertical = -1;
                        horizontal = 0;
                        break;
                    case 3:
                        playerTrigger.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
                        vertical = 0;
                        horizontal = -1;
                        break;
                    default:
                        break;
                }
            }
            waitTime = Random.Range(1f, 3f);
            moveDirection = new Vector2(horizontal, vertical);
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }
    }

    new public void GetHit()
    {
        Debug.Log("Health: " + health);
        health -= 1;
        if (health > 0)
        {
            SoundManager.PlaySound("skeletonHit");
            StartCoroutine(FlashCo(false));
        }
        else
        {

            SoundManager.PlaySound("skeletonDeath");
            StartCoroutine(FlashCo(true));

        }
    }

    private IEnumerator FlashCo(bool death)
    {
        int temp = 0;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        if (death)
        {
            if (Random.Range(0,1000)<15)
            {
                Instantiate(heart, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.identity);
            }

            this.gameObject.SetActive(false);
        }

    }

}
