namespace Utility
{
    public sealed class UserID
    {
        public UserID()
        {
            Value = _availableID.Min;
            _availableID.Remove(Value);
        }
        static UserID()
        {
            None = -1;
            MaxPlayers = 128;
            _availableID = new(Enumerable.Range(0, MaxPlayers));
        }
        public void Free()
        {
            _availableID.Add(Value);
            Value = None;
        }
        public static readonly int None;
        public static readonly int MaxPlayers;
        private static readonly SortedSet<int> _availableID;
        public int Value { get; private set; }
        public static implicit operator int(UserID u)
        {
            return u.Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
