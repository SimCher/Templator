using System;
using System.Windows;
using System.Windows.Controls;
using Templator.TransformElements;

namespace Templator.TemplateGenerator
{
    /// <summary>
    /// Абстрактный класс, представляющий текстовый элемент, который должен отображать данные из
    /// другого источника (БД, Excel, JSON и т.п.)
    /// </summary>
    public abstract class TemplateElement
    {
        protected TextElementTransform TextElement;
        protected string Source;
        protected bool IsMultipleValues;

        /// <summary>
        /// Текстовый элемент, который нужно заполнить.
        /// Базовая реализация содержит проверку на null с throw NullReferenceException
        /// </summary>
        public virtual TextElementTransform Element
        {
            get => TextElement;
            protected set
            {
                if (value != null)
                {
                    TextElement = value;
                    return;
                }

                throw new NullReferenceException($"{nameof(value)} было null");
            }
        }

        /// <summary>
        /// Нетрансформируемый текстовый элемент, который нужно заполнить
        /// </summary>
        public virtual UIElement UIElement
        {
            get => ((Border)TextElement.InputElement).Child;
        }

        /// <summary>
        /// Источник с которого будут браться данные для заполнения текстового элемента (строка БД, ячейка Excel и т.п.).
        /// Базовая реализация не содержит каких-либо проверок на корректность значения
        /// </summary>
        public virtual string TextSource { get => Source; set => Source = value; }

        /// <summary>
        /// Возвращает true, если, например, столбец БД содержит разные данные во всех строках.
        /// false, если столбец содержит одинаоквые данные или только одну строку
        /// </summary>
        public virtual bool IsMultiple
        {
            get => IsMultipleValues;
            set => IsMultipleValues = value;
        }

        public TemplateElement(TextElementTransform element)
        {
            if (element == null)
            {
                throw new NullReferenceException($"{nameof(element)} было null");
            }

            TextElement = element;
        }


        public TemplateElement(TextElementTransform element, string source, bool isMultiple) : this(element)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new NullReferenceException($"{nameof(source)} было null");
            }

            TextElement = element;
            Source = source;
            IsMultipleValues = isMultiple;
        }

        public override string ToString()
        {
            return $"TextElement: {TextElement} Source: {TextSource} Multiple: {IsMultipleValues}";
        }
    }
}
