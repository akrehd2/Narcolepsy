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

    public float turnSpeed = 4.0f; // ���콺 ȸ�� �ӵ�
    public float speed = 10.0f; // �̵� �ӵ�

    public float stunTime;
    public float stunCool;
    public float bindTime;
    public bool stun = false;
    public bool bind = false;

    public TextMesh text;

    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ���� ( ī�޶� �� �Ʒ� ���� )

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

    // ���콺�� �����ӿ� ���� ī�޶� ȸ�� ��Ų��.
    void MouseRotation()
    {
        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        // Clamp �� ���� ������ �����ϴ� �Լ�
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }

    // Ű������ ������ ���� �̵�
    void KeyboardMove()
    {
        // WASD Ű �Ǵ� ȭ��ǥŰ�� �̵����� ����
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // �̵����� * �ӵ� * �����Ӵ��� �ð��� ���ؼ� ī�޶��� Ʈ�������� �̵�
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