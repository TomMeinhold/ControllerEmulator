namespace ControllerEmulatorAPI.Controller.Base
{
    using ControllerEmulatorAPI.Global.Attributes;
    using System;
    using System.Text.Json.Serialization;
    using System.Windows.Forms;

    [SettingsRootNode("Profile")]
    [Serializable]
    public class Profile
    {
        [SettingsNode("General")]
        [SettingsOption("Name")]
        [JsonPropertyName("ProfileName")]
        public string ProfileName { get; set; } = "Profile";

        [SettingsNode("Buttons")]
        [SettingsOption("A")]
        [JsonPropertyName("A")]
        /// <summary>
        /// Button 1
        /// </summary>
        public Keys A { get; set; }

        [SettingsNode("Buttons")]
        [SettingsOption("B")]
        [JsonPropertyName("B")]
        /// <summary>
        /// Button 2
        /// </summary>
        public Keys B { get; set; }

        [SettingsNode("Buttons")]
        [SettingsOption("X")]
        [JsonPropertyName("X")]
        /// <summary>
        /// Button 3
        /// </summary>
        public Keys X { get; set; }

        [SettingsNode("Buttons")]
        [SettingsOption("Y")]
        [JsonPropertyName("Y")]
        /// <summary>
        /// Button 4
        /// </summary>
        public Keys Y { get; set; }

        [SettingsNode("Shoulder buttons")]
        [SettingsOption("L")]
        [JsonPropertyName("L")]
        /// <summary>
        /// Button 5
        /// </summary>
        public Keys L { get; set; }

        [SettingsNode("Shoulder buttons")]
        [SettingsOption("R")]
        [JsonPropertyName("R")]
        /// <summary>
        /// Button 6
        /// </summary>
        public Keys R { get; set; }

        [SettingsNode("Shoulder buttons")]
        [SettingsOption("ZL")]
        [JsonPropertyName("ZL")]
        public Keys ZL { get; set; }

        [SettingsNode("Shoulder buttons")]
        [SettingsOption("ZR")]
        [JsonPropertyName("ZR")]
        public Keys ZR { get; set; }

        [SettingsNode("Menu buttons")]
        [SettingsOption("Plus")]
        [JsonPropertyName("Plus")]
        /// <summary>
        /// Button 7
        /// </summary>
        public Keys Plus { get; set; }

        [SettingsNode("Menu buttons")]
        [SettingsOption("Minus")]
        [JsonPropertyName("Minus")]
        /// <summary>
        /// Button 8
        /// </summary>
        public Keys Minus { get; set; }

        [SettingsNode("DPad")]
        [SettingsOption("DPad Up")]
        [JsonPropertyName("DPad_Up")]
        public Keys DPad_Up { get; set; }

        [SettingsNode("DPad")]
        [SettingsOption("DPad Down")]
        [JsonPropertyName("DPad_Down")]
        public Keys DPad_Down { get; set; }

        [SettingsNode("DPad")]
        [SettingsOption("DPad Left")]
        [JsonPropertyName("DPad_Left")]
        public Keys DPad_Left { get; set; }

        [SettingsNode("DPad")]
        [SettingsOption("DPad Right")]
        [JsonPropertyName("DPad_Right")]
        public Keys DPad_Right { get; set; }

        [SettingsNode("Analog L")]
        [SettingsOption("Analog L Click")]
        [JsonPropertyName("Analog_L_Click")]
        /// <summary>
        /// Button 9
        /// </summary>
        public Keys Analog_L_Click { get; set; }

        [SettingsNode("Analog R")]
        [SettingsOption("Analog R Click")]
        [JsonPropertyName("Analog_R_Click")]
        /// <summary>
        /// Button 10
        /// </summary>
        public Keys Analog_R_Click { get; set; }

        [SettingsNode("Analog L")]
        [SettingsOption("Forward")]
        [JsonPropertyName("Analog_L_Up")]
        public Keys Analog_L_Up { get; set; }

        [SettingsNode("Analog L")]
        [SettingsOption("Backward")]
        [JsonPropertyName("Analog_L_Down")]
        public Keys Analog_L_Down { get; set; }

        [SettingsNode("Analog L")]
        [SettingsOption("Left")]
        [JsonPropertyName("Analog_L_Left")]
        public Keys Analog_L_Left { get; set; }

        [SettingsNode("Analog L")]
        [SettingsOption("Right")]
        [JsonPropertyName("Analog_L_Right")]
        public Keys Analog_L_Right { get; set; }

        [SettingsNode("Special buttons")]
        [SettingsOption("Run")]
        [JsonPropertyName("RunKey")]
        public Keys RunKey { get; set; }
    }
}