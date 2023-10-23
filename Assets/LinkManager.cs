using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour
{


    public void OpenPlayStore()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.PixeltubGames.TicTacTeo");
    }
    public void AboutUs()
    {
        Application.OpenURL("https://pixeltubgames.com");
    }


}