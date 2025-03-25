namespace FinTrack.Services.Context.Contracts
{
    public interface IContext
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Role { get; set; }
    }
}
