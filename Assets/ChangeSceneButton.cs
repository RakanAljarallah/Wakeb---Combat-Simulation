using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeScene()
    {
        MainMenu.Instance.loadScene();
    }
}
