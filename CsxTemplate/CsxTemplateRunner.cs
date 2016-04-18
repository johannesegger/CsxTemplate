using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj80;

namespace CsxTemplate
{
    [ComVisible(true)]
    [Guid("F2017AC4-71D7-406B-921F-F45847AB26C5")]
    [ProvideObject(typeof(CsxTemplateRunner))]
    [CodeGeneratorRegistration(typeof(CsxTemplateRunner), nameof(CsxTemplateRunner), vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    public class CsxTemplateRunner : IVsSingleFileGenerator
    {
        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = ".cs";
            return VSConstants.S_OK;
        }

        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace,
            IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            if (bstrInputFileContents == null)
                throw new ArgumentException(nameof(bstrInputFileContents));

            var content = GetOutput(wszInputFilePath, bstrInputFileContents, wszDefaultNamespace, pGenerateProgress).Result;

            var bytes = Encoding.UTF8.GetBytes(content);

            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(bytes.Length);
            Marshal.Copy(bytes, 0, rgbOutputFileContents[0], bytes.Length);
            pcbOutput = (uint)bytes.Length;

            return VSConstants.S_OK;
        }

        private static async Task<string> GetOutput(string inputFilePath, string inputFileContent, string defaultNamespace, IVsGeneratorProgress progress)
        {
            var scriptOptions = ScriptOptions.Default.WithFilePath(inputFilePath);
            // TODO globals don't work in our case. See https://github.com/dotnet/roslyn/issues/6101
            var script = CSharpScript.Create<string>($@"var Namespace = ""{defaultNamespace}"";")
                .ContinueWith(inputFileContent, scriptOptions);
            var diagnostics = script.Compile();
            progress.YieldDiagnostics(diagnostics);
            if (!diagnostics.IsEmpty)
            {
                return string.Join(Environment.NewLine, diagnostics.Select(d => $"// {d}"));
            }
            var state = await script.RunAsync();
            return state.ReturnValue.ToString();
        }
    }

    public class ScriptGlobals
    {
        public string Namespace { get; }

        public ScriptGlobals(string ns)
        {
            Namespace = ns;
        }
    }
}
