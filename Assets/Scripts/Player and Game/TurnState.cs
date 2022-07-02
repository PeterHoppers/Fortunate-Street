//an enum to keep track of what the player is doing. Would like to use it to control when the player is in menus and when the player can move
public enum TurnState
{
    BeforeRoll,
    Rolling,
    Moving,
    Stopped,
    Landed,
    Finished
}
