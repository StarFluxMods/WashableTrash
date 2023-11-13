using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;
using UnityEngine.VFX;

namespace KitchenWashableTrash.Customs
{
    public class WashedTrash : CustomItem
    {
        public override string UniqueNameID => "WashedTrash";
        public override GameObject Prefab => MaterialUtils.AssignMaterialsByNames(Mod.Bundle.LoadAsset<GameObject>("Bin Bag"));

        public override void OnRegister(Item gameDataObject)
        {
            foreach (Transform child in gameDataObject.Prefab.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.name.ToLower().StartsWith("vfx_"))
                {
                    VisualEffect effect = child.gameObject.AddComponent<VisualEffect>();
                    foreach (VisualEffectAsset asset in Resources.FindObjectsOfTypeAll<VisualEffectAsset>())
                    {
                        if (asset.name == "Knife Glint")
                        {
                            effect.visualEffectAsset = asset;
                        }
                    }
                }
            }
        }
    }
}