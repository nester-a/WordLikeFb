using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;

namespace WordLikeFb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            var doc = FB2DocumentReader.Read(filePath);

            FlowDocument flowDoc = new FlowDocument();

            var bodyElemName = XName.Get("body", "http://www.gribuser.ru/xml/fictionbook/2.0");
            var sectionElemName = XName.Get("section", "http://www.gribuser.ru/xml/fictionbook/2.0");
            var pElemName = XName.Get("p", "http://www.gribuser.ru/xml/fictionbook/2.0");

            foreach (XElement bodyElem in doc.Root.Elements(bodyElemName))
            {
                foreach (XElement sectionElem in bodyElem.Elements(sectionElemName))
                {
                    var section = new Section();

                    foreach(XElement pElem in sectionElem.Elements(pElemName))
                    {
                        var p = new Paragraph();

                        var nodes = pElem.Nodes();
                        foreach (var node in nodes)
                        {
                            var run = new Run();

                            if (node is XText text)
                            {
                                var value = Regex.Replace(text.Value, @"\s+", " ");
                                run.Text = value;
                            }
                            else if(node is XElement element)
                            {
                                switch (element.Name.LocalName)
                                {
                                    case "emphasis":
                                        run.FontStyle = FontStyles.Italic; 
                                        break;
                                    case "strong":
                                        run.FontWeight = FontWeights.Bold;
                                        break;
                                }
                                var value = Regex.Replace(element.Value, @"\s+", " ");
                                run.Text = value;
                            }
                            p.Inlines.Add(run);
                        }

                        section.Blocks.Add(p);
                    }

                    flowDoc.Blocks.Add(section);
                }
            }
            rtbEditor.Document = flowDoc;
        }

        private void LoadXml(string filePath)
        {
            try
            {
                XDocument doc = new XDocument(filePath);
                FlowDocument flowDoc = new FlowDocument();

                foreach (XElement paragraph in doc.Root.Elements("paragraph"))
                {
                    Paragraph p = new Paragraph();
                    p.FontSize = double.Parse(paragraph.Attribute("fontSize").Value);
                    p.FontFamily = new FontFamily(paragraph.Attribute("fontFamily").Value);
                    p.TextAlignment = (TextAlignment)Enum.Parse(typeof(TextAlignment), paragraph.Attribute("textAlignment").Value);

                    foreach (XElement run in paragraph.Elements("run"))
                    {
                        Run r = new Run(run.Value);
                        r.FontWeight = run.Attribute("fontWeight")?.Value switch
                        {
                            "Bold" => FontWeights.Bold,
                            _ => FontWeights.Normal
                        };
                        r.FontStyle = run.Attribute("fontStyle")?.Value switch
                        {
                            "Italic" => FontStyles.Italic,
                            _ => FontStyles.Normal
                        };
                        p.Inlines.Add(r);
                    }

                    flowDoc.Blocks.Add(p);
                }

                rtbEditor.Document = flowDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
            }
        }

        private void SaveFb2(string filePath)
        {

        }
        private void SaveXml(string filePath)
        {
            try
            {
                XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                XElement root = new XElement("document");
                doc.Add(root);

                foreach (Block block in rtbEditor.Document.Blocks)
                {
                    Paragraph paragraph = block as Paragraph;
                    if (paragraph != null)
                    {
                        XElement paraElem = new XElement("paragraph",
                            new XAttribute("fontSize", paragraph.FontSize),
                            new XAttribute("fontFamily", paragraph.FontFamily.Source),
                            new XAttribute("textAlignment", paragraph.TextAlignment));

                        foreach (Inline inline in paragraph.Inlines)
                        {
                            Run run = inline as Run;
                            if (run != null)
                            {
                                XElement runElem = new XElement("run",
                                    new XAttribute("fontWeight", run.FontWeight),
                                    new XAttribute("fontStyle", run.FontStyle),
                                    run.Text);
                                paraElem.Add(runElem);
                            }
                        }

                        root.Add(paraElem);
                    }
                }

                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        // Обработчик события нажатия кнопки "Открыть"
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog { Filter = "XML Files|*.xml|FB2 Files|*.fb2" };
            if (openDialog.ShowDialog() == true)
            {
                var ext = System.IO.Path.GetExtension(openDialog.FileName);
                switch (ext)
                {
                    case AllowedFilesExtensions.XML:
                        LoadXml(openDialog.FileName);
                        break;
                    case AllowedFilesExtensions.FB2:
                        LoadFb2(openDialog.FileName);
                        break;
                }
            }
        }

        // Обработчик события нажатия кнопки "Сохранить"
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog { Filter = "XML Files|*.xml" };
            if (saveDialog.ShowDialog() == true)
            {
                SaveXml(saveDialog.FileName);
            }
        }
    }
}