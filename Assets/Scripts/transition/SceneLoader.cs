using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;

    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;

    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadedSceneEvent;

    [Header("场景")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;

    private void Awake()
    {
        // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        // currentLoadedScene = firstLoadScene;
        // currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        SceneLoader[] sceneLoaders = FindObjectsOfType<SceneLoader>();
        if (sceneLoaders.Length > 1)
        {
            Debug.LogError("发现多个 SceneLoader 实例，可能会导致事件订阅冲突！");
        }
        //DontDestroyOnLoad(this.gameObject);

        //DontDestroyOnLoad(playerTrans.gameObject);
    }

    //TODO:做完MainMenu之后更改
    private void Start()
    {
        Debug.Log("SceneLoader 已启动");
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }

    private void OnEnable()
    {
        if (loadEventSO == null)
        {
            Debug.LogError("loadEventSO 未被正确引用！");
        }
        newGameEvent.OnEventRaised += NewGame;
        Debug.Log("NewGame 事件已订阅");
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        Debug.Log("OnLoadRequestEvent 事件已订阅");

        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        //SceneManager.sceneLoaded -= OnSceneLoaded;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        Debug.Log("NewGame 事件已解除订阅");
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        Debug.Log("OnLoadRequestEvent 事件已解除订阅");
        //SceneManager.sceneLoaded -= OnSceneLoaded;

        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // 在场景加载完成后执行初始化逻辑
    //    if (scene.name == "Forest")
    //    {
    //        InitializeSceneObjects();
    //    }
    //}

    //private void InitializeSceneObjects()
    //{
    //    // 初始化场景中的对象和逻辑
    //    Debug.Log("场景中的对象已初始化");
    //}
    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    private void NewGame()
    {
        Debug.Log("NewGame已启动");
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
        Debug.Log("NewGame 中 RaiseLoadRequestEvent 方法已调用");
    }


    /// <summary>
    /// 场景加载事件请求
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>



    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        Debug.Log($"SceneLoader: OnLoadRequestEvent 被调用 - 加载场景: {locationToLoad.name}");
        if (isLoading)
            return;

        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (currentLoadedScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }



    private IEnumerator UnLoadPreviousScene()
    {
        Debug.Log("开始卸载当前场景");
        if (fadeScreen)
        {
            //TODO:变黑
            fadeEvent.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        //广播事件调整血条显示
        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);
        Debug.Log("当前场景卸载事件已触发");


        yield return currentLoadedScene.sceneReference.UnLoadScene();
        Debug.Log("当前场景卸载完成");
        //关闭人物
        playerTrans.gameObject.SetActive(false);


        //加载新场景
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        Debug.Log($"开始加载场景: {sceneToLoad.sceneReference.RuntimeKey}");
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
        Debug.Log("场景加载操作已启动");
    }

    /// <summary>
    /// 场景加载完成后
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {

        Debug.Log($"场景 {sceneToLoad.sceneReference.RuntimeKey} 加载完成");
        //// 订阅场景加载完成事件
        //SceneManager.sceneLoaded += OnSceneLoaded;
        // 激活新加载的场景
        Scene newScene = obj.Result.Scene;
        SceneManager.SetActiveScene(newScene);
        
      
        currentLoadedScene = sceneToLoad;

        playerTrans.position = positionToGo;

        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //TODO:
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;
        if (currentLoadedScene.sceneType == SceneType.location)
            //场景加载完成后事件
            Debug.Log("场景加载完成后事件已触发");
        afterSceneLoadedEvent.RaiseEvent();

    }

        //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    // 只处理当前加载的场景
        //    if (scene.name == sceneToLoad.sceneReference.RuntimeKey.ToString())
        //    {
        //        SceneManager.sceneLoaded -= OnSceneLoaded; // 取消订阅

        //        SceneManager.SetActiveScene(scene);
        //        currentLoadedScene = sceneToLoad;

        //        playerTrans.position = positionToGo;
        //        playerTrans.gameObject.SetActive(true);

        //        if (fadeScreen)
        //        {
        //            fadeEvent.FadeOut(fadeDuration);
        //        }

        //        isLoading = false;
        //        if (currentLoadedScene.sceneType == SceneType.location)
        //        {
        //            Debug.Log("场景加载完成后事件已触发");
        //            afterSceneLoadedEvent.RaiseEvent();
        //        }
        //    }
        //}

        public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadedScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID].ToVector3();
            sceneToLoad = data.GetSavedScene();
            Debug.Log("即将调用 OnLoadRequestEvent");
            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}

