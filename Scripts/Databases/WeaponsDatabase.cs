using Godot;
using System;

public class WeaponsDatabase
{
	public static WeaponDTO[] Weapons = {
		new WeaponDTO {
			Name = "Broadsword",
			Level = 5,
			Path = "res://Prefabs/Weapons/WeaponPrefabs/BroadswordWeapon.tscn"
		},
		new WeaponDTO {
			Name = "Bow",
			Level = 1,
			Path = "res://Prefabs/Weapons/WeaponPrefabs/BroadswordWeapon.tscn"
		}
	};
	
	public static Weapon Create(string weaponName)
	{
		var foundWeaponDTO = Array.Find(Weapons, weapon => weapon.Name == weaponName);

		if (foundWeaponDTO == null)
		{
			return null;
		}
		
		var weaponPrefab = GD.Load<PackedScene>(foundWeaponDTO.Path);
		var weaponNode = weaponPrefab.Instance();
		
		return (Weapon) weaponNode;
	}
}
