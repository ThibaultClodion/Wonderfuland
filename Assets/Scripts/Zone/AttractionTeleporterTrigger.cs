using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttractionTeleporterTrigger : MonoBehaviour
{
    [Header("Attraction to TP")] 
    [SerializeField] private SceneAsset attractionScene;

    [Header("Timer")]
    private int nbReadyPlayer = 0;
    [SerializeField] private TextMeshProUGUI timerText;
    private float timerDelay = 3f;
    private float stepDelay = 1f;

    public void LoadCombatScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(attractionScene.name);
    }

    IEnumerator StartDelay(float delay, float step)
    {
        if (nbReadyPlayer != PlayerManager.playerControllers.Count)
        {
            ChangeTimerText("");
        }
        else
        {
            if (delay == 0)
            {
                LoadCombatScene();
            }
            else
            {
                ChangeTimerText(delay.ToString());
                yield return new WaitForSeconds(step);
                StartCoroutine(StartDelay(delay - step, step));
            }
        }
    }

    private void ChangeTimerText(string text)
    {
        if (timerText != null)
        {
            timerText.text = text;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nbReadyPlayer++;

            if (nbReadyPlayer == PlayerManager.playerControllers.Count)
            {
                StartCoroutine(StartDelay(timerDelay, stepDelay));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nbReadyPlayer--;
        }
    }
}
