﻿using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using WordLikeFb.Decorators;
using WordLikeFb.Serialization;

namespace WordLikeFb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool StructureIsVisible { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Structure_Visible(object sender, RoutedEventArgs e)
        {
            var blocks = rtbEditor.Document.Blocks;
            MakeStructureVisible(blocks);
            StructureIsVisible = true;
        }

        private void MakeStructureVisible(BlockCollection blocks)
        {
            if (blocks.Count == 0)
            {
                return;
            }
            var current = blocks.FirstBlock;
            do
            {
                var next = current?.NextBlock;

                if (current is not Section subSection)
                {
                    current = next;
                    continue;
                }

                MakeStructureVisible(subSection.Blocks);

                blocks.Remove(subSection);
                var decorated = new SectionStartEndDecorator(subSection);
                if (next is not null)
                    blocks.InsertBefore(next, decorated);
                else
                    blocks.Add(decorated);

                current = next;
            } while (current is not null);
        }

        private void MakeStructureUnvisible(BlockCollection blocks)
        {
            if (blocks.Count == 0)
            {
                return;
            }
            var current = blocks.FirstBlock;
            do
            {
                var next = current?.NextBlock;

                if (current is not SectionStartEndDecorator decoreted)
                {
                    current = next;
                    continue;
                }

                blocks.Remove(decoreted);
                var section = decoreted.DecorationTarget;

                MakeStructureUnvisible(section.Blocks);

                if (next is not null)
                    blocks.InsertBefore(next, section);
                else
                    blocks.Add(section);

                current = next;
            } while (current is not null);
        }

        private void Structure_Unvisible(object sender, RoutedEventArgs e)
        {
            var blocks = rtbEditor.Document.Blocks;
            MakeStructureUnvisible(blocks);
            StructureIsVisible = false;
        }

        private void Create_SubSection(object sender, RoutedEventArgs e)
        {
            TextSelection selection = rtbEditor.Selection;
            var currentParagraph = selection.Start.Paragraph;
            var parentSection = currentParagraph.Parent as Section;

            var nP = new Paragraph();
            var section = new Section(nP);
            var newSection = StructureIsVisible ? new SectionStartEndDecorator(section) : section;

            parentSection?.Blocks.Add(newSection);

            rtbEditor.CaretPosition = nP.ContentStart;
        }

        // Обработчик события нажатия кнопки "Новый"
        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.Document.Blocks.Clear(); // Очистка документа
        }

        // Обработчик события выхода из приложения
        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Форматирование текста
        private void Bold_Checked(object sender, RoutedEventArgs e)
        {
            MakeTextBold(true);
        }

        private void Bold_Unchecked(object sender, RoutedEventArgs e)
        {
            MakeTextBold(false);
        }

        private void Italic_Checked(object sender, RoutedEventArgs e)
        {
            MakeTextItalic(true);
        }

        private void Italic_Unchecked(object sender, RoutedEventArgs e)
        {
            MakeTextItalic(false);
        }

        private void Underline_Checked(object sender, RoutedEventArgs e)
        {
            MakeTextUnderlined(true);
        }

        private void Underline_Unchecked(object sender, RoutedEventArgs e)
        {
            MakeTextUnderlined(false);
        }

        // Методы для изменения стиля выделенного текста
        private void MakeTextBold(bool isBold)
        {
            TextSelection selection = rtbEditor.Selection;
            if (!selection.IsEmpty)
            {
                if (isBold)
                {
                    selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                }
                else
                {
                    selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                }
            }
        }

        private void MakeTextItalic(bool isItalic)
        {
            TextSelection selection = rtbEditor.Selection;
            if (!selection.IsEmpty)
            {
                if (isItalic)
                {
                    selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
                }
                else
                {
                    selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
                }
            }
        }

        private void MakeTextUnderlined(bool isUnderlined)
        {
            TextSelection selection = rtbEditor.Selection;
            if (!selection.IsEmpty)
            {
                if (isUnderlined)
                {
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
                else
                {
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
            }
        }

        private void LoadFb2(string filePath)
        {
            //var doc = FB2DocumentReader.Read(filePath);
            var text = File.ReadAllText(filePath);
            var flowDoc = new FictionBookSerializer().Deserialize(text);

            rtbEditor.Document = flowDoc;
        }

        private void SaveFb2(string filePath)
        {
            var doc = new FictionBookSerializer().Serialize(rtbEditor.Document);
            File.WriteAllText(filePath, doc);
            //(doc as XDocument).Save(filePath);
        }

        // Обработчик события нажатия кнопки "Открыть"
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog { Filter = "FB2 Files|*.fb2" };
            if (openDialog.ShowDialog() == true)
            {
                var ext = System.IO.Path.GetExtension(openDialog.FileName);
                switch (ext)
                {
                    case AllowedFilesExtensions.FB2:
                        LoadFb2(openDialog.FileName);
                        break;
                }
            }
        }

        // Обработчик события нажатия кнопки "Сохранить"
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog { Filter = "FB2 Files|*.fb2" };
            if (saveDialog.ShowDialog() == true)
            {
                SaveFb2(saveDialog.FileName);
            }
        }
    }
}