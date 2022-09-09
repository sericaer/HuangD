using CommandTerminal;
using HuangD.Commands;
using HuangD.Interfaces;
using HuangD.Mods;
using HuangD.Sessions;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LuanchScene: MonoBehaviour
{
    public GameObject menu;
    public GameObject board;

    private static readonly ConcurrentQueue<Action> RunOnMainThread = new ConcurrentQueue<Action>();

    void Awake()
    {
        Facade.mod = Mod.Builder.Build(Path.Combine(Application.streamingAssetsPath, "mods"));
    }

    public void OnStart()
    {
        Task.Run(() =>
        {

            var mapSize = 80;

            Facade.session = Session.Builder.Build(mapSize, "DEFAULT", Facade.mod.defs);

            RunOnMainThread.Enqueue(() => SceneManager.LoadScene(nameof(MainScene), LoadSceneMode.Single));
        });

        //SceneManager.LoadSceneAsync(nameof(MainScene), LoadSceneMode.Single);

        //Debug.Log("OnClick");
        //ComputeAsync();

        //var task = GenerateSession();
        menu.SetActive(false);
        board.SetActive(true);

        //Facade.session = task.Result;

        //SceneManager.LoadSceneAsync(nameof(MainScene), LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(true);
        board.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        while (RunOnMainThread.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }
}
