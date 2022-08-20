using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pashalka : MonoBehaviour
{
    public GameObject[] block;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            foreach (GameObject obj in block)
            {
                Destroy(obj);
            }
        }
    }
}
