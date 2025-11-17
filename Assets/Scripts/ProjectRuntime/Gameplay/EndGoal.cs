using ProjectRuntime.Managers;
using ProjectRuntime.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BattleManager.Instance.CompleteLevel();
        }
    }
}
