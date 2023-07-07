using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private static FPSCounter instance;
    public static FPSCounter Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    float fps;
    float ms;
    public Text fpsTxt;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        fps = 1.0f / Time.deltaTime;
        ms = Time.deltaTime * 1000;
        fpsTxt.text = $"FPS : {fps:F2} ({ms:F1}ms)";
    }
}
