using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCamera : MonoBehaviour
{
    public GameObject player;
    public Camera firstperson;
    public Camera thirdperson;
    bool cam_check = false;
    public float width;
    public float height;

    private void Awake()
    {
        firstperson.enabled = true;
        thirdperson.enabled = true;
    }

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 5, 10);
    }
}
