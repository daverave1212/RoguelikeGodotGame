	Unit
To create a new unit, make a duplicate of UnitTemplate.tscn.

If you want this unit to be controlled by the player, give it a UnitPlayerController child Node.
If you want this unit to be controlled by the AI, give it a UnitAIController child Node.

If you want the unit to be able to attack, give it a Weapon child Node.
Drag and drop a weapon from the Prefabs/Weapons/WeaponPrefabs folder onto the unit node.


	Unit Components Explained
A Unit is essentially just a KinematicBody2D (with a Unit script) with an animated sprite child.
By itself, it has a simple API for moving (this API is used by the Player/AI controllers).
  In the API, there are also methods for shooting and acquiring targets, but these are taken directly from its components.

It has some simple components:
- StatsComponent: just some simple stats, like Level, Health, etc. No other functionality.
- TargetAcquirer: provides the method for acquiring the closest target within a range; used by the weapon (shooting)
- UnitAnimationHandler: Plays 2 animations - Walk and Idle - by detecting the frames the unit moves

It can have some extra optional components:
- UnitPlayerController: the player can control this Unit
- UnitAIController: the AI controls this unit automatically (it moves it, shoots, etc)

It shoud be given a Weapon node; otherwise, the unit can't shoot.
