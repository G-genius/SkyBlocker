using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    float playerX;
    public float speed;
    Animator anim;
    bool isAngry = false;
    public Sprite halfHeart;
    public Transform groundDetect;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, 1f);

        if (isAngry)
        {
            playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
            if(groundInfo.collider)
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (playerX < transform.position.x)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else if (playerX > transform.position.x)
                transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (GetComponent<Enemy>().health == 1)
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = halfHeart;
        else if (GetComponent<Enemy>().health <= 0)
            transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision) //При касании будет двигаться за игроком
    {
        if(collision.gameObject.tag == "Player")
        {
            isAngry = true;
            anim.SetBool("isAngry", true);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
