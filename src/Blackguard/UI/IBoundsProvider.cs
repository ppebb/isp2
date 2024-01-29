namespace Blackguard.UI;

public interface IBoundsProvider {
    public (int X, int Y) GetBounds();

    public (int X, int Y) GetOffset();
}
