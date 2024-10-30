using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, llnteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;

    public void TriggerAction()
    {
        Debug.Log("传");

        if (loadEventSO == null)
        {
            Debug.LogError("SceneLoadEventSO is not assigned.");
            return;
        }

        if (sceneToGo == null)
        {
            Debug.LogError("GameSceneSO is not assigned.");
            return;
        }
        Debug.Log($"触发加载场景: {sceneToGo.name}，位置: {positionToGo}");
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }
    //public SceneLoader sceneLoader;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("Player entered teleport point.");
    //        sceneLoader.LoadNewScene();
    //    }
    //}
}
