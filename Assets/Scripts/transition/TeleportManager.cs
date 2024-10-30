//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TeleportManager : MonoBehaviour
//{
//    public static TeleportManager Instance;

//    private Vector3 targetPosition;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // 确保在场景切换时不会销毁
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void SetTargetPosition(Vector3 position)
//    {
//        targetPosition = position;
//    }

//    public Vector3 GetTargetPosition()
//    {
//        return targetPosition;
//    }
//}
