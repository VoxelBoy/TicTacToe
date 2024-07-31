public class PlayerSelectionState : IGameState
{
    private TicTacToeManager manager;

    public PlayerSelectionState(TicTacToeManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        manager.GetComponent<TicTacToePresenter>().ShowPlayerSelection();
    }

    public void Exit() { }
}