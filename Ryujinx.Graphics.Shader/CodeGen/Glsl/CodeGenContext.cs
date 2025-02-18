using Ryujinx.Graphics.Shader.StructuredIr;
using Ryujinx.Graphics.Shader.Translation;
using System.Text;

namespace Ryujinx.Graphics.Shader.CodeGen.Glsl
{
    class CodeGenContext
    {
        public const string Tab = "    ";

        public StructuredFunction CurrentFunction { get; set; }

        public ShaderConfig Config { get; }

        public OperandManager OperandManager { get; }

        private readonly StructuredProgramInfo _info;

        private readonly StringBuilder _sb;

        private int _level;

        private string _indentation;

        public CodeGenContext(StructuredProgramInfo info, ShaderConfig config)
        {
            _info = info;
            Config = config;

            OperandManager = new OperandManager();

            _sb = new StringBuilder();
        }

        public void AppendLine()
        {
            _sb.AppendLine();
        }

        public void AppendLine(string str)
        {
            _sb.AppendLine(_indentation + str);
        }

        public string GetCode()
        {
            return _sb.ToString();
        }

        public void EnterScope()
        {
            AppendLine("{");

            _level++;

            UpdateIndentation();
        }

        public void LeaveScope(string suffix = "")
        {
            if (_level == 0)
            {
                return;
            }

            _level--;

            UpdateIndentation();

            AppendLine("}" + suffix);
        }

        public StructuredFunction GetFunction(int id)
        {
            return _info.Functions[id];
        }

        public TransformFeedbackOutput GetTransformFeedbackOutput(int location, int component)
        {
            int index = (AttributeConsts.UserAttributeBase / 4) + location * 4 + component;
            return _info.TransformFeedbackOutputs[index];
        }

        public TransformFeedbackOutput GetTransformFeedbackOutput(int location)
        {
            int index = location / 4;
            return _info.TransformFeedbackOutputs[index];
        }

        private void UpdateIndentation()
        {
            _indentation = GetIndentation(_level);
        }

        private static string GetIndentation(int level)
        {
            string indentation = string.Empty;

            for (int index = 0; index < level; index++)
            {
                indentation += Tab;
            }

            return indentation;
        }
    }
}