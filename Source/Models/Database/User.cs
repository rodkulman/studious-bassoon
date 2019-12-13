namespace Rodkulman.MilkMafia.Models
{
    public class User
    {
        #region TableMapping
        public int Id { get; set; }
        public byte[] CPF { get; set; }
        public bool ReadAccess { get; set; }
        public bool WriteAccess { get; set; }
        public string Name { get; set; }
        #endregion
    }
}