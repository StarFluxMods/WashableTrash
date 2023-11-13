using System.Collections.Generic;
using System.IO;
using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using System.Linq;
using System.Reflection;
using Kitchen;
using KitchenData;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenWashableTrash.Customs;
using UnityEngine;

namespace KitchenWashableTrash
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.washabletrash";
        public const string MOD_NAME = "Washable Trash";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.1.4";

        public static AssetBundle Bundle;
        public static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            x();
        }

        protected override void OnUpdate()
        {
        }

        public void x()
        {
            Texture2D texture = PrefabSnapshot.GetItemSnapshot(GameData.Main.Get<Item>(ItemReferences.BinBag).Prefab);
         
            //then Save To Disk as PNG
            byte[] bytes = texture.EncodeToPNG();
            var dirPath = Application.dataPath + "/../SaveImages/";
            if(!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + "Image" + ".png", bytes);
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_GUID);
            Logger = InitLogger();
            AddGameDataObject<WashedTrash>();

            FieldInfo processes = ReflectionUtils.GetField<Item>("Processes");
            Events.BuildGameDataEvent += (sender, args) =>
            {
                if (args.gamedata.TryGet(ItemReferences.BinBag, out Item binBag))
                {
                    List<Item.ItemProcess> binBagProcesses = (List<Item.ItemProcess>) processes.GetValue(binBag);
                    binBagProcesses.Add(new Item.ItemProcess
                    {
                        IsBad = false,
                        Duration = 1,
                        Process = (Process)GDOUtils.GetExistingGDO(ProcessReferences.Clean),
                        Result = (Item)GDOUtils.GetCustomGameDataObject<WashedTrash>().GameDataObject,
                    });
                    processes.SetValue(binBag, binBagProcesses);
                }

            };
        }
    }
}

