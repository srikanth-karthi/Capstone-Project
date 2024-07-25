namespace EventManagementApp.Exceptions
{
    public class NoEventCategoryFoundException : Exception
    {
        public NoEventCategoryFoundException() : base("Event Category not found for given Id") { }
    }
}
