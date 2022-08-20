using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    public bool isGrounded;
    Animator anim;
    int curHp;
    int maxHp = 3;
    bool isHit = false;
    public Main main;
    public bool key = false;
    bool canTP = true;
    public bool inWater = false;
    public bool inBadWater = false;
    bool isClimbing = false;
    int coins = 0;
    public bool canHit = true;
    public GameObject blueGem, greenGem;
    int gemCount = 0;
    float insideTimer = -1f;
    public float insideTimerUp = 30f;
    public Image insideCountDown;
    public Sprite ButtonDown;
    public Inventory inventory;
    public Soundeffector soundeffector;
    public Joystick joystick;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {

        if (inWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = true;
            if (joystick.Horizontal >= 0.2f || joystick.Horizontal <= -0.2f)
                Flip();
        }
        else if (inBadWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = false;
            jumpHeight = 0f;
            if (joystick.Horizontal >= 0.2f || joystick.Horizontal <= -0.2f)
                Flip();
        }
        else
        {
            if (!isGrounded && !isClimbing)
                anim.SetInteger("State", 3);
            if (joystick.Horizontal < 0.2f && joystick.Horizontal > -0.2f && (isGrounded) && !isClimbing)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing)
                    anim.SetInteger("State", 2);
            }
        }
        if (joystick.Horizontal >= 0.2f)
            rb.velocity = new Vector2(speed, rb.velocity.y); //Влево-вправо
        else if (joystick.Horizontal <= -0.2f)
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0f, rb.velocity.y);
        if (insideTimer >= 0f)
        {
            insideTimer += Time.deltaTime;
            if (insideTimer >= insideTimerUp)
            {
                insideTimer = 0f;
                RecountHp(-1);
            }
            else
                insideCountDown.fillAmount = 1 - (insideTimer / insideTimerUp);
        }

    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = Vector2.up * jumpHeight;//Прыжок
            soundeffector.PlayJumpSound();
        }
    }
    void Flip() //Поворот персонажа
    {
        if (joystick.Horizontal >= 0.2f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (joystick.Horizontal <= -0.2f)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public void RecountHp(int deltaHp)
    {
        
        if (deltaHp < 0 && canHit)
        {
            curHp = curHp + deltaHp;
            soundeffector.PlayHitSound();
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;
            StartCoroutine(OnHit());
        }
        else if (deltaHp > 0) {
            if (curHp >= maxHp)
                curHp = maxHp;
            else
                curHp = curHp + deltaHp;
        }
        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f); //вызов с задержкой
        }

        
    }

    IEnumerator OnHit()
    {
        if (isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
        if (GetComponent <SpriteRenderer>().color.g >= 1f)
        {
            canHit = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);

            yield break;
        }

        if (GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        }

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_key();
        }

        if (collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {

                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPwait());
                soundeffector.PlayDoorSound();
            }
            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coins++;
            soundeffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            //RecountHp(1);
            inventory.Add_hp();
        }

        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }

        if (collision.gameObject.tag == "BlueGem")
        {
            Destroy(collision.gameObject);
            //StartCoroutine(NoHit());
            inventory.Add_bg();
        }

        if (collision.gameObject.tag == "GreenGem")
        {
            Destroy(collision.gameObject);
            //StartCoroutine(SpeedBonus());
            inventory.Add_gg();
        }

        if (collision.gameObject.tag == "TimerBtnStart")
        {
            insideTimer = 0f;
            GetComponent<SpriteRenderer>().sprite = ButtonDown;
            soundeffector.PlayBtnSound();
        }

        if (collision.gameObject.tag == "TimerBtnStop")
        {
            insideTimer = -1f;
            insideCountDown.fillAmount = 0f;
            soundeffector.PlayBtnSound();
        }

    }

    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1.5f);
        canTP = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ledder")
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, 0);
            if (joystick.Vertical <= 0.5f )
            {
                anim.SetInteger("State", 5);
            } 
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * joystick.Vertical * speed * 0.3f * Time.deltaTime);
            }
            if(joystick.Vertical >= 0.5f)
            {
                anim.SetInteger("State", 6);
            }
            else
            {
                anim.SetInteger("State", 5);
                transform.Translate(Vector3.down * joystick.Vertical * speed *- 0.3f * Time.deltaTime);
            }
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 1f)
            {
                jumpHeight = 20f;
                anim.SetInteger("State", 7);
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 10f;
                speed *= 0.25f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ledder")
        {
            isClimbing = false;
            rb.gravityScale = 1f;
        }

        if (collision.gameObject.tag == "Icy")
        {
            if (rb.gravityScale == 10f)
            {
                jumpHeight = 12f;
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        if (collision.gameObject.tag == "QuickSand")
        {
            jumpHeight = 0f;
            speed *= 0.25f;
            rb.mass *= 100f;
        }
        if(collision.gameObject.tag == "Icy")
        {
            anim.SetInteger("State", 7);
        }

    }
    

    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isjump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isjump", false);
    }

    IEnumerator NoHit()
    {
        StopCoroutine(OnHit());
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);

        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        float t = 4f;
        while (t > 0f)
        {
            canHit = false;
            t -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;

        gemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
    }

    IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);

        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(5f);
        StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;

        gemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);

    }

    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
        }
    }

    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invis(spr, time));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "QuickSand")
        {
            jumpHeight = 12f;
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetHP()
    {
        return curHp;
    }

    public void BlueGem()
    {
        StartCoroutine(NoHit());
    }

    public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
    }

}

