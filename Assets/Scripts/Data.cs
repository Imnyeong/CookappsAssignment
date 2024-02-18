#region Unit
public enum UnitType
{
    Character,
    Enemy
}
public enum UnitState
{
    Idle,
    Move,
    Attack,
    Skill,
    Death
}
public enum Elemental
{
    Fire,
    Water,
    Grass
}
public enum Profession
{
    Tanker,
    Warrior,
    Supporter
}
public enum SkillType
{
    Attack,
    Heal,
    Special
}
#endregion
#region Stage
public enum StageState
{ 
    Ready,
    Play,
    Clear,
    Defeat
}
#endregion
#region Lobby
public enum LobbyState
{
    Intro,
    StageSelect,
    CharacterSelect
}
#endregion
#region Scene
public enum SceneType
{
    Lobby,
    InGame,
}
#endregion
public enum PositionState
{
    Unselected,
    Selected
}
public enum SelectState
{
    Unselected,
    Selected
}