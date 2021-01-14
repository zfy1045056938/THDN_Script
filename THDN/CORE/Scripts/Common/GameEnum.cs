
//MATCH MODULE ALL [A] SIZE

//TILE

//Matches Collectiable Type
public enum TileType
{
    Obstcle,
    Normal,
	Breakable,
}

public enum MatchValue{
	MUT=0,
	STR=1,
	DEX=2,
	INT=3,
	SWORD=4,
	ARMOR=5,
	OTHER=6,	//other collectiable
	None=7,
}
public enum SlotType
{
	Normal,
	Obstacle,
	Breakable,
    
}

//Breakable Type
public enum BombType
{
	None,
	Column,
	Row,
	Adjacent,
	Color,
	Mixed,

}


public enum InterType
{
	Linear,
	EaseOut,
	EaseIn,
	SmoothStep,
	SmootherStep,
}




public enum StageType
{
	TOWN,
	DUNGEON,
	OUTDOOR,
}

public enum ItemRatity
{
	JUNK,
	NORMAL,
	RARE,
	EPIC,
}
//Enemy
public enum NpcType{
	NORMAL,
	ENEMY,	//InBattle(match3),or kill use by tools()
	QUEST,
	MERCHANT,
	BLACKSMITH,
	OTHER,
}

//FSM
public enum EntityAnimState{
	IDLE,
	MOVING,
	BATTLE,
	CASTING,
	DEAD,
	ATTACK,	//In AI plugins
	CAMP,
	DIALOGUE,


}

//Classes & Skills(For Player Classes to classIndex)
public enum Classes{
	Normal,
	Warrior,
	Hunter,
}


//FSM(COMBAT UND COMBO effect)
public enum DamageType {
	Normal=0,	//matches cause atk
	Block=1,	// when rv(dice) > (1-t.block.perc)
	Crit=2,		// when rv(dice) > (1-t.crit.perc)
	Skill=3,	//use skill check skill then dealDamage	
	Rage=4,		//when rage pool is 1 then can cause skil
	

}

public enum ElementDamageType
{
	None=0,
	Fire=1,
	Freeze=2,
	Posion=3,
	Frighting=4,
	ShenShan=5
}

public enum BuffType :int
{
	None=0,
	Atk=1,
	Armor=2,
	Health=3,
	Mana=4,
	MatchesBouns=4,
	STR=5,
	DEX=6,
	INTE=7,
	CON=8,
	CHAOS=9,
	VOID=10,


}

public enum LiveSkillType : int
{
	None=1,
	Dungeoneering =2,
	Leader=3,
	KissAss=4,
	LockPick=5,


}

///////////////////COMMON
//Common Enum
public enum SceneType{
	MainMenu,
	MainGame,
	LoadingPro,

}



////////////////////MIRROR::SERVER&CLIENT
//NETWORKSTATE
public enum NetworkState{
	None,
	Lobby,
	Online,
	World,
	HandShake,
    Offline,
}


public enum SkillElementType{
	None,
	Fire,
	Ice,
	Posion,
	Light,
	Dark,
}

public enum SkillTargetType{
	None,
	Both,
	Self,
	Target,
}



//

public enum CRaces{
	Qika,
	Kaian,
	Sodtyx,
	Vultus,
	Parlo,
	None,
}

public enum CType{
	None,
	Merchant,
	Npc,
	Enemy,
}

