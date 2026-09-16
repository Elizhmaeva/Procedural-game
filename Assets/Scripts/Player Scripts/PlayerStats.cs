using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public int health = 6;
    [SerializeField] private int numOfHearts = 6;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    public TMP_Text numsOfKeysText;
    public bool dmgImmunity = false;

    [SerializeField] private Image key;
    [SerializeField] private Sprite keySprite;
    public int numsOfKeys;
    [SerializeField] private GameObject UI;

    public void Start()
    {
        numsOfKeys = 0;
        numsOfKeysText.text = "x" + numsOfKeys.ToString();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Key"))
        {
            Debug.Log("Key Hit");
            numsOfKeys++;
            numsOfKeysText.text = "x" + numsOfKeys.ToString();
            SoundManager.PlaySound("miscCollect");
            other.gameObject.SetActive(false);
        }
        if (other.collider.CompareTag("Health"))
        {
            Debug.Log("Health Hit");
            health += 2;
            SoundManager.PlaySound("miscCollect");
            other.gameObject.SetActive(false);
        }
    }

    public bool useKeyOnDoor()
    {
        if (numsOfKeys>0)
        {
            SoundManager.PlaySound("doorOpen");

            numsOfKeys--;
            numsOfKeysText.text = "x" + numsOfKeys.ToString();
            return true;
        } else
        {
            SoundManager.PlaySound("doorClose");
            return false;
        }
    }

    void Update()
    {
        if (health>numOfHearts)
        {
            health = numOfHearts;
        }
        for (int i = 0; i < hearts.Length*2; i = i + 2)
        {
            if (i < health)
            {
                    hearts[i / 2].sprite = fullHeart;

            } else
            {
                hearts[i / 2].sprite = emptyHeart;
            }
            if (health%2==1)
            {
                hearts[health / 2].sprite = halfHeart;
            }
            if (i/2 < numOfHearts/2)
            {
                hearts[i / 2].enabled = true;
            } else
            {
                hearts[i / 2].enabled = false;
            }
        }

    }
}
