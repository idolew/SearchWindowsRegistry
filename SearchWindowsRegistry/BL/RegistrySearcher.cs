using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchWindowsRegistry.BL
{
    public class RegistrySearcher
    {
        public event EventHandler Finished;

        public event EventHandler<FoundRegistryItem> FoundRegistry;

        public bool IsSearching { get; set; }
        public bool IsCanceling { get; set; }

        private CancellationTokenSource cancelationTokenSource;

        public RegistrySearcher()
        {
        }

        public async Task StartSearch(string search, bool byKey, bool byValue)
        {
            cancelationTokenSource = new CancellationTokenSource();
            CancellationToken currentCancelationToken = cancelationTokenSource.Token;
            
            await Task.Factory.StartNew(() =>
            {
                IsSearching = true;

                try
                {

                    foreach (RegistryKey key in RegistryHelper.FlatAllRegistryChildren())
                    {
                        if (currentCancelationToken.IsCancellationRequested) return;

                        if (byKey)
                        {
                            foreach (var subkey in GetSubRegistryKeys(key))
                            {
                                if (IsSearchInKey(search, subkey))
                                {
                                    //return a registry item with the value if any
                                    FoundRegistry?.Invoke(this, subkey);
                                    continue;
                                }
                            }

                        }

                        if (byValue)
                        {

                            foreach (var subkey in GetSubRegistryKeys(key))
                            {
                                if (IsSearchInValue(search, subkey))
                                {
                                    //create a new registry item with the found value
                                    FoundRegistry?.Invoke(this, subkey);
                                    break;
                                }
                            }

                        }
                    }
                    
                }
                finally
                {
                    IsSearching = false;
                    Finished?.Invoke(this, EventArgs.Empty);
                }

            }
            , currentCancelationToken);
        }

        public static bool IsSearchInValue(string search, FoundRegistryItem subkey)
        {
            return subkey.Value?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static bool IsSearchInKey(string search, FoundRegistryItem subkey)
        {
            return subkey.Name.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private IEnumerable<FoundRegistryItem> GetSubRegistryKeys(RegistryKey key)
        {
            string[] valueNames = key.GetValueNames();

            foreach (string valueName in valueNames)
            {
                yield return new FoundRegistryItem(key.Name, valueName, key.GetValue(valueName)?.ToString());
            }
        }

        public void Cancel()
        {
            cancelationTokenSource.Cancel();
        }
    }
}
