using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneSelection : MonoBehaviour
{
    [SerializeField] private Zone zone;

    public void LoadZone()
    {
        SceneManager.Instance.LoadAZone(zone);
    }
}
