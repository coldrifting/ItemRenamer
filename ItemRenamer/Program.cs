using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Aspects;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ItemRenamer
{
    public class Program
    {
        private static readonly Dictionary<FormKey, string> dictionary = new();

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "ItemRenamer.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            BuildDatabase(state);

            // Run the patch for each record type
            ProcessNames<IAmmunitionGetter>(state);
            ProcessNames<IArmorGetter>(state);
            ProcessNames<IBookGetter>(state);
            ProcessNames<IIngestibleGetter>(state);
            ProcessNames<IIngredientGetter>(state);
            ProcessNames<ILightGetter>(state);
            ProcessNames<IMiscItemGetter>(state);
            ProcessNames<IScrollGetter>(state);
            ProcessNames<ISoulGemGetter>(state);
            ProcessNames<ISpellGetter>(state);
            ProcessNames<IWeaponGetter>(state);
        }

        // Generic method to patch a specific record that is part of a specified getter interface
        public static void ProcessNames<TMajor>(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
            where TMajor : IMajorRecordGetter, INamedGetter
        {
            foreach (var item in state.LoadOrder.PriorityOrder.WinningOverrideContexts<ISkyrimMod, ISkyrimModGetter>(state.LinkCache, typeof(TMajor)))
            {
                if (dictionary.TryGetValue(item.Record.FormKey, out var replacementName))
                {
                    var winningRecord = (INamed)item.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = replacementName;
                }
            }
        }


        // Build the dictionary to map formIDs to Names
        private static void BuildDatabase(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            // First merge default configs
            JObject? finalJson = new();
            if (Directory.Exists(state.ExtraSettingsDataPath + "\\Default"))
            {
                string[] defaultConfigFiles = Directory.GetFiles(state.ExtraSettingsDataPath + "\\Default");
                Array.Sort(defaultConfigFiles);

                if (defaultConfigFiles.Length == 0)
                {
                    Console.WriteLine("Warning: Unable to find default naming rules");
                }
                else
                {
                    Console.WriteLine("Adding default naming rules");
                }

                foreach (string path in defaultConfigFiles)
                {
                    if (File.Exists(path))
                        finalJson.Merge(JObject.Parse(File.ReadAllText(path)));
                }
            }

            // Then merge custom user settings
            if (Directory.Exists(state.ExtraSettingsDataPath))
            {
                string[] userConfigFiles = Directory.GetFiles(state.ExtraSettingsDataPath);
                Array.Sort(userConfigFiles);

                foreach (string path in userConfigFiles)
                {
                    Console.WriteLine($"Adding user defined naming rule - {Path.GetFileName(path)}");
                    if (File.Exists(path))
                        finalJson.Merge(JObject.Parse(File.ReadAllText(path)));
                }
            }

            // Store the settings in a dictionary for fast access later on
            Dictionary<string, string>? mydictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(finalJson.ToString());
            if (mydictionary != null)
            {
                foreach (KeyValuePair<string, string> kv in mydictionary)
                {
                    FormKey formKey = FormKey.Factory(kv.Key);
                    if (formKey != null)
                    {
                        dictionary.Add(formKey, kv.Value);
                    }
                }
            }
        }
    }
}
