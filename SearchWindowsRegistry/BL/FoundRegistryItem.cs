using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchWindowsRegistry.BL
{
    public class FoundRegistryItem
    {
        public FoundRegistryItem(string path, string name, string value)
        {
            Name = name;
            Value = value;
            Path = path;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public override string ToString()
        {
            return Path + "\\" + this.Name + ":" + this.Value;
        }
    }
}
