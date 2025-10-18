using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissorManager : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] Button RockButton;
    [SerializeField] Button PaperButton;
    [SerializeField] Button ScissorstButton;

    int selectedIndex;
    ChoiceSelected currentSelection;

    public void Start()
    {
        RockButton.onClick.AddListener(() => HandleChoiceClicked(0));
        PaperButton.onClick.AddListener(() => HandleChoiceClicked(1));
        ScissorstButton.onClick.AddListener(() => HandleChoiceClicked(2));
    }

    void HandleChoiceClicked(int index)
    {
        selectedIndex = index;
        switch (selectedIndex)
        {
            case 0: currentSelection = ChoiceSelected.Rock; break;
            case 1: currentSelection = ChoiceSelected.Paper; break;
            case 2: currentSelection = ChoiceSelected.Scissors; break;
        }
        Debug.Log($"Player selected: {currentSelection}");
        RockPaperScissorView.Instance.SubmitSelection(currentSelection);
        parent.SetActive(false);
    }
}
