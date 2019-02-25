using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SearchWindowsRegistry.BL
{
    public static class RegistryHelper
    {
        private static readonly RegistryKey REGISTRY_ROOT = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

        public static IEnumerable<RegistryKey> FlatAllRegistryChildren()
        {
            return FlatAllRegistryChildren(REGISTRY_ROOT);
        }

        public static IEnumerable<RegistryKey> FlatAllRegistryChildren(RegistryKey keyParent)
        {
            foreach (string subkeyName in keyParent.GetSubKeyNames())
            {
                RegistryKey subkey;

                try
                {
                    subkey = keyParent.OpenSubKey(subkeyName, RegistryRights.ReadKey);

                }
                catch
                {
                    Console.WriteLine($"Couldn't open {subkeyName}");
                    continue;
                }

                //return this subkey as a result
                yield return subkey;
                
                //recursively search the subkey for more keys
                foreach (RegistryKey registryKey in FlatAllRegistryChildren(subkey))
                {
                    yield return registryKey;
                }
            }
        }
    }
}
