using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticTargetGroup : MonoBehaviour
{
    private void OnEnable()
    {
        UpdateGroup();
    }

    private void Start()
    {
        UpdateGroup();
    }

    public void UpdateGroup()
    {
        CinemachineTargetGroup cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player != null) 
            {
                cinemachineTargetGroup.AddMember(player.transform, 1, 1);
            }
        }
    }
}
