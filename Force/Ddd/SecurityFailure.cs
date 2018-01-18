namespace Force.Ddd
{
    public class SecurityFailure: Failure
    {
        public SecurityFailure(string userName) : base($"Operation is forbidden for user \"{userName}\"")
        {
        }
    }
}