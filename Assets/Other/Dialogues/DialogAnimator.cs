using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAnimator : MonoBehaviour
{
    public Animator startAnim;
    public DialogManager dm;
    public Sprite finsprite;

    public void OnTriggerEnter2D(Collider2D other)
    {
        startAnim.SetBool("StartOpen", true);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = finsprite;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        startAnim.SetBool("StartOpen", false);
        dm.EndDialog();
    }
}
