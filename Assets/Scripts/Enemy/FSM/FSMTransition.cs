using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSMTransition
{
    // ����Decision�ķ���ֵ���������ĸ�״̬
    public FSMDesicion Desicion;
    public string TrueState;    
    public string FalseState;
}
