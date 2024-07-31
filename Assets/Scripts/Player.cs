public class Player
{
    protected Symbol symbol;
    private int wins;
    public string Name { get; private set; }

    public Player(Symbol symbol, string name)
    {
        this.symbol = symbol;
        this.Name = name;
    }

    public Symbol GetSymbol() => symbol;

    public void IncrementWins()
    {
        wins++;
    }

    public int GetWins() => wins;
}