using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void MlBTN()
    {
        SceneManager.LoadScene(1);
    }
    public void AiBTN()
    {
        SceneManager.LoadScene(2);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
