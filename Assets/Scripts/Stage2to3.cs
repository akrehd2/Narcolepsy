using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2to3 : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("Stage3");
    }
}
