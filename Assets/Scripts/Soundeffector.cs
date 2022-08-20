using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundeffector : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound, coinSound, winSound, loseSound, doorSound, btnSound, hitSound;

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinSound);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

    public void PlayDoorSound()
    {
        audioSource.PlayOneShot(doorSound);
    }
    public void PlayBtnSound()
    {
        audioSource.PlayOneShot(btnSound);
    }
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }

}
