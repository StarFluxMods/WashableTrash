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
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Bin Bag").AssignVFXByNames().AssignMaterialsByNames();

        public override void OnRegister(Item gameDataObject)
        {
        }
    }
}