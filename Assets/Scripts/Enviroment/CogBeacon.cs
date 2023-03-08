using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class CogBeacon : MonoBehaviour
{
    private bool found;
    private Animator anim;

    string cKey = "10450", mKey = "10339";
    public GameObject text;
    Vector3 pos = Vector3.zero;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!found)
        {
            anim.SetTrigger("Find");
            MessageFind();
        }
        else
        {
            MessageFind();
        }
    }

    private void MessageFind()
    {
        foreach (GameObject message in GameObject.FindGameObjectsWithTag("Message"))
        {
            Destroy(message);
        }
        LootLockerSDKManager.GetScoreList(mKey, 2000, 0, (response) =>
    {
        if (response.success)
        {
            StopAllCoroutines();
            StartCoroutine(Messages(response.items));
        }
        else
        {
            Debug.Log("Failed");
        }
    });
    }

    private IEnumerator Messages(LootLockerLeaderboardMember[] messages)
    {
        foreach (LootLockerLeaderboardMember mem in messages)
        {
            string msg = mem.metadata;
            string ID = mem.member_id;

            LootLockerSDKManager.GetMemberRank(cKey, ID, (response) =>
            {
                if (response.success)
                {
                    pos = StringToVector3(response.metadata);
                }
            });

            yield return new WaitForSeconds(0.5f);

            Debug.Log(pos);

            GameObject newText = Instantiate(text, pos, Quaternion.identity);
            newText.GetComponent<ReadMessage>().sTxt = msg;
        }
    }

    private Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
