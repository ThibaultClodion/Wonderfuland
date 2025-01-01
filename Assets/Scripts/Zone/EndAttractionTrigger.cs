using Cinemachine;
using UnityEngine;

public class EndAttractionTrigger : MonoBehaviour
{
    [TagField][SerializeField] private string _triggerTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _triggerTag)
        {
            SceneManager.Instance.LoadPreviousZone();
        }
    }
}
