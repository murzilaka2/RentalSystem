using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RentalSystem.ViewModels;

namespace RentalSystem.Pages.Admin.General_settings
{
    public class GeneralSettingsModel : PageModel
    {
        private readonly string _jsonFilePath;
        private readonly string _jsonFooterFilePath;
        private readonly string _jsonHeaderFilePath;
        public GeneralSettingsModel()
        {
            _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "JsonData/emails.json");
            _jsonFooterFilePath = Path.Combine(Directory.GetCurrentDirectory(), "JsonData/footer.json");
            _jsonHeaderFilePath = Path.Combine(Directory.GetCurrentDirectory(), "JsonData/header.json");
        }

        [BindProperty]
        public List<string> Recipients { get; set; }

        [BindProperty]
        public FooterData FooterData { get; set; }

        [BindProperty]
        public HeaderData HeaderData { get; set; }

        public void OnGet()
        {
            if (_jsonFilePath != null)
            {
                var jsonData = System.IO.File.ReadAllText(_jsonFilePath);
                var settings = JsonConvert.DeserializeObject<AppSettings>(jsonData);
                Recipients = settings?.EmailsList?.Recipients ?? new List<string>();
            }
            //Header data
            if (_jsonHeaderFilePath != null)
            {
                var headerData = System.IO.File.ReadAllText(_jsonHeaderFilePath);
                HeaderData = JsonConvert.DeserializeObject<HeaderData>(headerData);
            }
            //Footer data
            if (_jsonFooterFilePath != null)
            {
                var jsonContent = System.IO.File.ReadAllText(_jsonFooterFilePath);
                FooterData = JsonConvert.DeserializeObject<FooterData>(jsonContent);
                if (FooterData.Emails != null && FooterData.Emails.Count > 0)
                {
                    FooterData.EmailsForTextArea = String.Join("\n", FooterData.Emails);
                }
            }
        }
        public IActionResult OnPost()
        {
            var jsonData = System.IO.File.ReadAllText(_jsonFilePath);
            var settings = JsonConvert.DeserializeObject<AppSettings>(jsonData);
            if (settings != null && settings.EmailsList != null)
            {
                settings.EmailsList.Recipients = Recipients;
            }
            try
            {
                var updatedJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                System.IO.File.WriteAllText(_jsonFilePath, updatedJson);
                TempData["Successfully"] = true;
            }
            catch
            {
                TempData["Error"] = "There's been a mistake. Please try again later.";
            }
            return Redirect("/adminDashboard/generalSettings");
        }
        public IActionResult OnPostHeaderSettingsFormAsync()
        {
            TempData["ActiveTab"] = "HeaderContent";
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var jsonContent = JsonConvert.SerializeObject(HeaderData, Formatting.Indented);
                System.IO.File.WriteAllText(_jsonHeaderFilePath, jsonContent);
                TempData["Successfully"] = true;
            }
            catch (Exception)
            {
                TempData["Error"] = "There's been a mistake. Please try again later.";
            }
            return Redirect("/adminDashboard/generalSettings");
        }
        public IActionResult OnPostFooterSettingsFormAsync()
        {
            TempData["ActiveTab"] = "FooterContent";
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                FooterData.Emails = FooterData.EmailsForTextArea.Split("\n").ToList();
                var jsonContent = JsonConvert.SerializeObject(FooterData, Formatting.Indented);
                System.IO.File.WriteAllText(_jsonFooterFilePath, jsonContent);
                TempData["Successfully"] = true;
            }
            catch (Exception)
            {
                TempData["Error"] = "There's been a mistake. Please try again later.";
            }
            return Redirect("/adminDashboard/generalSettings");
        }
        public class EmailsList
        {
            public List<string> Recipients { get; set; }
        }
        public class AppSettings
        {
            public EmailsList EmailsList { get; set; }
        }
    }
}
