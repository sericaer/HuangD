using HuangD.Mods;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LuanchScene: MonoBehaviour
{
    void Awake()
    {
        Facade.mod = Mod.Builder.Build(Path.Combine(Application.streamingAssetsPath, "mods"));
    }

    public void OnStart()
    {
        SceneManager.LoadScene(nameof(MainScene), LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
