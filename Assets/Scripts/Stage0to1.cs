using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage0to1 : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("Stage1");
    }
}
