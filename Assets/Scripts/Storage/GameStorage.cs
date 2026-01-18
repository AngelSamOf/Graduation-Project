public static class GameStorage
{
    public static FieldObject FieldData => _fieldData;
    private static FieldObject _fieldData;

    public static void SetFieldData(FieldObject data)
    {
        _fieldData = data;
    }
}