using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class DatabaseConnect : MonoBehaviour
{
    void Awake()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Sucess");
            }
            else
            {
                Debug.Log("Error");
            }
        });
    }
}
