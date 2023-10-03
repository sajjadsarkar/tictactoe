using UnityEngine;
using UnityEngine.UI;

public class TileControllerAi : MonoBehaviour
{
    [Header("Component References")]
    public GameStateControllerAi gameControllerai;                       // Reference to the gamecontroller
    public Button interactiveButton;                                 // Reference to this button
    public Text internalText;                                        // Reference to this Text



    /// <summary>
    /// Called everytime we press the button, we update the state of this tile.
    /// The internal tracking for whos position (the text component) and disable the button
    /// </summary>
    public void UpdateTile()
    {
        internalText.text = gameControllerai.GetPlayersTurn();
        interactiveButton.image.sprite = gameControllerai.GetPlayerSprite();
        interactiveButton.interactable = false;
        gameControllerai.EndTurn();
    }

    /// <summary>
    /// Resets the tile properties
    /// - text component
    /// - buttton image
    /// </summary>
    public void ResetTile()
    {
        internalText.text = "";
        interactiveButton.image.sprite = gameControllerai.tileEmpty;
    }
}