using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Controller;

class OpenDialogCommand : ScriptableObject
{

}
/*BaseCommand
{
    //UI
    private GameObject dialogCanvas;
    private GameObject actualDialogCanvas;
    private TextMeshProUGUI actualDialogText;

    //Dialogs
    private Interaction.Dialog[] dialogs;
    private int limitOfCharacter = 280;
    private int dialogIndex = 0;
    private int characterIndex = 0;

    //Player that trigger the dialog
    private PlayerController playerController;

    public void Init(Interaction.Dialog[] dialogs, GameObject dialogCanvas, PlayerController playerController)
    {
        this.dialogs = dialogs;
        this.dialogCanvas = dialogCanvas;
        this.playerController = playerController;
    }

    public void Disable()
    {
        if(dialogCanvas != null) 
        {
            Destroy(actualDialogCanvas);
        }
    }

    public void Execute(InputValue value)
    {
        if(GlobalUI.CanCreateAGlobalUI())
        {
            InstantiateCanvas();
        }
    }

    private void InstantiateCanvas()
    {
        actualDialogCanvas = Instantiate(dialogCanvas);

        //Reset text
        actualDialogText = actualDialogCanvas.GetComponentInChildren<TextMeshProUGUI>();
        actualDialogText.text = "";

        //Add Listener to OnClick Button
        actualDialogCanvas.GetComponentInChildren<Button>().onClick.AddListener(() => { UpdateDialog(); });

        //Linked the Player to the button
        playerController.eventSystem.SetSelectedGameObject(actualDialogCanvas.GetComponentInChildren<Button>().gameObject);

        UpdateDialog();
    }

    private void UpdateDialog()
    {
        //The entire dialog is finished
        if (dialogIndex >= dialogs.Length)
        {
            Disable();
        }
        else
        {
            if(characterIndex == 0)
            {
                actualDialogCanvas.GetComponentInChildren<RawImage>().texture = dialogs[dialogIndex].character;
            }

            //Isolate the words starting from the characterIndex
            string[] words = dialogs[dialogIndex].text.Substring(characterIndex).Split(new char[] { ' ' });
            int wordIndex = 0;
            string textToDisplay = "";


            //While we can add word to the text
            while (wordIndex < words.Length && textToDisplay.Length + words[wordIndex].Length <= limitOfCharacter)
            {
                textToDisplay += words[wordIndex];
                textToDisplay += " ";
                wordIndex++;
            }

            //Update characterIndex and display the text
            characterIndex += textToDisplay.Length;
            actualDialogText.text = textToDisplay;


            //The dialog[wordIndex] is finished
            if (wordIndex == words.Length)
            {
                characterIndex = 0;
                dialogIndex++;
            }
        }
    }
}*/