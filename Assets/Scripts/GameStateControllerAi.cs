using UnityEngine;
using UnityEngine.UI;

public class GameStateControllerAi : MonoBehaviour
{
    [Header("AI Settings")]
    public bool enableAutoPlay = false;

    [Header("TitleBar References")]
    public Image playerXIcon;                                        // Reference to the playerX icon
    public Image playerOIcon;                                        // Reference to the playerO icon
    public InputField player1InputField;                             // Reference to P1 input field
    public InputField player2InputField;                             // Refernece to P2 input field
    /*    public Text winnerText;*/                                          // Displays the winners name

    [Header("Misc References")]
    public GameObject endGameState;                                  // Game footer container + winner text

    [Header("Asset References")]
    public Sprite tilePlayerO;                                       // Sprite reference to O tile
    public Sprite tilePlayerX;                                       // Sprite reference to X tile
    public Sprite tileEmpty;                                         // Sprite reference to empty tile
    public Text[] tileList;                                          // Gets a list of all the tiles in the scene

    [Header("GameState Settings")]
    public Color inactivePlayerColor;                                // Color to display for the inactive player icon
    public Color activePlayerColor;                                  // Color to display for the active player icon
    public string whoPlaysFirst;                                     // Who plays first (X : 0) {NOTE! no checks are made to ensure this is either X or O}

    [Header("Private Variables")]
    private string playerTurn;                                       // Internal tracking whos turn is it
    private string player1Name;                                      // Player1 display name
    private string player2Name;                                      // Player2 display name
    private int moveCount;                                           // Internal move counter


    public GameObject Win;
    public GameObject Lose;
    public GameObject Draw;



    public AudioSource XSound;
    public AudioSource OSound;


    private bool isPlayerInputEnabled = true;
    /// <summary>
    /// Start is called on the first active frame
    /// </summary>
    private void Start()
    {
        Win.SetActive(false);
        Lose.SetActive(false);
        Draw.SetActive(false);
        // Set the internal tracker of whos turn is first and setup UI icon feedback for whos turn it is
        playerTurn = whoPlaysFirst;
        if (playerTurn == "X") playerOIcon.color = inactivePlayerColor;
        else playerXIcon.color = inactivePlayerColor;

        //Adds a listener to the name input fields and invokes a method when the value changes. This is a callback.
        player1InputField.onValueChanged.AddListener(delegate { OnPlayer1NameChanged(); });
        player2InputField.onValueChanged.AddListener(delegate { OnPlayer2NameChanged(); });

        // Set the default values to what tthe inputField text is
        player1Name = player1InputField.text;
        player2Name = player2InputField.text;
    }


    /// <summary>
    /// Called at the end of every turn to check for win conditions
    /// Hardcoded all possible win conditions (8)
    /// We just take position of tiles and check the neighbours (within a row)
    /// 
    /// Tiles are numbered 0..8 from left to right, row by row, example:
    /// [0][1][2]
    /// [3][4][5]
    /// [6][7][8]
    /// </summary>
    public void EndTurn()
    {

        moveCount++;


        if (!endGameState.activeSelf && moveCount < 9)
        {
            if (playerTurn == "X")
            {
                XSound.Play();
            }
            else if (playerTurn == "O")
            {
                OSound.Play();
            }
            // Introduce a delay before AI's move
            Invoke("AutoPlay", 0.1f); // 1 second delay before AutoPlay method is called

        }

        if (!enableAutoPlay && playerTurn == "O")
        {
            return;
        }
        if (tileList[0].text == playerTurn && tileList[1].text == playerTurn && tileList[2].text == playerTurn) GameOver(playerTurn);
        else if (tileList[3].text == playerTurn && tileList[4].text == playerTurn && tileList[5].text == playerTurn) GameOver(playerTurn);
        else if (tileList[6].text == playerTurn && tileList[7].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn);
        else if (tileList[0].text == playerTurn && tileList[3].text == playerTurn && tileList[6].text == playerTurn) GameOver(playerTurn);
        else if (tileList[1].text == playerTurn && tileList[4].text == playerTurn && tileList[7].text == playerTurn) GameOver(playerTurn);
        else if (tileList[2].text == playerTurn && tileList[5].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn);
        else if (tileList[0].text == playerTurn && tileList[4].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn);
        else if (tileList[2].text == playerTurn && tileList[4].text == playerTurn && tileList[6].text == playerTurn) GameOver(playerTurn);
        else if (moveCount >= 9) GameOver("D");
        else
            ChangeTurn();
    }
    private void AutoPlay()
    {
        // Check if the game is still ongoing and it's the AI player's turn
        if (!endGameState.activeSelf && playerTurn == "O")
        {
            // Try to find a winning move
            int winningMove = FindWinningMove();
            if (winningMove != -1)
            {
                tileList[winningMove].GetComponentInParent<Button>().onClick.Invoke();
                return;
            }

            // If no winning move is available, try to block the opponent's winning move
            int blockingMove = FindBlockingMove();
            if (blockingMove != -1)
            {
                tileList[blockingMove].GetComponentInParent<Button>().onClick.Invoke();
                return;
            }

            // If neither winning nor blocking move is available, make a random move
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, tileList.Length);
            } while (tileList[randomIndex].text != "");

