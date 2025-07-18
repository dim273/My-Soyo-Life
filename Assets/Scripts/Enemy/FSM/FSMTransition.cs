using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSMTransition
{
    // 根据Decision的返回值决定进入哪个状态
    public FSMDesicion Desicion;
    public string TrueState;    
    public string FalseState;
}
