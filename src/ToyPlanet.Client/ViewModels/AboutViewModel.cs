namespace ToyPlanet.Client.ViewModels
{
    /// <summary>
    /// ViewModel для сторінки About
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Про додаток";
            ApplicationName = "ToyPlanet";
            Version = "1.0.0";
            Author = "ToyPlanet Development Team";
            Description = "Кросс-платформний клієнт для системи управління магазином іграшок";
            ReleaseDate = "16 січня 2026 року";
            Website = "https://github.com/viciusername/toy-shopp";
        }

        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string Website { get; set; }
    }
}