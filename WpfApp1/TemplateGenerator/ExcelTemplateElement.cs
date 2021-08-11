using System;
using System.Text.RegularExpressions;
using Templator.TransformElements;

namespace Templator.TemplateGenerator
{
    /// <summary>
    /// Представляет текстовый элемент с настройками для передачи в него данных из Excel-документа
    /// </summary>
    class ExcelTemplateElement : TemplateElement
    {
        private static readonly Regex ExcelPattern;
        private string _constCell;

        /// <summary>
        /// Название Excel-столбца с которого (или начиная с которого, если IsMultiple = true) нужно передать данные в текстовый элемент
        /// </summary>
        public override string TextSource
        {
            get => Source;
            set
            {
                Source = value;
            }
        }

        /// <summary>
        /// Если данные не многочисленны, то они берутся из одной ячейки, которая сохраняется сюда
        /// </summary>
        public string ConstCell
        {
            get => _constCell;
            set
            {
                if (!ExcelPattern.IsMatch(value))
                {
                    throw new ArgumentException("Введено некорректное название столбца Excel");
                }

                _constCell = value;
            }

        }

        public ExcelTemplateElement(TextElementTransform element) : base(element) { }

        public ExcelTemplateElement(TextElementTransform element, string column, bool isMultiple) :  base(element, column, isMultiple)
        {
            if (!ExcelPattern.IsMatch(column))
            {
                throw new ArgumentException("Введено некорректное название столбца Excel");
            }
        }

        static ExcelTemplateElement()
        {
            ExcelPattern = new Regex("\\b([A-Z]+)(\\d+)\\b");
        }

        public override string ToString()
        {
            return $"TextElement: {TextElement} ExcelColumn: {TextSource} IsMultiple: {IsMultipleValues}";
        }
    }
}
