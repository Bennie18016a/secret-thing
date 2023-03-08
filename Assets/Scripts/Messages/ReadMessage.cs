using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadMessage : MonoBehaviour
{
    GameObject player;
    public TMP_Text text;
    public string sTxt;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && InRange())
        {
            text.text = sTxt;
        }

        if (!InRange())
        {
            text.text = null;
        }
    }

    private bool InRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) < 5;
    }
}
