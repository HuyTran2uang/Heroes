using UnityEngine;

namespace Inventory
{
    public class EquipmentManager : MonoBehaviourSingleton<EquipmentManager>
    {
        private MeleeWeapon _meleeWeapon;
        private RangeWeapon _rangeWeapon;
        private Armor _armor;

        public MeleeWeapon MeleeWeapon => _meleeWeapon;
        public RangeWeapon RangeWeapon => _rangeWeapon;
        public Armor Armor => _armor;

        public int MeleeDam
        {
            get
            {
                return _meleeWeapon.MeleeAtk;
            }
        }
        public int RangeDam
        {
            get
            {
                return _rangeWeapon.RangeAtk;
            }
        }
        public int Hp
        {
            get
            {
                return _armor.HP;
            }
        }

        public void Equip(MeleeWeapon meleeWeapon)
        {
            _meleeWeapon = meleeWeapon;
            PlayerPrefs.SetString($"{typeof(MeleeWeapon)}", _meleeWeapon.Name);
        }

        public void Equip(RangeWeapon rangeWeapon)
        {
            _rangeWeapon = rangeWeapon;
            PlayerPrefs.SetString($"{typeof(RangeWeapon)}", _rangeWeapon.Name);
        }

        public void Equip(Armor armor)
        {
            _armor = armor;
            PlayerPrefs.SetString($"{typeof(Armor)}", _armor.Name);
        }
    }
}
