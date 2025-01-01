using Controller;

static class ActionManager
{
    public static void Combat(params PlayerController[] playerControllers)
    {
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.ResetCommands();

            //Movements
            new CombatStickMoveCommand(PlayerInputs.LeftStickOrWASD, PlayerInputs.LeftStickOrWASD, playerController);
            //Spells
            new PreviousSpellCommand(PlayerInputs.LeftTriggerOrArrow, PlayerInputs.LeftTriggerOrArrow, playerController);
            new NextSpellCommand(PlayerInputs.RightTriggerOrArrow, PlayerInputs.RightTriggerOrArrow, playerController);
            new LaunchSpellCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, playerController);
            //EndTurn
            new EndTurnCommand(PlayerInputs.XOrSpace, PlayerInputs.XOrSpace, playerController);
            //Cameras
            new SwitchCombatCamera(PlayerInputs.SelectOrTab, PlayerInputs.SelectOrTab, playerController);
        }

        //new KeyboardCombatMoveCommand(PlayerInputs.RightStickOrMouse, PlayerInputs.RightStickOrMouse, playerController);
    }

    public static void Exploration(params PlayerController[] playerControllers)
    {
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.ResetCommands();
            new GamepadExplorationMoveCommand(PlayerInputs.LeftStickOrWASD, PlayerInputs.LeftStickOrWASD, playerController);
            new OpenMapCommand(PlayerInputs.SelectOrTab, PlayerInputs.SelectOrTab, playerController);
            new DashCommand(PlayerInputs.XOrSpace, PlayerInputs.XOrSpace, playerController);
        }

        //new KeyboardExplorationMoveCommand(PlayerInputs.LeftStickOrWASD, PlayerInputs.LeftStickOrWASD, playerController);
    }

    public static void ChoosingSpawnPosition(params PlayerController[] playerControllers)
    {
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.ResetCommands();
            new ReadyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, playerController);
            new NotReadyCommand(PlayerInputs.BOrE, PlayerInputs.BOrE, playerController);
            new SwitchCombatCamera(PlayerInputs.SelectOrTab, PlayerInputs.SelectOrTab, playerController);
            new StickChoosingSpawnMoveCommand(PlayerInputs.LeftStickOrWASD, PlayerInputs.LeftStickOrWASD, playerController);
        }

        /*new KeyboardChoosingPositionMoveCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, playerController);*/
    }

    public static void FreeCameraRotationCinematic(params PlayerController[] playerControllers)
    {
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.ResetCommands();
            new CinematicMoveCommand(PlayerInputs.RightStickOrMouse, PlayerInputs.RightStickOrMouse, playerController);
            new CinematicPauseCommand(PlayerInputs.YOrRightClick, PlayerInputs.YOrRightClick, playerController);
            new CinematicEndCommand(PlayerInputs.BOrE, PlayerInputs.BOrE, playerController);
        }
    }

}
