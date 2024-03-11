using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private BackGroundType _type; 

    public BackGroundType GetBGType()
    {
        return _type;
    }
}
public enum BackGroundType
{
    Left_BackGround,
    Right_BackGround
}
