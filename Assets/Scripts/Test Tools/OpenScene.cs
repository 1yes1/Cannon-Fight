using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenScene : MonoBehaviour
{
    public static bool cheatBool;

    bool lost = false;
    
    private void Start()
    {
        cheatBool = false;

    }
    public void OpenGameScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Restart()
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(y);
    }
}