            tileList[randomIndex].GetComponentInParent<Button>().onClick.Invoke();
        }

    }

    // Find a winning move for the AI
    private int FindWinningMove()
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileList[i].text == "")
            {
                // Simulate placing "O" on the empty tile and check for a win
                tileList[i].text = "O";
                if (CheckWinCondition("O"))
                {
                    tileList[i].text = ""; // Reset the tile
                    return i;
                }
                tileList[i].text = ""; // Reset the tile
            }
        }
        return -1; // No winning move found
    }

    // Find a move to block the opponent's winning move
    private int FindBlockingMove()
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileList[i].text == "")
            {
                // Simulate placing "X" (opponent's move) on the empty tile and check if it prevents a win
                tileList[i].text = "X";
                if (CheckWinCondition("X"))
                {
                    tileList[i].text = ""; // Reset the tile
                    return i;
                }
                tileList[i].text = ""; // Reset the tile
            }
        }
        return -1; // No blocking move found
    }
    // Check if a player has won the game
    private bool CheckWinCondition(string player)
    {
        // Check rows, columns, and diagonals for a win
        for (int i = 0; i < 3; i++)
        {
            // Check rows
            if (tileList[i * 3].text == player && tileList[i * 3 + 1].text == player && tileList[i * 3 + 2].text == player)
                return true;

            // Check columns
            if (tileList[i].text == player && tileList[i + 3].text == player && tileList[i + 6].text == player)
                return true;
        }

        // Check diagonals
        if (tileList[0].text == player && tileList[4].text == player && tileList[8].text == player)
            return true;
        if (tileList[2].text == player && tileList[4].text == player && tileList[6].text == player)
            return true;

        return false; // No win condition found
    }
    /// <summary>
    /// Changes the internal tracker for whos turn it is
    /// </summary>
    public void ChangeTurn()
    {
        // This is called a Ternary operator which evaluates "X" and results in "O" or "X" based on truths
        // We then just change some ui feedback like colors.
        playerTurn = (playerTurn == "X") ? "O" : "X";
        if (playerTurn == "X")
        {

            playerXIcon.color = activePlayerColor;
            playerOIcon.color = inactivePlayerColor;
        }
        else
        {
            playerXIcon.color = inactivePlayerColor;
            playerOIcon.color = activePlayerColor;
        }
    }

    /// <summary>
    /// Called when the game has found a win condition or draw
    /// </summary>
    /// <param name="winningPlayer">X O D</param>
    private void GameOver(string winningPlayer)
    {
        switch (winningPlayer)
        {
            case "D":

                Draw.SetActive(true);
                Win.SetActive(false);
                Lose.SetActive(false);
                /*  winnerText.text = "DRAW";*/
                break;
            case "X":
                Win.SetActive(true);
                Draw.SetActive(false);
                Lose.SetActive(false);
                /* winnerText.text = player1Name + " Win";*/
                break;
            case "O":
                Lose.SetActive(true);
                Win.SetActive(false);
                Draw.SetActive(false);
                /*winnerText.text = player2Name + " Win";*/
                break;
        }
        endGameState.SetActive(true);
        ToggleButtonState(false);
    }

    /// <summary>
    /// Restarts the game state
    /// </summary>
    public void RestartGame()
    {
        // Reset some gamestate properties
        moveCount = 0;
        playerTurn = whoPlaysFirst;
        ToggleButtonState(true);
        endGameState.SetActive(false);

        // Loop though all tiles and reset them
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<TileControllerAi>().ResetTile();
        }
    }

    /// <summary>
    /// Enables or disables all the buttons
    /// </summary>
    private void ToggleButtonState(bool state)
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<Button>().interactable = state;
        }
    }


    /// <summary>
    /// Returns the current players turn (X / O)
    /// </summary>
    public string GetPlayersTurn()
    {
        return playerTurn;
    }

    /// <summary>
    /// Retruns the display sprite (X / 0)
    /// </summary>
    public Sprite GetPlayerSprite()
    {
        if (playerTurn == "X") return tilePlayerX;
        else return tilePlayerO;
    }

    /// <summary>
    /// Callback for when the P1_textfield is updated. We just update the string for Player1
    /// </summary>
    public void OnPlayer1NameChanged()
    {
        player1Name = player1InputField.text;
    }

    /// <summary>
    /// Callback for when the P2_textfield is updated. We just update the string for Player2
    /// </summary>
    public void OnPlayer2NameChanged()
    {
        player2Name = player2InputField.text;
    }
}