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

    public float turnSpeed = 4.0f; // 마우스 회전 속도
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )

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

        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
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