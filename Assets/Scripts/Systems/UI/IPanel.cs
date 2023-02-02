/// <summary>
/// Generic panel interface
/// </summary>
public interface IPanel
{
    public void SetupPanel(LevelData actualLevelData);

    public void OpenPanel(object aditionalOpenData = null);

    public void ClosePanel();
}
