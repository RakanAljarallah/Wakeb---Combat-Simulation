using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _LoaderCanvas;
    [SerializeField] private Image _progressBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MainMenu Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void loadScene( )
   {
       
       _LoaderCanvas.SetActive(true);
     var scene =   SceneManager.LoadSceneAsync("Main-1");
     
     scene.allowSceneActivation = false;
        
     
     do
     {
        _progressBar.fillAmount = scene.progress;
     } while (scene.progress < 0.9f);
     
     scene.allowSceneActivation = true;
     _LoaderCanvas.SetActive(false);
   }
}
