﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isHit = false;
    public GameObject drop;
    public int health = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isHit) {
            collision.gameObject.GetComponent<Player>().RecountHp(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 4f, ForceMode2D.Impulse);
        }
    }

    public IEnumerator Death()
    {
        health--;
        if (health <= 0)
        {

            if (drop != null)
            {
                Instantiate(drop, transform.position, Quaternion.identity);
            }
            isHit = true;
            GetComponent<Animator>().SetBool("Dead", true);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Collider2D>().enabled = false;
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }

    public void startDeath()
    {
        StartCoroutine(Death());
    }
}