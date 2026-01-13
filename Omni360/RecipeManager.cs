using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.InputsPackage;

namespace OmniCheck_360
{
    public static class RecipeManager
    {
        // Mapping for C1
        public static readonly Dictionary<string, string> C1Mapping = new Dictionary<string, string>
        {
            {"C1ExposureTxt", "Exposure"},
            {"C1FillLevelIntensityEnable", "FillLevelIntensityEnable"},
            {"C1FillLevelCenterXTextBoxValue", "FillLevelIntensityX"},
            {"C1FillLevelCenterYTextBoxValue", "FillLevelIntensityY"},
            {"C1FillLevelCenterWidthTextBoxValue", "FillLevelIntensityWidth"},
            {"C1FillLevelCenterHeightTextBoxValue", "FillLevelIntensityHeight"},
            {"C1FillLevelCenterAngleTextBoxValue", "FillLevelIntensityAngle"},
            {"C1FillLevelIntensityRejectionStatus", "FillLevelIntensityRejectionStatus"},
            {"C1FillLevelIntensityGreaterThanLessThan","FillLevelGreaterThanLessThan"},
            {"C1FillLevelValue", "FillLevelIntensityThreshold"},
            {"C1FillLevelEdgeEnable", "FillLevelEdgeEnable"},
            {"C1FillLevelEdgeX", "FillLevelEdgeX"},
            {"C1FillLevelEdgeY", "FillLevelEdgeY"},
            {"C1FillLevelEdgeWidth", "FillLevelEdgeWidth"},
            {"C1FillLevelEdgeHeight", "FillLevelEdgeHeight"},
            {"C1FillLevelEdgeAngle", "FillLevelEdgeAngle"},
            {"C1FillLevelEdgeRejectionStatus", "FillLevelEdgeRejectionStatus"},
            {"C1FillLevelEdgeGreaterThanLessThan","FillLevelEdgeGreaterThanLessThan" },
            {"C1FillLevelEdgeThreshold", "FillLevelEdgeThreshold"},
            {"C1FillLevelEdgeStrength", "FillLevelEdgeStrength"},
            {"C1FillLevelIntensityFilterDropDownBox","FillLevelIntensityFilterDropDownBox" },
            {"C1FillLevelIntensityIncludeConditionDropDownBox","FillLevelIntensityIncludeConditionDropDownBox" },
            {"C1FillLevelIntensityConditionLowValue","FillLevelIntensityConditionLowValue" },
            {"C1FillLevelIntensityConditionHighValue","FillLevelIntensityConditionHighValue" },
            {"C1FillLevelEdgeAdvancedNumber","FillLevelEdgeAdvancedNumber" },
            {"C1FillLevelEdgeAdvancedFilterTypeDropDown","FillLevelEdgeAdvancedFilterTypeDropDown" },
            {"C1FillLevelEdgeAdvancedPolarityDropDown","FillLevelEdgeAdvancedPolarityDropDown" },
            {"C1FillLevelEdgeAdvancedMinimum","FillLevelEdgeAdvancedMinimum" },
            {"C1FillLevelEdgeAdvancedSubregion","FillLevelEdgeAdvancedSubregion" },
            {"C1FillLevelEdgeAdvancedSmoothness","FillLevelEdgeAdvancedSmoothness" },
            {"C1FillLevelEdgeAdvancedEdgeThreshold","FillLevelEdgeAdvancedEdgeThreshold" },
            {"C1FillLevelEdgeAdvancedMinVariation" ,"FillLevelEdgeAdvancedMinVariation" },
            {"C1LeftHighCapValue","LeftHighCapThreshold"},
            {"C1RightHighCapValue","RightHighCapThreshold"}

        };

