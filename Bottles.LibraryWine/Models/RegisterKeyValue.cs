using System;

namespace Bottles.LibraryWine.Models
{
    public class RegisterKeyValue
    {
        public string Name { get; set; }
        public WineRegister.KTypes Type { get; set; }
        public string Data { get; set; }
    }
}