using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Controller
{
    class PlayerController : CharacterController
    {
        //Linked Character Data's
        [SerializeField] private List<CharacterData> charactersData;
        private int characterPlayedIndex;

        //Movement Data's
        [NonSerialized] public Vector3 movement;
        private float moveSpeed = 7.0f;
        
        //Dash
        private float dashPower = 4f;
        private float dashTime = 0.2f;
        private float dashCooldown = 0.2f;
        private bool isDashing = false;
        private bool canDash = true;

        //Actions Data's
        [SerializeField] public PlayerInput playerInput;
        [SerializeField] public MultiplayerEventSystem eventSystem;
        [NonSerialized] public BaseCommand leftStickOrWASDCommand;
        [NonSerialized] public BaseCommand rightStickOrMouseCommand;
        [NonSerialized] public BaseCommand aOrLeftClickCommand;
        [NonSerialized] public BaseCommand xOrSpaceommand;
        [NonSerialized] public BaseCommand yOrRightClickCommand;
        [NonSerialized] public BaseCommand leftTriggerOrArrowCommand;
        [NonSerialized] public BaseCommand rightTriggerOrArrowCommand;
        [NonSerialized] public BaseCommand selectOrTabCommand;
        [NonSerialized] public BaseCommand startOrEchapCommand;
        [NonSerialized] public BaseCommand bOrECommand;

        //Spell Data's
        [SerializeField] public GameObject spellVisualisationPrefab;
        [NonSerialized] public GameObject spellVisualisationGO;
        [NonSerialized] public Vector2 spellVisualisationMovement;
        private CellEffect rangedSpellVisualisation = null;
        private Color rangedVisualisationColor = new Color32(178, 132, 33, 160);
        private List<Vector2Int> rangedPositions;
        [NonSerialized] public int spellIndex;
        private Coroutine spellVisualisationCoroutine;

        //Spawn Data's
        [NonSerialized] public bool hasSpawnSet;

        // UI
        private CharacterUI characterUI;

        public Character GetCurrentCharacter() { return characters[characterPlayedIndex]; }
        public GameObject GetCurrentCharacterGO() { return charactersGO[characterPlayedIndex]; }
        public Rigidbody GetCurrentCharacterRB() { return charactersGORigidbody[characterPlayedIndex]; }

        private void Awake()
        {
            characterPlayedIndex = 0;
            DontDestroyOnLoad(this);

            ////Permit Player to join in exploration
            //if (GameState.isInExploration)
            //{
            //    InstantiateCharacter(charactersData[i] , MapManager.Instance.zone.GetNextSpawnPosition());
            //    GameObject.Find("Target Group").GetComponent<AutomaticTargetGroup>().UpdateGroup();
            //}
        }

        private void FixedUpdate()
        {
            if (GameState.actualState == GameState.State.InExploration && GetCurrentCharacterRB() != null)
            {
                if (GetCurrentCharacterRB() == null) return ;

                if (isDashing)
                {
                    GetCurrentCharacterRB().velocity = movement * moveSpeed * dashPower;
                }
                else
                {
                    GetCurrentCharacterRB().velocity = movement * moveSpeed;
                }

                if (movement.magnitude >= 0.1f)
                {
                    GetCurrentCharacterRB().transform.forward = movement;
                }
            }
        }

        public new void Clear()
        {
            base.Clear();
            characterPlayedIndex = 0;
        }

        public void InstantiateCharacters(List<Vector2Int> positions)
        {
            for (int i = 0; i < positions.Count && i < charactersData.Count; i++)
            {
                InstantiateCharacter(charactersData[i], positions[i]);
            }
        }

        public void InstantiateCurrentCharacter(Vector2Int position)
        {
            InstantiateCharacter(charactersData[characterPlayedIndex], position);
        }

        public void InstantiateCharacter(CharacterData characterData, Vector2Int position)
        {
            int characterIndex = InstantiateCharacter(characterData, Character.CharacterTeam.BLUE, position);

            //Instantiate spell visualisation
            if (GameState.actualState == GameState.State.InCombat || GameState.actualState == GameState.State.ChoosingSpawnPosition)
            {
                spellVisualisationGO = Instantiate(spellVisualisationPrefab);
                MapManager.Instance.PutGameObjectOnCurrentMap(spellVisualisationGO, position, spellVisualisationGO.transform.position.y);
                spellVisualisationGO.SetActive(false);
            }
        }

        public void SetPlayingCharacter(int i)
        {
            spellIndex = 0;
            characterPlayedIndex = i;
            Character character = characters[characterPlayedIndex];
            characterUI.UpdateCurrentCharacter(ref character);
        }

        public void EndCharacterTurn()
        {
            charactersCanPlay[characterPlayedIndex] = false;

            if (!charactersCanPlay.Contains(true))
            {
                FightManager.Instance.EndCurrentTurn();
                return;
            }

            for (int i = 0; i < charactersCanPlay.Count; i++)
            {
                if (charactersCanPlay[i])
                {
                    SetPlayingCharacter(i);
                }
            }
        }

        public void SetCharacterUI(GameObject characterUIGO)
        {
            Character character = characters[characterPlayedIndex];
            characterUI = characterUIGO.GetComponent<CharacterUI>();
            characterUI.Initialize(this, ref character);
        }

        public void OnDeviceLost()
        {
            //DestroyCharacter();
            PlayerManager.playerControllers.Remove(this);

            //Destroy Linked Canvas if there is one existing
            if (eventSystem.currentSelectedGameObject != null)
            {
                //If the Canvas had a Destroy Option
                if (eventSystem.currentSelectedGameObject.GetComponentInParent<DestroyCanvas>() != null)
                {
                    eventSystem.currentSelectedGameObject.GetComponentInParent<DestroyCanvas>().SelfDestroy();
                }
                //If the Canvas must be give to another player
                else if (PlayerManager.playerControllers.Count > 0)
                {
                    PlayerManager.playerControllers[0].eventSystem.SetSelectedGameObject(eventSystem.currentSelectedGameObject);
                }
                //If there is no other player
                else
                {
                    PlayerManager.Instance.mainCanvasIsSelected = false;
                }
            }

            Destroy(gameObject);
        }

        public void OnDeviceRegained()
        {
            PlayerManager.Instance.OnPlayerJoined(playerInput);
        }

        public void CanPlay()
        {
            for (int i = 0; i < charactersCanPlay.Count; i++)
            {
                charactersCanPlay[i] = true;
            }

            if (characterUI != null)
            {
                characterUI.canPlay = true;
                characterUI.UpdateDisplayInfo();
            }

            //Display the Visualisation of Spells
            spellVisualisationGO.SetActive(true);
            spellVisualisationGO.transform.localPosition = new Vector3(characters[characterPlayedIndex].position.x, spellVisualisationGO.transform.localPosition.y, characters[characterPlayedIndex].position.y);
            spellVisualisationMovement = Vector2.zero;
            StartVisualisationCoroutine();
            DisplayRangeVisualisationSpell();
        }

        public void CantPlay()
        {
            if (characterUI != null)
            {
                characterUI.canPlay = false;
                characterUI.UpdateDisplayInfo();
            }

            DestroyRangeVisualisationSpell();
            StopSpellVisualisationCoroutine();
            spellVisualisationGO.SetActive(false);
        }

        public void ResetCommands()
        {
            movement = Vector3.zero; // Reset movement

            leftStickOrWASDCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            rightStickOrMouseCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            aOrLeftClickCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            xOrSpaceommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            yOrRightClickCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            leftTriggerOrArrowCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            rightTriggerOrArrowCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            selectOrTabCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            startOrEchapCommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
            bOrECommand = new EmptyCommand(PlayerInputs.AOrLeftClick, PlayerInputs.AOrLeftClick, this);
        }

        public void GridMove(Vector2Int newPosition)
        {
            MovementManager.MoveCharacter(newPosition, this);
        }

        public void Dash()
        {
            if (canDash)
            {
                StartCoroutine(CoroutineDash());
            }
        }

        private IEnumerator CoroutineDash()
        {
            //Is Dashing
            canDash = false;
            isDashing = true;
            yield return new WaitForSeconds(dashTime);

            //Is in cooldown
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);

            //Cooldown finish
            canDash = true;
        }

        #region Spell
        public void LaunchSpell()
        {
            Vector2Int position = new Vector2Int((int)spellVisualisationGO.transform.localPosition.x, (int)spellVisualisationGO.transform.localPosition.z);

            //If the selected cell is in the reach of spell
            if (rangedPositions.Contains(position) && spellIndex >= 0)
            {
                characters[characterPlayedIndex].LaunchSpell(spellIndex, position);
                characterUI.UpdateDisplayInfo();
            }
        }

        public void PreviousSpell()
        {
            if (spellIndex == 0) return;

            spellIndex--;
            DisplayRangeVisualisationSpell();
        }

        public void NextSpell()
        {
            if (spellIndex == characters[characterPlayedIndex].spellDeck.spells.Count - 1) return;

            spellIndex++;
            DisplayRangeVisualisationSpell();
        }

        private void DisplayRangeVisualisationSpell()
        {
            DestroyRangeVisualisationSpell();

            rangedPositions = characters[characterPlayedIndex].spellDeck.spells[spellIndex].getAvailableLaunchPositions(characters[characterPlayedIndex].position);
            if (rangedPositions.Count > 0)
            {
                rangedSpellVisualisation = EffectPool.SetEffects(rangedPositions, rangedVisualisationColor);
            }
        }

        private void DestroyRangeVisualisationSpell()
        {
            if (rangedSpellVisualisation != null)
            {
                rangedSpellVisualisation.RemoveEffect();
                rangedSpellVisualisation = null;
            }
        }

        public void StopSpellVisualisationCoroutine()
        {
            if (spellVisualisationCoroutine == null) return;

            StopCoroutine(spellVisualisationCoroutine);
            spellVisualisationCoroutine = null;
        }

        public void StartVisualisationCoroutine()
        {
            if (spellVisualisationCoroutine != null) return;

            spellVisualisationCoroutine = StartCoroutine(MoveSpellVisualisation());
        }

        IEnumerator MoveSpellVisualisation()
        {
            Vector2Int coordinate = Vector2Int.RoundToInt(spellVisualisationMovement + new Vector2Int((int)spellVisualisationGO.transform.localPosition.x, (int)spellVisualisationGO.transform.localPosition.z));
            MovementManager.MoveVisualisationEffect(spellVisualisationGO, coordinate);

            yield return new WaitForSeconds(0.15f);
            spellVisualisationCoroutine = StartCoroutine(MoveSpellVisualisation());
        }
        #endregion

        #region V2-InputManagement
        public void OnLeftStickOrWASD(InputValue value)
        {
            leftStickOrWASDCommand.Execute(value);
        }
        public void OnRightStickOrMouse(InputValue value)
        {
            rightStickOrMouseCommand.Execute(value);
        }
        public void OnAOrLeftClick(InputValue value)
        {
            aOrLeftClickCommand.Execute(value);
        }
        public void OnXOrSpace(InputValue value)
        {
            xOrSpaceommand.Execute(value);
        }
        public void OnYOrRightClick(InputValue value)
        {
            yOrRightClickCommand.Execute(value);
        }
        public void OnLeftTriggerOrArrow(InputValue value)
        {
            leftTriggerOrArrowCommand.Execute(value);
        }
        public void OnRightTriggerOrArrow(InputValue value)
        {
            rightTriggerOrArrowCommand.Execute(value);
        }
        public void OnSelectOrTab(InputValue value)
        {
            selectOrTabCommand.Execute(value);
        }
        public void OnStartOrEchap(InputValue value)
        {
            startOrEchapCommand.Execute(value);
        }
        public void OnBOrE(InputValue value)
        {
            bOrECommand.Execute(value);
        }
        #endregion
    }
}