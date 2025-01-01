public static class GameState
{
    public enum State
    {
        ChoosingSpawnPosition,
        ChoosingAZone,
        InCombat,
        InExploration,
        InCinematic
    }

    public static State actualState = State.ChoosingAZone;

    public static void IsChoosingSpawnPosition()
    {
        actualState = State.ChoosingSpawnPosition;
        ActionManager.ChoosingSpawnPosition(PlayerManager.playerControllers.ToArray());
        UIManager.Instance.RenderSpawnSelectionUI();
    }

    public static void IsInCombat()
    {
        actualState = State.InCombat;
        ActionManager.Combat(PlayerManager.playerControllers.ToArray());
        UIManager.Instance.RenderFightUI();
    }

    public static void IsInExploration()
    {
        actualState = State.InExploration;
        ActionManager.Exploration(PlayerManager.playerControllers.ToArray());
        UIManager.Instance.RenderExplorationUI();
    }

    public static void IsInCinematic()
    {
        actualState = State.InCinematic;
        ActionManager.FreeCameraRotationCinematic(PlayerManager.playerControllers.ToArray());
    }

    public static void IsChoosingAZone()
    {
        actualState = State.ChoosingAZone;
        PlayerManager.Instance.ResetMovements();
    }

    public static void IsStoppingChoosingAZone()
    {
        actualState = State.InExploration;
        UIManager.Instance.RenderExplorationUI();
    }
}
