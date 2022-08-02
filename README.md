# ItemRenamer
A Synthesis Patcher for Skyrim that adjusts item and spell names to match those specified in one or more user configurable files, usually to facilitate better inventory sorting.

A default config file is provided with support for the vanilla game and DLC. I also include definitions for a few mods that I personally use.

# Current List of Supported Mods
- Beyond Skyrim - Bruma
- Mysticism

# Configuration
To add suport for additional mods, or to change a name definition, simply create one or more json files in the patcher data directory, and they will be read in alphabetical order by file name.
For instance, you could create a file called CustomRules.json in the ```C:/../Synthesis/Data/Skyrim Special Edition/ItemRenamer``` directory.

If, for instance, you wanted to change the name of the gold item from Gold to Septim, you could put:

```"00000F:Skyrim.esm": "Septim",```

on a line in that file, along with the opening and closing JSON braces. In this case, 00000F is the FormID of the Gold item as seen in xEdit, without the load order index, and Skyrim.esm is the plugin or master file where the item was originally defined. Finally the last part of the line is what you would like to rename the item or spell to. Finally, don't forget to add commas after all lines except the last, as well as opening and closing curly braces to each file.

You can browse the rules included in Default folder in the patcher data folder for more examples.
