using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Syncfusion.XlsIO;
using Templator.ImageProcessing;
using Templator.TransformElements;

namespace Templator.TemplateGenerator
{
    /// <summary>
    /// Представляет собой генератор шаблонов, загружающий данные из Excel-документа и заполняющий
    /// текстовые UI-элементы, хранящиеся в его ObservableCollection типа TemplateElement
    /// </summary>
    public class TemplateGenerator
    {
        private ObservableCollection<TemplateElement> _settings;
        private static readonly Regex Regex;
        private static string _excelPath;

        /// <summary>
        /// Коллекция элементов типа TemplateElement, в которой содержатся настроенные текстовые UI-элементы для заполнения их данными из Excel-документа
        /// </summary>
        public ObservableCollection<TemplateElement> TemplateSettings
        {
            get => _settings;
        }

        static TemplateGenerator()
        {
            Regex = new Regex("(\\d+)");
        }

        public TemplateGenerator()
        {
            if (_settings == null)
            {
                _settings = new ObservableCollection<TemplateElement>();
            }

            _excelPath = ComponentService.GetExcelFilename();

        }

        public TemplateGenerator(TemplateGenerator generator) :this()
        {
            _settings = generator.TemplateSettings;
        }

        public TemplateGenerator(IEnumerable<TextElementTransform> transformUiTexts) : this()
        {
            _settings = new ObservableCollection<TemplateElement>();

            foreach (var transform in transformUiTexts)
            {
                var template = new ExcelTemplateElement(transform);
                TemplateSettings.Add(template);
            }
        }

        /// <summary>
        /// Загружает данные из указанного Excel-документа в текстовые UI-элементы, хранящиеся в коллекции типа
        /// ObservableCollection и
        /// делает рендеринг объекта типа obj, который должен быть графическим контейнером (чаще всего Canvas) в формат png, сохраняя выходной файл в указанную
        /// пользователем директорию
        /// </summary>
        /// <param name="obj"></param>
        public void Generate(object obj)
        {
            if (obj is not Canvas canvas) return;
            string GetTextFromCell(IWorksheet worksheet, string excelCell)
            {
                return worksheet.Range[excelCell].Text;
            }

            var savePath = ComponentService.GetSavePath();

            using var engine = new ExcelEngine();
            IApplication app = engine.Excel;

            IWorkbook workbook = app.Workbooks.Open(_excelPath);

            app.DefaultVersion = workbook.Version;

            IWorksheet worksheet = workbook.Worksheets[0];

            string columnLetter;
            int columnNumber = int.Parse(Regex.Match(TemplateSettings[0].TextSource).Value);

            string resultText = "init";
            var builder = new StringBuilder();

            while (!string.IsNullOrEmpty(resultText))
            {
                builder.Append(savePath);
                foreach (var setting in TemplateSettings)
                {
                    columnLetter = new Regex("([A-Z]+)").Match(setting.TextSource).Value;
                    resultText = GetTextFromCell(worksheet, $"{columnLetter}{columnNumber}");
                    
                    ((TextBlock)setting.UIElement).Text = resultText;
                    canvas.UpdateLayout();
                }

                builder.Insert(builder.ToString().Length - 4, columnNumber);
                CanvasConverter.PngExport(canvas, builder.ToString());
                columnNumber++;
                builder.Clear();
            }

            engine.Dispose();
            OpenDirectory(savePath.Remove(savePath.Length - 4, 4));
        }

        /// <summary>
        /// Открывает директорию, в которую сохранялись выходные файлы рендеринга
        /// </summary>
        /// <param name="path"></param>
        private static void OpenDirectory(string path)
        {
            string directoryPath = Path.GetDirectoryName(path) ?? string.Empty;
            //if(!string.IsNullOrEmpty(directoryPath))
               // System.Diagnostics.Process.Start(directoryPath);
        }
    }
}
