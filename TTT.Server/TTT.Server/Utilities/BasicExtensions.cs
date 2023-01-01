namespace TTT.Server.Utilities
{
    public static class BasicExtensions
    {
        public static (byte, byte) GetRowCol(byte index)
        {
            var row = (byte)(index / 3);
            var col = (byte)(index % 3);

            return (row, col);
        }
    }
}