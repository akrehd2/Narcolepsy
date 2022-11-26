using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1toCutScene : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Potal")
        {
            SceneManager.LoadScene("Stage2_cutScene");
        }
    }
}
