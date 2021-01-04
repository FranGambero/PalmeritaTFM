public interface ITurn
{
    int turnIndex { get; set; }
    bool turneable { get; set; }

    void onTurnStart(int currentIndex);
    void onTurnFinished();
}
