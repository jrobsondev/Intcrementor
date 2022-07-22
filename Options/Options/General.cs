using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Intcrementor.Options
{
    internal partial class OptionsProvider
    {
        [ComVisible(true)]
        public class GeneralOptions : BaseOptionPage<General> { }
    }

    public class General : BaseOptionModel<General>
    {
        private int _DefaultStepValue = 1;
        [Category("General")]
        [DisplayName("Default Step Value")]
        [Description("The default value to increment/decrement all numbers by.\nMinimum length: 1\nMaximum length: int.MaxValue")]
        [DefaultValue(1)]
        public int DefaultStepValue { get => _DefaultStepValue; set { _DefaultStepValue = Validators.ValidateRange(value, 1, int.MaxValue); } }

        private int _MaxIntegerLength = 2;
        [Category("General")]
        [DisplayName("Max Integer Length")]
        [Description("Sets the max length an integer can be when searching.\nMinimum length: 1\nMaximum length: 10")]
        [DefaultValue(2)]
        public int MaxIntegerLength { get => _MaxIntegerLength; set { _MaxIntegerLength = Validators.ValidateRange(value, 1, 10); } }
    }
}
