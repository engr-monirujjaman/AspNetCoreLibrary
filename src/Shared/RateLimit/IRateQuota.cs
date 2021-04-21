
// Single resource

namespace Microsoft.AspNetCore.Internal
{
    public interface IRateQuota<TKey>
    {
        // Read the resource count for a given resourceId
        long GetTally(TKey resourceId);

        // Update the resource count for a given resourceId
        long IncrementTally(TKey resourceId, long amount);
    }
}
