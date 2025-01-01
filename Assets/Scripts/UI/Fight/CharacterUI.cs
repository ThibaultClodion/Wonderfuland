using Controller;
using UnityEngine;

namespace UI
{
    class CharacterUI : MonoBehaviour
    {
        public SpellBar spellBarGO;
        public CharacterInfos characterInfos;
        public CharacterChoice characterChoice;
        [HideInInspector] public bool canPlay = true;

        private Character character;
        private PlayerController playerController = null;

        // doit revoir une liste des personnages
        public void Initialize(PlayerController _playerController, ref Character _character)
        {
            character = _character;
            playerController = _playerController;
            spellBarGO.Initialize(OnLaunchSpell, character);
            characterChoice.Initialize(playerController.SetPlayingCharacter, playerController.characters);
            characterInfos.Display(character);
        }

        public void UpdateCurrentCharacter(ref Character _character)
        {
            character = _character;
            UpdateDisplayInfo();
            DisplaySpellBar();
        }

        public void OnLaunchSpell(int index)
        {

        }

        public void UpdateDisplayInfo()
        {
            characterInfos.Display(character);
        }

        public void DisplaySpellBar()
        {
            spellBarGO.Initialize(OnLaunchSpell, character);
        }

        // Events UI
        public void EndTurn()
        {
            if (canPlay)
            {
                playerController.EndCharacterTurn();
            }
        }
    }
}