        // Mapping for C2
        public static readonly Dictionary<string, string> C2Mapping = new Dictionary<string, string>
        {
            {"C2ExposureTxt", "Exposure"},
            {"C2FillLevelIntensityEnable", "FillLevelIntensityEnable"},
            {"C2FillLevelCenterXTextBoxValue", "FillLevelIntensityX"},
            {"C2FillLevelCenterYTextBoxValue", "FillLevelIntensityY"},
            {"C2FillLevelCenterWidthTextBoxValue", "FillLevelIntensityWidth"},
            {"C2FillLevelCenterHeightTextBoxValue", "FillLevelIntensityHeight"},
            {"C2FillLevelCenterAngleTextBoxValue", "FillLevelIntensityAngle"},
            {"C2FillLevelIntensityRejectionStatus", "FillLevelIntensityRejectionStatus"},
            {"C2FillLevelIntensityGreaterThanLessThan","FillLevelGreaterThanLessThan"},
            {"C2FillLevelValue", "FillLevelIntensityThreshold"},
            {"C2FillLevelEdgeEnable", "FillLevelEdgeEnable"},
            {"C2FillLevelEdgeX", "FillLevelEdgeX"},
            {"C2FillLevelEdgeY", "FillLevelEdgeY"},
            {"C2FillLevelEdgeWidth", "FillLevelEdgeWidth"},
            {"C2FillLevelEdgeHeight", "FillLevelEdgeHeight"},
            {"C2FillLevelEdgeAngle", "FillLevelEdgeAngle"},
            {"C2FillLevelEdgeRejectionStatus", "FillLevelEdgeRejectionStatus"},
            {"C2FillLevelEdgeGreaterThanLessThan","FillLevelEdgeGreaterThanLessThan" },
            {"C2FillLevelEdgeThreshold", "FillLevelEdgeThreshold"},
            {"C2FillLevelEdgeStrength", "FillLevelEdgeStrength"},
            {"C2FillLevelIntensityFilterDropDownBox","FillLevelIntensityFilterDropDownBox" },
            {"C2FillLevelIntensityIncludeConditionDropDownBox","FillLevelIntensityIncludeConditionDropDownBox" },
            {"C2FillLevelIntensityConditionLowValue","FillLevelIntensityConditionLowValue" },
            {"C2FillLevelIntensityConditionHighValue","FillLevelIntensityConditionHighValue" },
            {"C2FillLevelEdgeAdvancedNumber","FillLevelEdgeAdvancedNumber" },
            {"C2FillLevelEdgeAdvancedFilterTypeDropDown","FillLevelEdgeAdvancedFilterTypeDropDown" },
            {"C2FillLevelEdgeAdvancedPolarityDropDown","FillLevelEdgeAdvancedPolarityDropDown" },
            {"C2FillLevelEdgeAdvancedMinimum","FillLevelEdgeAdvancedMinimum" },
            {"C2FillLevelEdgeAdvancedSubregion","FillLevelEdgeAdvancedSubregion" },
            {"C2FillLevelEdgeAdvancedSmoothness","FillLevelEdgeAdvancedSmoothness" },
            {"C2FillLevelEdgeAdvancedEdgeThreshold","FillLevelEdgeAdvancedEdgeThreshold" },
            {"C2FillLevelEdgeAdvancedMinVariation" ,"FillLevelEdgeAdvancedMinVariation" },
            {"C2LeftHighCapValue","LeftHighCapThreshold"},
            {"C2RightHighCapValue","RightHighCapThreshold"}

        };

