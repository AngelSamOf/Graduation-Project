using System;

static class EventEmitter
{
    static public Action<Direction, SymbolBase> MoveSymbol;
    static public Action EndMoveSymbol;
    static public Action<WinCombination> WinCombination;
    static public Action<PlayerCharacterComponent> ClickCharacter;
}