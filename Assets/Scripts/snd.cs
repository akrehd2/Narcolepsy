using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snd : MonoBehaviour
{
    AudioSource audioSource;

    float time = 5f;
    bool doo = false;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        time = Random.Range(5f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > -2f)
        {
            time -= 1f * Time.deltaTime;
        }
        else if(time <= -2f)
        {
            time = Random.Range(5f, 15f);
            doo = false;
        }

        if (time <= 0f && !doo)
        {
            audioSource.Play();
            doo = true;
        }
    }
}
