using Cinemachine;
using UnityEngine;

public class MapTeleporterTrigger : MonoBehaviour
{
    [TagField][SerializeField] private string _triggerTag;
    [SerializeField] private Map mapToTP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _triggerTag)
        {
            FightManager.Instance.InstantiateNewMap(mapToTP);
        }
    }
}
