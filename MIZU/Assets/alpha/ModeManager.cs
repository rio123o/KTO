using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public GameObject player1Model;   // player1‚Ìƒ‚ƒfƒ‹
    public GameObject player2Model;   // player2‚Ìƒ‚ƒfƒ‹

    [HideInInspector] public KK_PlayerModelSwitcher player1Mode;
    [HideInInspector] public KK_PlayerModelSwitcher player2Mode;

    [HideInInspector] public string player1ModelTag;
    [HideInInspector] public string player2ModelTag;

    [HideInInspector] public GameObject Player1 => player1Model;
    [HideInInspector] public GameObject Player2 => player2Model;

    void Start()
    {
        player1Mode = player1Model.GetComponent<KK_PlayerModelSwitcher>();
        player2Mode = player2Model.GetComponent<KK_PlayerModelSwitcher>();
    }

    void Update()
    {
        player1ModelTag = player1Mode.currentModel.tag;
        player2ModelTag = player2Mode.currentModel.tag;

        Debug.Log("Player1 model tag: " + player1ModelTag);
        Debug.Log("Player2 model tag: " + player2ModelTag);
    }
}
