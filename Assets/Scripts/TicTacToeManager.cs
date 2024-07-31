using System.Collections;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    [SerializeField] private TicTacToePresenter presenter;
    
    private TicTacToeModel model;
    private IGameState currentState;

    private IGameState playerSelectionState;
    private IGameState playingState;
    private IGameState gameOverState;

    private void Start()
    {
        model = new TicTacToeModel(3);
        presenter.Initialize(OnCellClicked, OnPlayerVsPlayerSelected, OnPlayerVsAISelected, PlayAgain, BackToMenu);

        playerSelectionState = new PlayerSelectionState(this);
        playingState = new PlayingState(this);
        gameOverState = new GameOverState();
        
        SetState(playerSelectionState);
    }

    private void StartGame(bool isAIGame, AIPlayer.Difficulty aiDifficulty = AIPlayer.Difficulty.Medium)
    {
        model.InitializePlayers(isAIGame, aiDifficulty);
        presenter.UpdateView(model);
        presenter.UpdateWinCounts(model.Players);
        SetState(playingState);
    }

    private void OnPlayerVsPlayerSelected() => StartGame(false);
    private void OnPlayerVsAISelected(AIPlayer.Difficulty difficulty) => StartGame(true, difficulty);

    private void SetState(IGameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void OnCellClicked(int x, int y)
    {
        if (currentState is PlayingState && !(model.CurrentPlayer is AIPlayer))
        {
            if (model.MakeMove(x, y))
            {
                presenter.UpdateView(model);
                CheckGameState();
            }
        }
    }

    private void CheckGameState()
    {
        if (model.CheckWin())
        {
            EndGame(model.CurrentPlayer);
        }
        else if (model.IsBoardFull())
        {
            EndGame(null);
        }
        else
        {
            model.SwitchTurn();
            presenter.UpdateView(model);
            if (model.CurrentPlayer is AIPlayer aiPlayer)
            {
                StartCoroutine(MakeAIMoveCoroutine(aiPlayer));
            }
        }
    }

    private IEnumerator MakeAIMoveCoroutine(AIPlayer aiPlayer)
    {
        yield return new WaitForSeconds(0.5f);
        (int x, int y) = aiPlayer.MakeMove(model.GameBoard);
        model.MakeMove(x, y);
        presenter.UpdateView(model);
        CheckGameState();
    }

    private void EndGame(Player winner)
    {
        winner?.IncrementWins();
        presenter.ShowGameResult(winner);
        presenter.UpdateWinCounts(model.Players);
        SetState(gameOverState);
    }

    private void PlayAgain()
    {
        model.ResetBoard();
        model.ResetCurrentPlayerToFirstPlayer();
        SetState(playingState);
        presenter.UpdateView(model);
        presenter.HideGameResult();
    }

    private void BackToMenu()
    {
        model.ResetBoard();
        model.ResetCurrentPlayerToFirstPlayer();
        SetState(playerSelectionState);
        presenter.HideGameResult();
    }
}