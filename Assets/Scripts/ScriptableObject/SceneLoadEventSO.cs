using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="locationToLoad">Ҫ���صĳ���</param>
    /// <param name="posToGo">Player��Ŀ������</param>
    /// <param name="fadeScreen">�Ƿ��뽥��</param>
    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        Debug.Log($"RaiseLoadRequestEvent: ����: {locationToLoad.name}, λ��: {posToGo}, ���뵭��: {fadeScreen}");
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}