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

    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;

    [Header("�㲥")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadedSceneEvent;

    [Header("����")]
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
            Debug.LogError("���ֶ�� SceneLoader ʵ�������ܻᵼ���¼����ĳ�ͻ��");
        }
        //DontDestroyOnLoad(this.gameObject);

        //DontDestroyOnLoad(playerTrans.gameObject);
    }

    //TODO:����MainMenu֮�����
    private void Start()
    {
        Debug.Log("SceneLoader ������");
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }

    private void OnEnable()
    {
        if (loadEventSO == null)
        {
            Debug.LogError("loadEventSO δ����ȷ���ã�");
        }
        newGameEvent.OnEventRaised += NewGame;
        Debug.Log("NewGame �¼��Ѷ���");
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        Debug.Log("OnLoadRequestEvent �¼��Ѷ���");

        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        //SceneManager.sceneLoaded -= OnSceneLoaded;

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        Debug.Log("NewGame �¼��ѽ������");
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        Debug.Log("OnLoadRequestEvent �¼��ѽ������");
        //SceneManager.sceneLoaded -= OnSceneLoaded;

        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;

        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // �ڳ���������ɺ�ִ�г�ʼ���߼�
    //    if (scene.name == "Forest")
    //    {
    //        InitializeSceneObjects();
    //    }
    //}

    //private void InitializeSceneObjects()
    //{
    //    // ��ʼ�������еĶ�����߼�
    //    Debug.Log("�����еĶ����ѳ�ʼ��");
    //}
    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    private void NewGame()
    {
        Debug.Log("NewGame������");
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
        Debug.Log("NewGame �� RaiseLoadRequestEvent �����ѵ���");
    }


    /// <summary>
    /// ���������¼�����
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>



    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        Debug.Log($"SceneLoader: OnLoadRequestEvent ������ - ���س���: {locationToLoad.name}");
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
        Debug.Log("��ʼж�ص�ǰ����");
        if (fadeScreen)
        {
            //TODO:���
            fadeEvent.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        //�㲥�¼�����Ѫ����ʾ
        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);
        Debug.Log("��ǰ����ж���¼��Ѵ���");


        yield return currentLoadedScene.sceneReference.UnLoadScene();
        Debug.Log("��ǰ����ж�����");
        //�ر�����
        playerTrans.gameObject.SetActive(false);


        //�����³���
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        Debug.Log($"��ʼ���س���: {sceneToLoad.sceneReference.RuntimeKey}");
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
        Debug.Log("�������ز���������");
    }

    /// <summary>
    /// ����������ɺ�
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {

        Debug.Log($"���� {sceneToLoad.sceneReference.RuntimeKey} �������");
        //// ���ĳ�����������¼�
        //SceneManager.sceneLoaded += OnSceneLoaded;
        // �����¼��صĳ���
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
            //����������ɺ��¼�
            Debug.Log("����������ɺ��¼��Ѵ���");
        afterSceneLoadedEvent.RaiseEvent();

    }

        //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    // ֻ����ǰ���صĳ���
        //    if (scene.name == sceneToLoad.sceneReference.RuntimeKey.ToString())
        //    {
        //        SceneManager.sceneLoaded -= OnSceneLoaded; // ȡ������

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
        //            Debug.Log("����������ɺ��¼��Ѵ���");
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
            Debug.Log("�������� OnLoadRequestEvent");
            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}

