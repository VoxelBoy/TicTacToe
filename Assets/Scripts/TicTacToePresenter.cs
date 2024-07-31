using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToePresenter : MonoBehaviour
{
    [SerializeField] private GameObject playerSelectionUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private Button playerVsPlayerButton;
    [SerializeField] private Button playerVsAIButton;
    [SerializeField] private Text turnIndicatorText;
    [SerializeField] private Text resultText;
    [SerializeField] private Text player1WinsText;
    [SerializeField] private Text player2WinsText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform boardContainer;
    [SerializeField] private Dropdown aiDifficultyDropdown;

    private Button[,] cellButtons;

    public void Initialize(System.Action<int, int> onCellClicked, System.Action onPlayerVsPlayer, 
        System.Action<AIPlayer.Difficulty> onPlayerVsAI, System.Action onPlayAgain, System.Action onBackToMenu)
    {
        playerVsPlayerButton.onClick.AddListener(() => onPlayerVsPlayer());
        playerVsAIButton.onClick.AddListener(() => onPlayerVsAI(GetSelectedAIDifficulty()));
        playAgainButton.onClick.AddListener(() => onPlayAgain());
        backToMenuButton.onClick.AddListener(() => onBackToMenu());

        InitializeBoardButtons(onCellClicked);
        InitializeAIDifficultyDropdown();
    }
    
    public void UpdateView(TicTacToeModel model)
    {
        UpdateBoard(model.GameBoard);
        UpdateTurnIndicator(model.CurrentPlayer);
        SetBoardInteractable(model.CurrentPlayer is not AIPlayer);
    }

    private void InitializeAIDifficultyDropdown()
    {
        aiDifficultyDropdown.ClearOptions();
        aiDifficultyDropdown.AddOptions(new List<string> { "Easy", "Medium", "Hard" });
    }

    private AIPlayer.Difficulty GetSelectedAIDifficulty()
    {
        return (AIPlayer.Difficulty)aiDifficultyDropdown.value;
    }

    private void InitializeBoardButtons(System.Action<int, int> onCellClicked)
    {
        cellButtons = new Button[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject cellObject = Instantiate(cellPrefab, boardContainer);
                cellObject.name = $"Cell_{i}_{j}";
                
                RectTransform rectTransform = cellObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((j-1) * 100, (-i+1) * 100);

                Button cellButton = cellObject.GetComponent<Button>();
                int row = i, col = j;
                cellButton.onClick.AddListener(() => onCellClicked(row, col));
                cellButtons[i, j] = cellButton;
            }
        }
    }

    private void UpdateBoard(Board board)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cellButtons[i, j].GetComponentInChildren<Text>().text = 
                    SymbolToString(board.GetCell(i, j));
            }
        }
    }

    private void UpdateTurnIndicator(Player currentPlayer)
    {
        turnIndicatorText.text = $"Current Turn: {currentPlayer.Name} ({SymbolToString(currentPlayer.GetSymbol())})";
    }

    public void ShowGameResult(Player winner)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = winner != null ? $"Winner: {winner.Name}" : "It's a draw!";
    }
    
    public void HideGameResult()
    {
        resultText.gameObject.SetActive(false);
    }

    public void UpdateWinCounts(Player[] players)
    {
        player1WinsText.text = $"{players[0].Name} Wins: {players[0].GetWins()}";
        player2WinsText.text = $"{players[1].Name} Wins: {players[1].GetWins()}";
    }

    private void SetBoardInteractable(bool interactable)
    {
        foreach (var button in cellButtons)
        {
            button.interactable = interactable;
        }
    }

    public void ShowPlayerSelection()
    {
        playerSelectionUI.SetActive(true);
        gameUI.SetActive(false);
    }

    public void ShowGameUI()
    {
        playerSelectionUI.SetActive(false);
        gameUI.SetActive(true);
    }

    private string SymbolToString(Symbol symbol)
    {
        switch (symbol)
        {
            case Symbol.X: return "X";
            case Symbol.O: return "O";
            default: return "";
        }
    }
}