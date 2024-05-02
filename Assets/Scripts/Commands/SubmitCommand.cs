using Holder;
using Tiles;

namespace Commands
{
    public class SubmitCommand
    {
        public void Execute(TileManager tileManager, HolderManager holderManager,UIManager uiManager)
        {
            holderManager.Submit(out var word,out var score);
            uiManager.gameScreen.AddWord(word);
            uiManager.gameScreen.AddScore(score);
        }
    }
}