using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{

    // Load scene by name
    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
