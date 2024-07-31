public class PlayingState : IGameState
{
    private TicTacToeManager manager;

    public PlayingState(TicTacToeManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        manager.GetComponent<TicTacToePresenter>().ShowGameUI();
    }
    
    public void Exit() { }
}