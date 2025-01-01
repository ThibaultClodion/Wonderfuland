using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUI : MonoBehaviour
{
    public static bool CanCreateAGlobalUI()
    {
        return GameObject.FindGameObjectsWithTag(TagManager.GLOBAL_UI_TAG).Length == 0;
    }
}
