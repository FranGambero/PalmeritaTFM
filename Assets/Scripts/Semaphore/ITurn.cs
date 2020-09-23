public interface ITurn
{
    int turnIndex { get; set; }

    void onTurnStart(int currentIndex);
    void onTurnFinished();
}