        // Mapping for C3
        public static readonly Dictionary<string, string> C3Mapping = new Dictionary<string, string>
        {
            {"C3ExposureTxt", "Exposure"},
            {"C3FillLevelIntensityEnable", "FillLevelIntensityEnable"},
            {"C3FillLevelCenterXTextBoxValue", "FillLevelIntensityX"},
            {"C3FillLevelCenterYTextBoxValue", "FillLevelIntensityY"},
            {"C3FillLevelCenterWidthTextBoxValue", "FillLevelIntensityWidth"},
            {"C3FillLevelCenterHeightTextBoxValue", "FillLevelIntensityHeight"},
            {"C3FillLevelCenterAngleTextBoxValue", "FillLevelIntensityAngle"},
            {"C3FillLevelIntensityRejectionStatus", "FillLevelIntensityRejectionStatus"},
            {"C3FillLevelIntensityGreaterThanLessThan","FillLevelGreaterThanLessThan"},
            {"C3FillLevelValue", "FillLevelIntensityThreshold"},
            {"C3FillLevelEdgeEnable", "FillLevelEdgeEnable"},
            {"C3FillLevelEdgeX", "FillLevelEdgeX"},
            {"C3FillLevelEdgeY", "FillLevelEdgeY"},
            {"C3FillLevelEdgeWidth", "FillLevelEdgeWidth"},
            {"C3FillLevelEdgeHeight", "FillLevelEdgeHeight"},
            {"C3FillLevelEdgeAngle", "FillLevelEdgeAngle"},
            {"C3FillLevelEdgeRejectionStatus", "FillLevelEdgeRejectionStatus"},
            {"C3FillLevelEdgeGreaterThanLessThan","FillLevelEdgeGreaterThanLessThan" },
            {"C3FillLevelEdgeThreshold", "FillLevelEdgeThreshold"},
            {"C3FillLevelEdgeStrength", "FillLevelEdgeStrength"},
            {"C3FillLevelIntensityFilterDropDownBox","FillLevelIntensityFilterDropDownBox" },
            {"C3FillLevelIntensityIncludeConditionDropDownBox","FillLevelIntensityIncludeConditionDropDownBox" },
            {"C3FillLevelIntensityConditionLowValue","FillLevelIntensityConditionLowValue" },
            {"C3FillLevelIntensityConditionHighValue","FillLevelIntensityConditionHighValue" },
            {"C3FillLevelEdgeAdvancedNumber","FillLevelEdgeAdvancedNumber" },
            {"C3FillLevelEdgeAdvancedFilterTypeDropDown","FillLevelEdgeAdvancedFilterTypeDropDown" },
            {"C3FillLevelEdgeAdvancedPolarityDropDown","FillLevelEdgeAdvancedPolarityDropDown" },
            {"C3FillLevelEdgeAdvancedMinimum","FillLevelEdgeAdvancedMinimum" },
            {"C3FillLevelEdgeAdvancedSubregion","FillLevelEdgeAdvancedSubregion" },
            {"C3FillLevelEdgeAdvancedSmoothness","FillLevelEdgeAdvancedSmoothness" },
            {"C3FillLevelEdgeAdvancedEdgeThreshold","FillLevelEdgeAdvancedEdgeThreshold" },
            {"C3FillLevelEdgeAdvancedMinVariation" ,"FillLevelEdgeAdvancedMinVariation" },
            {"C3LeftHighCapValue","LeftHighCapThreshold"},
            {"C3RightHighCapValue","RightHighCapThreshold"}

        };

        // Set of control names that are checkboxes/toggles
        public static readonly HashSet<string> ToggleControlNames = new HashSet<string>
        {
            "FillLevelIntensityEnable",
            "FillLevelIntensityRejectionStatus",
            "FillLevelEdgeEnable",
            "FillLevelEdgeRejectionStatus",
            "FillLevelEdgeGreaterThanLessThan",
            "FillLevelGreaterThanLessThan"
        };

        public static readonly HashSet<string> ComboBoxControlNames = new HashSet<string>
            {
            "FillLevelIntensityFilterDropDownBox",
             "FillLevelIntensityIncludeConditionDropDownBox",
             "FillLevelEdgeAdvancedPolarityDropDown",
             "FillLevelEdgeAdvancedFilterTypeDropDown"
    // Add more as needed
            };

