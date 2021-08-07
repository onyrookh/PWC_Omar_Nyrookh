namespace PWC.Infrastructure
{
  /// <summary>
  /// Creates new instances of a unit of Work.
  /// </summary>
  public interface IUnitOfWorkFactory
  {
    /// <summary>
    /// Creates a new instance of a unit of work
    /// </summary>
    IUnitOfWork Create();
  }
}
