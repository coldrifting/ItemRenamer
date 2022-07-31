using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins.Exceptions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Cache;

namespace ItemSorting
{
    public class Program
    {
        private static readonly Dictionary<FormKey, string> dictionary = new();

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "SortedNames.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (!BuildDatabase(state))
            {
                throw new InvalidDataException("Unable to read the input naming rules file. Please check the formating.");
            }

            // Ammuntion
            foreach (IAmmunitionGetter? item in state.LoadOrder.ListedOrder.Ammunition().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IAmmunition, IAmmunitionGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IAmmunition winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Armor
            foreach (IArmorGetter? item in state.LoadOrder.ListedOrder.Armor().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IArmor, IArmorGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IArmor winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Books
            foreach (IBookGetter? item in state.LoadOrder.ListedOrder.Book().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IBook, IBookGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IBook winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Ingestible
            foreach (IIngestibleGetter? item in state.LoadOrder.ListedOrder.Ingestible().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IIngestible, IIngestibleGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IIngestible winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Ingredients
            foreach (IIngredientGetter? item in state.LoadOrder.ListedOrder.Ingredient().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IIngredient, IIngredientGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IIngredient winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Lights
            foreach (ILightGetter? item in state.LoadOrder.ListedOrder.Light().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, ILight, ILightGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    ILight winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Misc
            foreach (IMiscItemGetter? item in state.LoadOrder.ListedOrder.MiscItem().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IMiscItem, IMiscItemGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IMiscItem winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Scrolls
            foreach (IScrollGetter? item in state.LoadOrder.ListedOrder.Scroll().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IScroll, IScrollGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IScroll winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Soul Gems
            foreach (ISoulGemGetter? item in state.LoadOrder.ListedOrder.SoulGem().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, ISoulGem, ISoulGemGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    ISoulGem winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Weapons
            foreach (IWeaponGetter? item in state.LoadOrder.ListedOrder.Weapon().WinningOverrides())
            {
                if (!item.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, IWeapon, IWeaponGetter>(state.LinkCache, out var winningItemContext)) continue;

                if (dictionary.ContainsKey(item.FormKey))
                {
                    IWeapon winningRecord = winningItemContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(item.FormKey);
                }
            }

            // Spells
            foreach (ISpellGetter? spell in state.LoadOrder.ListedOrder.Spell().WinningOverrides())
            {
                if (!spell.ToLink().TryResolveContext<ISkyrimMod, ISkyrimModGetter, ISpell, ISpellGetter>(state.LinkCache, out var winningSpellContext)) continue;

                if (dictionary.ContainsKey(spell.FormKey))
                {
                    ISpell winningRecord = winningSpellContext.GetOrAddAsOverride(state.PatchMod);
                    winningRecord.Name = dictionary.GetValueOrDefault(spell.FormKey);
                }
            }
        }


        // Build the dictionary to map formIDs to Names
        private static bool BuildDatabase(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            string namingRulesRawText = File.ReadAllText(state.RetrieveConfigFile("NamingRules.json"));
            var mydictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(namingRulesRawText);
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
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
