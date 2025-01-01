using Cinemachine;
using UnityEditor.UIElements;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    [TagField] [SerializeField] private string _triggerTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _triggerTag)
        {
            CinematicManager.instance.StartCinematic();
        }
    }
}
