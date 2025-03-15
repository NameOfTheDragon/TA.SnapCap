// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.ComponentModel;
using System.Configuration;
using NLog;

namespace TA.SnapCap.HardwareSimulator
    {
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed class Settings
        {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
            {
            log.Info("Settings changed");
            }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
            {
            log.Info("Settings saving");
            }
        }
    }