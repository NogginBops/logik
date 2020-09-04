namespace LogikCore
{
    public enum ValueState : byte
    {
        // The bit patterns here are important for the logic that
        // resolves two values states. It allows for resolving to be
        // a single bitwise-or operation.
        Floating = 0b00,
        Zero = 0b01,
        One = 0b10, 
        Error = 0b11,
    }
}
