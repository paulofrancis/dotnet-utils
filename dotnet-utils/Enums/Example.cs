namespace dotnet_utils.Enums
{
    public class Example : Enumeration
    {
        public static Example Example1 = new Example(1, nameof(Example1));
        public static Example Example2 = new Example(2, nameof(Example2));
        public static Example Example3 = new Example(3, nameof(Example3));

        public Example(int id, string name) : base(id, name) { }
    }
}
