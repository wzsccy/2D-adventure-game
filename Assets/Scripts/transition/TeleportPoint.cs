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
        Debug.Log("��");

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
        Debug.Log($"�������س���: {sceneToGo.name}��λ��: {positionToGo}");
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
