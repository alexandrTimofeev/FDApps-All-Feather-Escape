public static class CarrySystem
{
    public static ICarryable CurrentItem { get; set; }

    public static void Pick(ICarryable item)
    {
        if (CurrentItem != null)
            CurrentItem.Drop();

        CurrentItem = item;
        item.PickUp();
    }

    public static void DropCurrent()
    {
        if (CurrentItem != null)
        {
            CurrentItem.Drop();
            CurrentItem = null;
        }
    }
}
