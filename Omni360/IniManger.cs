using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OmniCheck_360
{
    public class IniManager
    {
        public Dictionary<string, List<string>> SizeFlavourMap = new Dictionary<string, List<string>>();
        public Dictionary<string, string> RecipeTypeMap = new Dictionary<string, string>(); // key: "Size|Flavour", value: "Default"/"Green Bottle"/"Nitrous"
        public List<string> Sizes = new List<string>();

        public void LoadFromFile(string path)
        {
            SizeFlavourMap.Clear();
            RecipeTypeMap.Clear();
            Sizes.Clear();

            // Store Nitrous and Green Bottle special mappings
            var nitrousSet = new HashSet<(string, string)>();
            var greenBottleSet = new HashSet<(string, string)>();
            var specialBottleSet = new HashSet<(string, string)>();

            string currentSection = null;

            // Temporary storage for section contents
            var sectionContents = new Dictionary<string, List<string>>();

            foreach (var lineRaw in File.ReadLines(path))
            {
                var line = lineRaw.Trim();

                if (string.IsNullOrEmpty(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    if (!sectionContents.ContainsKey(currentSection))
                        sectionContents[currentSection] = new List<string>();
                    continue;
                }

                if (currentSection != null)
                    sectionContents[currentSection].Add(line);
            }

            // Parse NitrousRecipes and GreenBottle sections for mappings
            if (sectionContents.ContainsKey("NitrousRecipes"))
            {
                foreach (var entry in sectionContents["NitrousRecipes"])
                {
                    // Example: Name=710ml_Brisk_Tea
                    var match = Regex.Match(entry, @"Name\s*=\s*(.+)");
                    if (match.Success)
                    {
                        var val = match.Groups[1].Value;
                        int underscore = val.IndexOf('_');
                        if (underscore > 0)
                        {
                            var size = val.Substring(0, underscore);
                            var flavour = val.Substring(underscore + 1).Replace('_', ' ');
                            nitrousSet.Add((size, flavour));
                        }
                    }
                }
            }
            if (sectionContents.ContainsKey("GreenBottle"))
            {
                foreach (var entry in sectionContents["GreenBottle"])
                {
                    // Example: Name=710ml_Ginger_Ale
                    var match = Regex.Match(entry, @"Name\s*=\s*(.+)");
                    if (match.Success)
                    {
                        var val = match.Groups[1].Value;
                        int underscore = val.IndexOf('_');
                        if (underscore > 0)
                        {
                            var size = val.Substring(0, underscore);
                            var flavour = val.Substring(underscore + 1).Replace('_', ' ');
                            greenBottleSet.Add((size, flavour));
                        }
                    }
                }
            }
            if (sectionContents.ContainsKey("SpecialBottle"))
            {
                foreach (var entry in sectionContents["SpecialBottle"])
                {
                    var match = Regex.Match(entry, @"Name\s*=\s*(.+)");
                    if (match.Success)
                    {
                        var val = match.Groups[1].Value;
                        int underscore = val.IndexOf('_');
                        if (underscore > 0)
                        {
                            var size = val.Substring(0, underscore);
                            var flavour = val.Substring(underscore + 1).Replace('_', ' ');
                            specialBottleSet.Add((size, flavour));
                        }
                    }
                }
            }
            // Parse flavours in size sections
            foreach (var section in sectionContents.Keys)
            {
                if (Regex.IsMatch(section, @"^\d+(\.\d+)?(L|ml)$", RegexOptions.IgnoreCase))
                {
                    var size = section;
                    Sizes.Add(size);
                    SizeFlavourMap[size] = new List<string>();
                    foreach (var entry in sectionContents[section])
                    {
                        var match = Regex.Match(entry, @"Flavour\d+\s*=\s*(.+)");
                        if (match.Success)
                        {
                            var flavour = match.Groups[1].Value.Trim();
                            SizeFlavourMap[size].Add(flavour);

                            // Determine recipe type
                            string recipeType = "Default";
                            if (nitrousSet.Contains((size, flavour))) recipeType = "Nitrous";
                            else if (greenBottleSet.Contains((size, flavour))) recipeType = "Green Bottle";
                            else if (specialBottleSet.Contains((size, flavour))) recipeType = "Special Bottle";
                            RecipeTypeMap[$"{size}|{flavour}"] = recipeType;
                        }
                    }
                }
            }
        }

        public void SaveToFile(string path)
        {
            // 1. Read all lines of the original file
            var allLines = File.Exists(path) ? File.ReadAllLines(path).ToList() : new List<string>();

            // 2. Find all size sections you will overwrite (e.g., 710ml, 1L, 2L, etc.)
            var sizeSections = new HashSet<string>(Sizes, StringComparer.OrdinalIgnoreCase);

            // 3. We'll build the new file here
            var output = new List<string>();

            int i = 0;
            while (i < allLines.Count)
            {
                string line = allLines[i];
                // Detect section headers
                var match = Regex.Match(line.Trim(), @"^\[(.+?)\]$");
                if (match.Success)
                {
                    string sectionName = match.Groups[1].Value.Trim();
                    if (sizeSections.Contains(sectionName))
                    {
                        // Write the updated section for this size
                        output.Add($"[{sectionName}]");
                        if (SizeFlavourMap.TryGetValue(sectionName, out var flavours))
                        {
                            for (int j = 0; j < flavours.Count; j++)
                            {
                                var flavour = flavours[j];
                                var key = $"{sectionName}|{flavour}";
                                var recipeType = RecipeTypeMap.ContainsKey(key) ? RecipeTypeMap[key] : "Default";
                                output.Add($"Flavour{j + 1} = {flavour}");
                            }
                        }
                        output.Add(""); // Blank line after each section

                        // Skip all lines in the original file that were in this section
                        i++;
                        while (i < allLines.Count && !Regex.IsMatch(allLines[i].Trim(), @"^\[.+\]$"))
                        {
                            i++;
                        }
                        continue; // Don't increment i again, so the new section check hits the next section header
                    }
                }
                // Not a size section, just copy as-is
                output.Add(line);
                i++;
            }

            // 4. Add any new size sections that did not exist in the original file
            foreach (var size in Sizes)
            {
                if (!allLines.Any(l => l.Trim().Equals($"[{size}]", StringComparison.OrdinalIgnoreCase)))
                {
                    output.Add($"[{size}]");
                    if (SizeFlavourMap.TryGetValue(size, out var flavours))
                    {
                        for (int j = 0; j < flavours.Count; j++)
                        {
                            var flavour = flavours[j];
                            var key = $"{size}|{flavour}";
                            var recipeType = RecipeTypeMap.ContainsKey(key) ? RecipeTypeMap[key] : "Default";
                            output.Add($"Flavour{j + 1} = {flavour} ; {recipeType}");
                        }
                    }
                    output.Add("");
                }
            }

            // 5. Write the fully patched INI back to disk
            File.WriteAllLines(path, output);
        }

        /// <summary>
        /// Removes any special recipe sections (GreenBottleRecipe or NitrousRecipe) for the given size and flavour from the INI file at the given path.
        /// Call this after deleting a flavour mapping and before/while saving.
        /// </summary>
        /// <summary>
        /// Removes any special recipe sections (GreenBottleRecipe or NitrousRecipe) for the given size and flavour from the INI file at the given path.
        /// ALSO removes the entry from the [GreenBottle] or [NitrousRecipes] section.
        /// Call this after deleting a flavour mapping and before/while saving.
        /// </summary>
        public void RemoveSpecialRecipeSections(string path, string size, string flavour)
        {
            // Normalize for section format: e.g. [GreenBottleRecipe_710ml_Mountain_Dew]
            string flavourKey = flavour.Replace(" ", "_");
            string[] recipeTypes = { "GreenBottleRecipe", "NitrousRecipe" , "SpecialBottleRecipe" };

            var lines = File.Exists(path) ? File.ReadAllLines(path).ToList() : new List<string>();
            var sectionsToRemove = recipeTypes
                .Select(type => $"[{type}_{size}_{flavourKey}]")
                .ToHashSet();

            // These are the section headers for the mappings
            string[] mappingSections = { "GreenBottle", "NitrousRecipes", "SpecialBottle" };
            // These are the mapping lines to remove (Name=710ml_Mountain_Dew)
            string mappingNameEntry = $"Name={size}_{flavourKey}";

            var output = new List<string>();
            bool skipSection = false;
            string currentSection = null;

            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                // Section header
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentSection = trimmed.Substring(1, trimmed.Length - 2);
                }

                // If starting a section that should be removed, set skip flag
                if (sectionsToRemove.Contains(trimmed))
                {
                    skipSection = true;
                    continue; // Don't add the section header itself
                }

                // If this is a new section and we were skipping, stop skipping
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]") && skipSection)
                {
                    skipSection = false;
                }

                // If we are in [GreenBottle] or [NitrousRecipes], skip line if it matches Name=...
                if (mappingSections.Contains(currentSection) && trimmed.Equals(mappingNameEntry, StringComparison.OrdinalIgnoreCase))
                {
                    continue; // Don't add this mapping line
                }

                // Only add lines not in a skipped section
                if (!skipSection)
                    output.Add(line);
            }

            File.WriteAllLines(path, output);
        }
    }
}