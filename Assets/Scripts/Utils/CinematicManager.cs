using Controller;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager instance;

    [Header("Clip")]
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private List<PlayableAsset> clip;
    [SerializeField] private List<CinematicTrigger> cinematicTrigger;
    private int cinematicIndex = 0;
    private bool isInPause = false;

    [Header("Game Objects")]
    [SerializeField] public RotateCamera cinematicCamera;
    [SerializeField] private List<GameObject> goToEnableInCinematic;
    [SerializeField] private List<GameObject> goToDisableInCinematic;
    [SerializeField] private Transform wagon;

    private void Start()
    {
        //Only one CinematicManager is possible by scene
        instance = this;
        playableDirector.stopped += EndCinematic;
    }

    public void StartCinematic()
    {
        if (cinematicIndex >= cinematicTrigger.Count || GameState.actualState == GameState.State.InCinematic) return;
        GameState.IsInCinematic();

        ChangeCharactersActivity(false); //Disable characters
        ChangeGOActivity(goToEnableInCinematic, true);
        ChangeGOActivity(goToDisableInCinematic, false);

        playableDirector.playableAsset = clip[cinematicIndex];
        playableDirector.Play();

        cinematicIndex++;
        isInPause = false;
    }

    private void EndCinematic(PlayableDirector director)
    {
        //The combat could have start before.
        if (GameState.actualState == GameState.State.InCinematic) GameState.IsInExploration();

        ChangeCharactersActivity(true); //Reanable characters
        ChangeGOActivity(goToEnableInCinematic, false);
        ChangeGOActivity(goToDisableInCinematic, true);

        isInPause = true;
    }

    public void End()
    {
        playableDirector.Resume();  //In case it's in pause
        playableDirector.time = playableDirector.duration;
    }

    public void Pause()
    {
        if(!isInPause) playableDirector.Pause();
        else playableDirector.Resume();

        isInPause = !isInPause;
    }

    private void ChangeGOActivity(List<GameObject> gameObjects, bool isActive)
    {
        if (!this.gameObject.scene.isLoaded) return;    //Without this you have error message when stop playing

        foreach (GameObject go in gameObjects)
        {
            go.SetActive(isActive);
        }
    }

    private void ChangeCharactersActivity(bool isActive)
    {
        foreach(PlayerController player in PlayerManager.playerControllers)
        {
            foreach(GameObject characterGo in player.charactersGO)
            {
                characterGo.SetActive(isActive);
            }
        }
    }
}