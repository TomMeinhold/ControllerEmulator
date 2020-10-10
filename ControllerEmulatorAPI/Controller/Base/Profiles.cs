namespace ControllerEmulatorAPI.Controller.Base
{
    using ControllerEmulatorAPI.UserInterface.UserControls;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Controls;

    public static class Profiles
    {
        private readonly static List<UIProfile> profiles = new List<UIProfile>();

        public static void Load(DirectoryInfo directory)
        {
            profiles.Clear();
            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    if (file.Extension == ".json")
                    {
                        profiles.Add(new UIProfile(Serializer.Deserialize<Profile>(file)));
                    }
                }
            }
            else
            {
                directory.Create();
            }
        }

        public static void Save(DirectoryInfo directory)
        {
            int i = 0;

            if (!directory.Exists)
            {
                directory.Create();
            }

            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }

            profiles.ForEach((x) =>
            {
                i++;
                Serializer.Serialize(new FileInfo(directory.FullName + $"//Profile{i}.json"), x.Profile);
            });
        }

        public static List<UIProfile> GetProfiles()
        {
            return profiles;
        }

        public static void AddToUI(UIElementCollection ts)
        {
            profiles.ForEach(x => ts.Add(x));
        }
    }
}