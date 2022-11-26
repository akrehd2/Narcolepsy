using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    public float vignette_speed;

    public Volume volume;
    public Vignette vignette;
    public DepthOfField depthOfField;
    public bool Is_On_corutine = false;
    public bool Hit = false;

    public float turnSpeed = 4.0f; // ���콺 ȸ�� �ӵ�
    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ���� ( ī�޶� �� �Ʒ� ���� )

    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out depthOfField);

        vignette.active = false;
        depthOfField.active = false;
        Is_On_corutine = false;
        Hit = false;
    }
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0,2,0);

        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;

        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        // Clamp �� ���� ������ �����ϴ� �Լ�
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        if (Hit && Is_On_corutine == false)
        {
            StartCoroutine(Hit_Effect());
        }
    }
    IEnumerator Hit_Effect()
    {
        vignette.active = true;
        Is_On_corutine = true;
        Hit = true;
        depthOfField.active = true;
        vignette.intensity.value = 0f;
        depthOfField.focalLength.value = 0;

        for (float i = 0; vignette.intensity.value <= 0.5f; i++)
        {
            vignette.intensity.value += vignette_speed * Time.smoothDeltaTime;
            depthOfField.focalLength.value += 100 * vignette_speed * Time.smoothDeltaTime;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.05f);

        for (float i = 0; vignette.intensity.value <= 0.5f; i++)
        {
            vignette.intensity.value -= vignette_speed * Time.smoothDeltaTime;
            depthOfField.focalLength.value -= 100 * vignette_speed * Time.smoothDeltaTime;
            yield return new WaitForSeconds(0.02f);
        }

        vignette.active = false;
        depthOfField.active = false;
        Is_On_corutine = false;
        Hit = false;
        yield return null;
    }
}