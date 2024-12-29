namespace RentalSystem.ViewModels
{
    public class HeaderData
    {
        public string Logo { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
    public class MenuItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
