using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCInteractionWithChoices : MonoBehaviour
{
    public GameObject interactionText;
    public GameObject dialogueBox;
    public GameObject choicesBox;
    public Text dialogueText;
    public Text npcNameText;
    public Button choice1Button;
    public Button choice2Button;
    public Text choice1Text;
    public Text choice2Text;

    [TextArea(3, 5)]
    public string[] dialogues;
    public string choicePrompt;
    public string[] choice1Dialogues;
    public string[] choice2Dialogues;
    public string npcName;
    public string choice1Label;
    public string choice2Label;

    private int currentDialogueIndex = 0;
    private bool isPlayerNearby = false;
    private bool isInConversation = false;
    private bool hasMadeChoice = false;

    void Start()
    {
        interactionText.SetActive(false);
        dialogueBox.SetActive(false);
        choicesBox.SetActive(false);
        dialogueText.text = "";
        npcNameText.text = ""; // Ensure NPC name is hidden initially

        choice1Button.onClick.AddListener(() => MakeChoice(1));
        choice2Button.onClick.AddListener(() => MakeChoice(2));

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
            interactionText.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isInConversation)
        {
            StartConversation();
        }

        if (isInConversation && Input.GetMouseButtonDown(0) && !hasMadeChoice)
        {
            ShowNextDialogue();
        }
    }

    void StartConversation()
    {
        isInConversation = true;
        interactionText.SetActive(false);
        dialogueBox.SetActive(true);
        npcNameText.text = npcName; // Show NPC's name only during the conversation
        npcNameText.gameObject.SetActive(true); // Enable the NPC name text
        ShowNextDialogue();
    }

    void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];

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
        dialogueBox.SetActive(false);
        choicesBox.SetActive(true);
    }

    void MakeChoice(int choice)
    {
        hasMadeChoice = true;
        choicesBox.SetActive(false);
        dialogueBox.SetActive(true);

        if (choice == 1)
        {
            StartCoroutine(ShowChoiceDialogues(choice1Dialogues));
        }
        else if (choice == 2)
        {
            StartCoroutine(ShowChoiceDialogues(choice2Dialogues));
        }
    }

    IEnumerator ShowChoiceDialogues(string[] choiceDialogues)
    {
        foreach (var dialogue in choiceDialogues)
        {
            dialogueText.text = dialogue;
            yield return new WaitForSeconds(2f);
        }
        EndConversation();
    }

    void EndConversation()
    {
        isInConversation = false;
        hasMadeChoice = false;
        dialogueBox.SetActive(false);
        choicesBox.SetActive(false);
        npcNameText.gameObject.SetActive(false); // Hide NPC name when conversation ends
        dialogueText.text = "";
        currentDialogueIndex = 0;

        if (isPlayerNearby)
        {
            interactionText.SetActive(true);
        }
    }
}
