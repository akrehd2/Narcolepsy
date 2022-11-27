using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody rigid;
    SpriteRenderer spriteRenderer;

    public GameObject Image;
    public GameObject over;
    public Fade fade;
    public GameObject[] cam;
    public Sprite[] sprites = new Sprite[2];

    public bool jump = false;
    public bool run = false;
    public float HP = 100.0f;
    public float score = 0.0f;

    public float turnSpeed = 4.0f; // 마우스 회전 속도
    public float speed = 10.0f; // 이동 속도

    public float stunTime;
    public float stunCool;
    public float bindTime;
    public bool stun = false;
    public bool bind = false;

    public TextMesh text;

    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        Image.gameObject.SetActive(true);
        rigid = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        fade = GameObject.Find("FadeImage").GetComponent<Fade>();
        //text.text = score.ToString();

        stunTime = Random.Range(1, 5);
}

    void Update()
    {
        MouseRotation();
        Ani();
        Narcolepsy();
        Jump();
    }

    void FixedUpdate()
    {
        KeyboardMove();
    }

    // 마우스의 움직임에 따라 카메라를 회전 시킨다.
    void MouseRotation()
    {
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }

    // 키보드의 눌림에 따라 이동
    void KeyboardMove()
    {
        // WASD 키 또는 화살표키의 이동량을 측정
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // 이동방향 * 속도 * 프레임단위 시간을 곱해서 카메라의 트랜스폼을 이동
        transform.Translate(dir * speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            run = true;
        }
        else
        {
            run = false;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jump == false)
        {
            rigid.AddForce(Vector3.up * 8f, ForceMode.Impulse);

            jump = true;
        }
    }

    void Ani()
    {
        if(run)
        {
            spriteRenderer.sprite = sprites[1];
        }
        else if(!run)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    void Narcolepsy()
    {
        if(stunTime > 0)
        {
            stunTime -= 1 * Time.deltaTime;
        }
        else if(stunTime <= 0)
        {
            stun = true;
        }

        if(stun)
        {
            fade.Fading = true;
            stun = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            jump = false;
        }

        if (other.gameObject.CompareTag("obastacle"))
        {
            over.gameObject.SetActive(true);
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Chair")
        {
            SceneManager.LoadScene("Stage7_diagnosis");
        }

        if (other.gameObject.tag == "next")
        {
            SceneManager.LoadScene("Stage6_inHospital");
        }

        if (other.gameObject.tag == "door")
        {
            SceneManager.LoadScene("Stage5_enteringCutScene");
        }

        if (other.gameObject.tag == "Potal")
        {
            SceneManager.LoadScene("Stage2_cutScene");
        }

        if (other.gameObject.tag == "Item_ScoreDOWN")
        {
            Destroy(other.gameObject);
            score += 150;
            text.text = score.ToString();
        }

        if (other.gameObject.tag == "Item_ScoreUP")
        {
            Destroy(other.gameObject);
            score -= 150;
            text.text = score.ToString();
        }
    }
}