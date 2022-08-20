using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadWater : MonoBehaviour
{
    float timer = 0f;
    float timerHit = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            timer = 0;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (timer >= 1f)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().inBadWater = true;
            timerHit += Time.deltaTime;
            if (timerHit >= 2f)
            {
                collision.gameObject.GetComponent<Player>().RecountHp(-1);
                timerHit = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().inBadWater = false;
            timerHit = 0;
        }
    }
}
