using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class Messaging : MonoBehaviour
{
    public GameObject text;
    public TMP_InputField inputField;
    public PlayerStateMachine psm;

    public void Confirm()
    {
        if (inputField.text == null)
        {
            Cancel();
            return;
        }

        GameObject newText = Instantiate(text, psm.transform.position, Quaternion.identity);
        newText.GetComponent<ReadMessage>().sTxt = inputField.text;
        Cancel();
        UploadMessage(newText, inputField.text);
    }

    public void Cancel()
    {
        psm.Texting = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        psm.CanMove = true;

        psm.TextMenu.SetActive(false);
    }

    private void UploadMessage(GameObject newText, string txt)
    {
        int mID = Random.Range(100000, 100000000);
        string cKey = "10450", mKey = "10339";
        int index = GameObject.FindGameObjectsWithTag("Message").Length + 1;

        LootLockerSDKManager.SubmitScore(mID.ToString(), index, cKey, newText.transform.position.ToString(), (response) => { });
        LootLockerSDKManager.SubmitScore(mID.ToString(), index, mKey, txt, (response) => { });
    }
}
