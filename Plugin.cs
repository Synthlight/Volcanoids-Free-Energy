using Base_Mod;
using JetBrains.Annotations;

namespace Free_Energy {
    [UsedImplicitly]
    public class Plugin : BaseGameMod {
        protected override string ModName    => "Free-Energy";
        protected override bool   UseHarmony => true;
    }
}