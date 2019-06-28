namespace GenesisChallenge.Abstractions.Repositories
{
    /// <summary>
    /// Wraps data access functionality for entities
    /// </summary>
    /// <remarks>
    /// Expose data access operation for each entity and a common Save action to persist all the changes
    /// </remarks>
    public interface IRepositoryWrapper
    {
        /// <value>Gets the data access operations for a User</value>
        IUserRepository User { get; }

        /// <value>Calls the SaveChanges method to persist all the changes in the context</value>
        void Save();
    }
}
