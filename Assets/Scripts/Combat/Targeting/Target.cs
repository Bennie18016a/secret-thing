using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> onDestroyed;

    private void OnDestroy()
    {
        onDestroyed?.Invoke(this);
    }
}
