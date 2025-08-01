using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour  where T : MonoBehaviour
{
    // ����һ��ͨ�õĵ���ģʽ
    public static T instance {  get; private set; }

    private void Awake()
    {
        instance = this as T;
    }
}
