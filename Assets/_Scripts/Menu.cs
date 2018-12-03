using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public string scene;
    private bool next;

    public void PlayGame()
    {
        if (next)
            return;
        next = true;
        LoadingScreen.Instance.LoadScene(scene);
    }

    public void QuitGame()
    {
        if (next)
            return;
        next = true;
        Application.Quit();
        Debug.Log("Quit");
    }
}
