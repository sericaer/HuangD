using CommandTerminal;
using HuangD.Commands;
using HuangD.Interfaces;
using HuangD.Mods;
using HuangD.Sessions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            var seed = System.Guid.NewGuid().ToString();
            //var seed = "1ba206dd-5b8d-4d90-bde6-96ddc9453ca0";
            Debug.Log($"Seed:{seed}");
            Facade.session = Session.Builder.Build(new HuangD.Maps.MapInit() { 
                width = 120, 
                high = 80, 
                terrainPercents = new Dictionary<TerrainType, int>()
                {
                    {TerrainType.Plain, 55 },
                    {TerrainType.Hill, 25 },
                    {TerrainType.Mount, 10}
                }},
                seed,
                Facade.mod.defs,
                (info)=> RunOnMainThread.Enqueue(() => UpdateBroad(info)));

            RunOnMainThread.Enqueue(() =>
            {
                UpdateBroad("½øÈëÓÎÏ·");
                SceneManager.LoadScene(nameof(MainScene), LoadSceneMode.Single);
            });
        }).ContinueWith(_=>
        {
            if (_.Exception?.InnerException is { } inner)
            {
                Debug.Log(String.Format("{0}: {1} \n {2}",
                    inner.GetType().Name,
                    inner.Message,
                    inner.StackTrace));
            }
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

    void UpdateBroad(string info)
    {
        board.GetComponentInChildren<Text>().text = info;
        Debug.Log(info);
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