        /// <summary>
        /// Apply a recipe to the DA OperatorView using the provided mapping.
        /// Includes debug output for tracing connection and value setting.
        /// </summary>
        public static async Task ApplyRecipeAsync(
            string hostName,
            string projectName,
            string operatorViewName,
            Dictionary<string, string> recipeDict,
            Dictionary<string, string> mappingDict,
            IProgress<string> progress = null) // new optional progress parameter
        {
            foreach (var mapKvp in mappingDict)
            {
                string iniKey = mapKvp.Key;
                string controlName = mapKvp.Value;

                if (!recipeDict.TryGetValue(iniKey, out string value))
                    continue;

                bool isToggle = ToggleControlNames.Contains(controlName);
                bool isCombo = ComboBoxControlNames.Contains(controlName);

                try
                {
                    progress?.Report($"Preparing to set {controlName} = {value}");

                    if (isToggle)
                    {
                        bool set = false;
                        Exception lastEx = null;

                        // First try ToggleButtonUIControl
                        try
                        {
                            var toggle = new ToggleButtonUIControl
                            {
                                HostName = hostName,
                                ProjectName = projectName,
                                PageName = operatorViewName,
                                ControlName = controlName
                            };
                            await toggle.ConnectAsync();
                            toggle.Checked = value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase);
                            await Task.Delay(200);
                            toggle.Disconnect();
                            set = true;
                            progress?.Report($"Set {controlName} (toggle) = {toggle.Checked}");
                        }
                        catch (Exception ex)
                        {
                            lastEx = ex;
                            progress?.Report($"Warning: ToggleButtonUIControl failed for '{controlName}': {ex.Message}");
                        }

                        // If ToggleButton fails, try CheckBoxUIControl
                        if (!set)
                        {
                            try
                            {
                                var checkBox = new CheckBoxUIControl
                                {
                                    HostName = hostName,
                                    ProjectName = projectName,
                                    PageName = operatorViewName,
                                    ControlName = controlName
                                };
                                await checkBox.ConnectAsync();
                                checkBox.Checked = value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase);
                                await Task.Delay(200);
                                checkBox.Disconnect();
                                set = true;
                                progress?.Report($"Set {controlName} (checkbox) = {checkBox.Checked}");
                            }
                            catch (Exception ex)
                            {
                                lastEx = ex;
                                progress?.Report($"Warning: CheckBoxUIControl failed for '{controlName}': {ex.Message}");
                            }
                        }

                        if (!set)
                        {
                            progress?.Report($"Error: Could not set toggle/checkbox for control '{controlName}'. Last exception: {lastEx?.Message}");
                        }
                    }
                    else if (isCombo)
                    {
                        try
                        {
                            var combo = new ComboBoxUIControl
                            {
                                HostName = hostName,
                                ProjectName = projectName,
                                PageName = operatorViewName,
                                ControlName = controlName
                            };
                            await combo.ConnectAsync();

                            // Update via UI thread safely if necessary
                            if (combo.InvokeRequired)
                            {
                                combo.Invoke(new Action(() => { combo.SelectedText = value; }));
                            }
                            else
                            {
                                combo.SelectedText = value;
                            }

                            await Task.Delay(200);
                            combo.Disconnect();
                            progress?.Report($"Set {controlName} (combo) = {value}");
                        }
                        catch (Exception ex)
                        {
                            progress?.Report($"Error setting ComboBoxUIControl '{controlName}': {ex.Message}");
                        }
                    }
                    else
                    {
                        // Handle TextBoxUIControl
                        try
                        {
                            var textBox = new TextBoxUIControl
                            {
                                HostName = hostName,
                                ProjectName = projectName,
                                PageName = operatorViewName,
                                ControlName = controlName
                            };
                            await textBox.ConnectAsync();
                            textBox.Text = value;
                            await Task.Delay(200);
                            textBox.Disconnect();
                            progress?.Report($"Set {controlName} (text) = {value}");
                        }
                        catch (Exception ex)
                        {
                            progress?.Report($"Error setting TextBoxUIControl '{controlName}': {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"General exception setting '{controlName}': {ex.Message}");
                }
            }
        }
    }
}