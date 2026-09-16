using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkeletraxAI : Enemy
{
    int maxHealth;
    GameObject bossUI;
    public BoxCollider2D boxCollider;
    public RectInt room;
    bool playBossMusic;

    void Start()
    {
        name = "Skeletrax";
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        boxCollider.size = room.size;
        health = 20;
        maxHealth = health;
        bossUI = GameObject.FindGameObjectWithTag("BossHP");
        bossUI.SetActive(false);
        playBossMusic = false;
        currentState = EnemyState.idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                Idle();
                break;
        }
    }

    private void Idle()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playBossMusic)
            {
                FindObjectOfType<MusicSoundManager>().ChangeMusic("bossMusic");
                playBossMusic = true;
            }

            bossUI.SetActive(true);
            if (!bossUI.Equals(null))
            {
                bossUI.GetComponentInChildren<TMP_Text>().text = name;
                bossUI.GetComponentInChildren<Slider>().maxValue = maxHealth;
                bossUI.GetComponentInChildren<Slider>().value = health;
            }

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!bossUI.Equals(null))
            {
                bossUI.GetComponentInChildren<TMP_Text>().text = name;
                bossUI.GetComponentInChildren<Slider>().maxValue = maxHealth;
                bossUI.GetComponentInChildren<Slider>().value = health;
            }
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
            this.gameObject.SetActive(false);
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
            FindObjectOfType<GameManager>().EndGame(true);

        }
    }
}
