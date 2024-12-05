using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCInteractionWithChoices : MonoBehaviour
{
    public GameObject interactionText; // Prompt text (e.g., "Press E to Talk")
    public GameObject dialogueBox; // Dialogue box to display text
    public GameObject choicesBox; // Choices box to display options
    public Text dialogueText; // Text element for dialogue content
    public Text npcNameText; // Text element to show NPC's name
    public Button choice1Button; // Button for first choice
    public Button choice2Button; // Button for second choice
    public Text choice1Text; // Text component inside Choice1Button
    public Text choice2Text; // Text component inside Choice2Button

    [TextArea(3, 5)]
    public string[] dialogues; // Array of dialogues
    public string choicePrompt; // Dialogue where choices are presented
    public string[] choice1Dialogues; // Dialogue path for "Yes" choice
    public string[] choice2Dialogues; // Dialogue path for "No" choice
    public string npcName; // NPC name
    public string choice1Label; // Text for the first choice (Yes)
    public string choice2Label; // Text for the second choice (No)

    private int currentDialogueIndex = 0;
    private bool isPlayerNearby = false;
    private bool isInConversation = false;
    private bool hasMadeChoice = false; // Tracks if a choice has been made

    void Start()
    {
        interactionText.SetActive(false);
        dialogueBox.SetActive(false);
        choicesBox.SetActive(false);
        dialogueText.text = ""; // Ensure dialogue text starts empty
        npcNameText.text = ""; // Ensure NPC name starts empty

        // Assign button listeners
        choice1Button.onClick.AddListener(() => MakeChoice(1));
        choice2Button.onClick.AddListener(() => MakeChoice(2));

        // Set the choice labels
        choice1Text.text = choice1Label;
        choice2Text.text = choice2Label;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInConversation)
        {
            isPlayerNearby = true;
            interactionText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EndConversation();
            isPlayerNearby = false;
            interactionText.SetActive(false); // Hide interaction prompt when leaving
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isInConversation)
        {
            StartConversation();
        }

        if (isInConversation && Input.GetMouseButtonDown(0) && !hasMadeChoice) // Left mouse button for dialogue
        {
            ShowNextDialogue();
        }
    }

    void StartConversation()
    {
        isInConversation = true;
        interactionText.SetActive(false); // Turn off interaction text
        dialogueBox.SetActive(true);
        npcNameText.text = npcName; // Show NPC's name
        ShowNextDialogue(); // Show the first dialogue
    }

    void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];

            // Check if this is the choice dialogue
            if (dialogues[currentDialogueIndex] == choicePrompt)
            {
                ShowChoices();
            }
            else
            {
                currentDialogueIndex++;
            }
        }
        else
        {
            EndConversation();
        }
    }

    void ShowChoices()
    {
        dialogueBox.SetActive(false); // Hide dialogue box
        choicesBox.SetActive(true); // Show choices box
    }

    void MakeChoice(int choice)
    {
        hasMadeChoice = true; // Set choice flag to true
        choicesBox.SetActive(false); // Hide choices box
        dialogueBox.SetActive(true); // Show dialogue box

        // Show the correct path based on the player's choice
        if (choice == 1)
        {
            // Show "Yes" dialogue path
            StartCoroutine(ShowChoiceDialogues(choice1Dialogues));
        }
        else if (choice == 2)
        {
            // Show "No" dialogue path
            StartCoroutine(ShowChoiceDialogues(choice2Dialogues));
        }
    }

    IEnumerator ShowChoiceDialogues(string[] choiceDialogues)
    {
        foreach (var dialogue in choiceDialogues)
        {
            dialogueText.text = dialogue;
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before showing next part of the dialogue
        }
        EndConversation(); // End conversation after choice path is completed
    }

    void EndConversation()
    {
        isInConversation = false;
        hasMadeChoice = false; // Reset the choice flag
        dialogueBox.SetActive(false); // Hide the dialogue box
        choicesBox.SetActive(false); // Hide the choices box
        npcNameText.text = ""; // Hide NPC name
        dialogueText.text = ""; // Clear the dialogue text
        currentDialogueIndex = 0; // Reset dialogue for the next interaction
        interactionText.SetActive(isPlayerNearby); // Show interaction prompt again if player is nearby
    }
}
