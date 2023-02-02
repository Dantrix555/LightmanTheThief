/// <summary>
/// Generic observer interface
/// </summary>
public interface IObserver
{
    /// <summary>
    /// Update about observed subject status
    /// </summary>
    /// <param name="subject">Subject observed</param>
    public void UpdateSubjectState(ISubject subject);
}
