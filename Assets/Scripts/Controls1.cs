using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls1 : MonoBehaviour
{
    public PlayerStateMachine PBS;
    public GameObject UI;


    private void Update()
    {
        if (Input.anyKey)
        {
            UI.SetActive(false);
            PBS.CanMove = true;
            Destroy(this);
        }
    }
}
