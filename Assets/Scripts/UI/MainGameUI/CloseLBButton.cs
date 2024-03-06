using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseLBButton : MonoBehaviour
{
   public void CloseLBClick()
    {
        gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
