using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Player player;

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    private Image image;

    public bool Fading = false;
    public bool FadeOut = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if(Fading)
        {
            StartCoroutine(fade(1, 0));
        }

        if(FadeOut)
        {
            StartCoroutine(fade(0, 1));
        }
    }

    private IEnumerator fade(float start, float end)
    {
        float currentTime = Random.Range(-3f, -5.0f);
        float percent = 0.0f;

        while(percent<1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            if (Fading)
            {
                //yield return new WaitForSeconds(Random.Range(1f*Time.deltaTime,5f * Time.deltaTime));
                Fading = false;
                FadeOut = true;
            }
            else if(FadeOut && player.stunTime <= 0)
            {
                player.stunTime = Random.Range(3, 5);
                Fading = false;
            }
            else if(player.stunTime > 0)
            {
                FadeOut = false;
            }

            yield return null;
        }
    }
}
