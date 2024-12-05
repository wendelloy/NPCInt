using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactionText; // Prompt text (e.g., "Press E to Talk")
    public GameObject dialogueBox; // Dialogue box to display text
    public Text dialogueText; // Text element for dialogue content
    public Text npcNameText; // Text element to show NPC's name

    [TextArea(3, 5)]
    public string[] dialogues; // Array of dialogues
    public string npcName; // NPC name

    private int currentDialogueIndex = 0;
    private bool isPlayerNearby = false;
    private bool isInConversation = false;

    void Start()
    {
        interactionText.SetActive(false);
        dialogueBox.SetActive(false);
        dialogueText.text = ""; // Ensure dialogue text starts empty
        npcNameText.text = ""; // Ensure NPC name starts empty
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

        if (isInConversation && Input.GetMouseButtonDown(0)) // Left mouse button
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
            currentDialogueIndex++;
        }
        else
        {
            EndConversation();
        }
    }

    void EndConversation()
    {
        isInConversation = false;
        dialogueBox.SetActive(false); // Hide the dialogue box
        npcNameText.text = ""; // Hide NPC name
        dialogueText.text = ""; // Clear the dialogue text
        currentDialogueIndex = 0; // Reset dialogue for the next interaction
        interactionText.SetActive(isPlayerNearby); // Show interaction prompt again if player is nearby
    }
}
