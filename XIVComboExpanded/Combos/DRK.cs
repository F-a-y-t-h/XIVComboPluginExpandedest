using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class DRK
    {
        public const byte JobID = 32;

        public const uint
            HardSlash = 3617,
            Unleash = 3621,
            BloodWeapon = 3625,
            CarveAndSpit = 3643,
            SyphonStrike = 3623,
            Souleater = 3632,
            Quietus = 7391,
            Bloodspiller = 7392,
            FloodOfDarkness = 16466,
            EdgeOfDarkness = 16467,
            StalwartSoul = 16468,
            FloodOfShadow = 16469,
            EdgeOfShadow = 16470,
            LivingShadow = 16472,
            Shadowbringer = 25757;

        public static class Buffs
        {
            public const ushort
                BloodWeapon = 742,
                Delirium = 1972;
        }

        public static class Debuffs
        {
            public const ushort Placeholder = 0;
        }

        public static class Levels
        {
            public const byte
                SyphonStrike = 2,
                Souleater = 26,
                FloodOfDarkness = 30,
                EdgeOfDarkness = 40,
                CarveAndSpit = 60,
                Bloodpiller = 62,
                Quietus = 64,
                Delirium = 68,
                StalwartSoul = 40,
                Shadow = 74,
                EdgeOfShadow = 74,
                LivingShadow = 80,
                SaltAndDarkness = 86,
                Shadowbringer = 90;
        }
    }

    internal class DarkSouleaterCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.DarkSouleaterCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == DRK.Souleater)
            {
                var gauge = GetJobGauge<DRKGauge>();
                var gcd = GetCooldown(DRK.HardSlash);

                if ((LocalPlayer.CurrentMp > 9000 || gauge.DarksideTimeRemaining < 10) && gcd.CooldownRemaining > 1)
                {
                    if (level >= DRK.Levels.EdgeOfShadow)
                        return DRK.EdgeOfShadow;
                    if (level >= DRK.Levels.EdgeOfDarkness)
                        return DRK.EdgeOfDarkness;
                    if (level >= DRK.Levels.FloodOfDarkness && level < DRK.Levels.EdgeOfDarkness)
                        return DRK.FloodOfDarkness;
                }

                if (lastComboMove == DRK.SyphonStrike && level >= DRK.Levels.Bloodpiller && gauge.Blood >= 90)
                {
                    return DRK.Bloodspiller;
                }

                if ((gauge.Blood >= 80 && HasEffect(DRK.Buffs.BloodWeapon)) || HasEffect(DRK.Buffs.Delirium))
                        return DRK.Bloodspiller;

                if (comboTime > 0)
                {
                    if (lastComboMove == DRK.HardSlash && level >= DRK.Levels.SyphonStrike)
                        return DRK.SyphonStrike;

                    if (lastComboMove == DRK.SyphonStrike && level >= DRK.Levels.Souleater)
                        return DRK.Souleater;
                }

                return DRK.HardSlash;
            }

            return actionID;
        }
    }

    internal class DarkStalwartSoulCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.DarkStalwartSoulCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == (IsEnabled(CustomComboPreset.DarkEvilStalwartSoulCombo) ? DRK.Unleash : DRK.StalwartSoul))
            {
                var gauge = GetJobGauge<DRKGauge>();
                if (IsEnabled(CustomComboPreset.DRKOvercapFeature) && level > DRK.Levels.Quietus)
                {
                    if ((gauge.Blood >= 90 && HasEffect(DRK.Buffs.BloodWeapon)) || HasEffect(DRK.Buffs.Delirium))
                        return DRK.Quietus;
                }

                if (comboTime > 0 && lastComboMove == DRK.Unleash && level >= DRK.Levels.StalwartSoul)
                {
                    if (IsEnabled(CustomComboPreset.DRKOvercapFeature) && (gauge.Blood >= 90 || (gauge.Blood >= 80 && HasEffect(DRK.Buffs.BloodWeapon))))
                        return DRK.Quietus;
                    return DRK.StalwartSoul;
                }

                return DRK.Unleash;
            }

            return actionID;
        }
    }

    internal class DarkBloodWeaponFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.DarkBloodWeaponFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (IsActionOffCooldown(DRK.BloodWeapon) || level < DRK.Levels.CarveAndSpit)
                return DRK.BloodWeapon;

            return actionID;
        }
    }

    internal class DarkLivingShadowFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.DarkLivingShadowFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            var gauge = GetJobGauge<DRKGauge>();
            if (IsActionOffCooldown(DRK.LivingShadow) && level >= DRK.Levels.LivingShadow && gauge.Blood >= 50)
                return DRK.LivingShadow;

            return actionID;
        }
    }
}
