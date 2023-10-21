using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    [Header("Dialogue Manager")]
    [SerializeField] private DialogueManager dialogueManager; // Associe o DialogueManager específico no Unity Inspector

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !dialogueManager.dialogueIsPlaying)
        {
            if (playerInRange)
            {
                Time.timeScale = 0;
                dialogueManager.EnterDialogueMode(inkJSON, this.gameObject);
            }
        }

        if (playerInRange)
        {
            visualCue.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}