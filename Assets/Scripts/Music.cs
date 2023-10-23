using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music instance;

    private void Awake()
    {
        if (instance == null)
        {
            // If no instance exists, make this the instance
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            // An instance already exists, so destroy this duplicate
            Destroy(this.gameObject);
        }
    }
}
