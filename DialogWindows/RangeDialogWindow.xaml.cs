using Microsoft.VisualStudio.PlatformUI;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Intcrementor.DialogWindows
{
    public partial class RangeDialogWindow : DialogWindow, INotifyPropertyChanged
    {
        private readonly List<Match> _regexMatchList;
        private readonly List<int> _integerMatchList;
        private readonly IntcrementorManager _manager;

        public event PropertyChangedEventHandler PropertyChanged;

        internal RangeDialogWindow(RangeDialogCommand rangeDialogCommand)
        {
            InitializeComponent();
            DataContext = this;
            _manager = rangeDialogCommand.Manager;
            _regexMatchList = _manager.GetIntegersInDocViewAsMatchList();
            _integerMatchList = _regexMatchList.OfType<Match>()
                .Select(x => int.Parse(x.Groups[0].Value))
                .ToList();
            InitializeData();
        }

        private int _startNTBControlValue;
        public int StartNTBControlValue
        {
            get => _startNTBControlValue;
            set
            {
                _startNTBControlValue = value;
                OnPropertyChanged();
            }
        }

        private int _startNTBControlMinValue;
        public int StartNTBControlMinValue
        {
            get => _startNTBControlMinValue;
            set
            {
                _startNTBControlMinValue = value;
                OnPropertyChanged();
            }
        }

        private int _endNTBControlValue;
        public int EndNTBControlValue
        {
            get => _endNTBControlValue;
            set
            {
                _endNTBControlValue = value;
                OnPropertyChanged();
            }
        }

        private int _endNTBControlMaxValue;
        public int EndNTBControlMaxValue
        {
            get => _endNTBControlMaxValue;
            set
            {
                _endNTBControlMaxValue = value;
                OnPropertyChanged();
            }
        }

        private int _stepNTBControlValue;
        public int StepNTBControlValue
        {
            get => _stepNTBControlValue;
            set
            {
                _stepNTBControlValue = value;
                OnPropertyChanged();
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InitializeData()
        {
            StartNTBControlValue = _integerMatchList.Min();
            StartNTBControlMinValue = _integerMatchList.Min();

            EndNTBControlValue = _integerMatchList.Max();
            EndNTBControlMaxValue = _integerMatchList.Max();

            StepNTBControlValue = _manager.Options.DefaultStepValue;
        }

        private async void GoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var matches = _regexMatchList.Where(x => int.Parse(x.Value) >= StartNTBControlValue && int.Parse(x.Value) <= EndNTBControlValue).ToList();
            await _manager.GetSelectionsAndAdjustAsync(matches, () => _manager.SelectionBroker.PerformActionOnAllSelections(x => _manager.AdjustSelection(x.Selection, StepNTBControlValue)), $"Incrementing numbers from {StartNTBControlValue} to {EndNTBControlValue} by {StepNTBControlValue}");
        }
    }
}
