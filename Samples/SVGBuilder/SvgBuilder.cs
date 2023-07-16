using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Svg;

namespace SVGBuilder
{
    public partial class SvgBuilder : Form
    {
        private SvgDocument svgDoc;

        public SvgBuilder()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = @"svgDoc = new SvgDocument
{
    Width = 500,
    Height = 500,
    ViewBox = new SvgViewBox(-250, -250, 500, 500),
};

var group = new SvgGroup();
svgDoc.Children.Add(group);

group.Children.Add(new SvgCircle
{
    Radius = 100,
    Fill = new SvgColourServer(Color.Red),
    Stroke = new SvgColourServer(Color.Black),
    StrokeWidth = 2
});
";
            textBox2.SelectionStart = textBox2.Text.Length;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                svgDoc.Write(saveFileDialog1.FileName);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image?.Dispose();

            pictureBox1.Image = null;
            button1.Enabled = false;

            try
            {
                svgDoc = CreateDocument(textBox2.Text);
                if (svgDoc != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        svgDoc.Write(stream);
                        textBox1.Text = Encoding.UTF8.GetString(stream.GetBuffer());
                    }
                    pictureBox1.Image = svgDoc.Draw();
                    button1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
            }
        }

        private SvgDocument CreateDocument(string userCode)
        {
            var source = $@"using System;
using System.Drawing;
using System.IO;
using Svg;

class Program
{{
    public static SvgDocument CreateDocument()
    {{
        SvgDocument svgDoc = null;
        {userCode}
        return svgDoc;
    }}
}}
";

            var sourcePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly()?.Location ?? string.Empty);
            var sourceText = SourceText.From(source, Encoding.UTF8);

            var options = CSharpParseOptions.Default
                .WithLanguageVersion(LanguageVersion.CSharp8);

            var syntaxTree = CSharpSyntaxTree.ParseText(
                sourceText,
                options
            )
                .WithFilePath(Path.Combine(sourcePath, "Program.cs"));

            var runtimeDirectoryPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(runtimeDirectoryPath, "mscorlib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(runtimeDirectoryPath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(runtimeDirectoryPath, "System.Drawing.Primitives.dll")),
                MetadataReference.CreateFromFile(Path.Combine(runtimeDirectoryPath, "System.Private.Xml.dll")),
                MetadataReference.CreateFromFile(Path.Combine(runtimeDirectoryPath, "System.Xml.ReaderWriter.dll")),
                MetadataReference.CreateFromFile(Path.Combine(sourcePath, "Svg.dll")),
            };

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release);

            var compilation = CSharpCompilation.Create(
                "ConsoleApp",
                new[] { syntaxTree },
                references,
                compilationOptions);

            using (var stream = new MemoryStream())
            {
                var emitResult = compilation.Emit(stream);
                if (!emitResult.Success)
                {
                    var messages = new StringBuilder();
                    foreach (var diagnostic in emitResult.Diagnostics)
                    {
                        var pos = diagnostic.Location.GetLineSpan();
                        var location = $"({pos.StartLinePosition.Line + 1 - 10},{pos.StartLinePosition.Character + 1})";
                        messages.AppendLine($"{location}: {diagnostic.Severity} {diagnostic.Id}: {diagnostic.GetMessage()}");
                    }
                    throw new InvalidProgramException(messages.ToString());
                }

                var assemblyLoadContext = new System.Runtime.Loader.AssemblyLoadContext(compilation.AssemblyName, true);
                try
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var assembly = assemblyLoadContext.LoadFromStream(stream);

                    var type = assembly.GetType("Program");
                    var method = type?.GetMethod("CreateDocument");
                    return method?.Invoke(null, null) as SvgDocument;
                }
                catch (Exception ex)
                {
                    throw ex.InnerException ?? ex;
                }
                finally
                {
                    assemblyLoadContext.Unload();
                }
            }
        }
    }
}
