using FileSearchApp.Model;
using FileSearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileSearchApp.ViewModel
{
    class FileSearchViewModel : ViewModelBase
    {
        public DirectorySearch DirectorySearch { get; } = new DirectorySearch();
        public FileFilter FileFilter { get; } = new FileFilter();
        public TextFilter TextFilter { get; } = new TextFilter();
        public ObservableCollection<FileMatch> Results { get; set; } = new ObservableCollection<FileMatch>();
        public FileFilterTypeVM[] FileFilterTypes { get; }
        public TextFilterTypeVM[] TextFilterTypes { get; }
        public ICommand SearchCommand { get; private set; }

        private CancellationTokenSource _cancelToken;
        private bool _isRunning;
        private bool _resultsVisibile;

        public FileSearchViewModel()
        {
            FileFilterTypes = (FileFilterTypeVM[])Enum.GetValues(typeof(FileFilterTypeVM));
            TextFilterTypes = (TextFilterTypeVM[])Enum.GetValues(typeof(TextFilterTypeVM));

            InitCommands();
        }

        /// <summary>
        /// Initiate commands bound to UI
        /// </summary>
        public void InitCommands()
        {
            SearchCommand = new RelayCommand(StartStopSearch);
        }


        public string DirectorySearchPath
        {
            get { return DirectorySearch.SearchPath; }
            set
            {
                DirectorySearch.SearchPath = value;
                ClearErrors("DirectorySearchPath");
                OnPropertyChanged("DirectorySearchPath");
            }
        }

        public bool DirectorySearchSubfolder
        {
            get { return DirectorySearch.SearchSubfolders; }
            set
            {
                DirectorySearch.SearchSubfolders = value;
                OnPropertyChanged("DirectorySearchSubfolder");
            }
        }

        public bool DirectorySearchHidden
        {
            get { return DirectorySearch.SearchHidden; }
            set
            {
                DirectorySearch.SearchHidden = value;
                OnPropertyChanged("DirectorySearchHidden");
            }
        }

        public FileFilterTypeVM FileFilterType
        {
            get { return EnumConverter.Convert<FileFilterTypeVM>(FileFilter.SearchType);  }
            set
            {
                FileFilter.SearchType = EnumConverter.Convert<FileFilterType>(value);
                OnPropertyChanged("FileFilterType");
            }
        }

        public string FileFilterValue
        {
            get { return FileFilter.Value; }
            set
            {
                FileFilter.Value = value;
                OnPropertyChanged("FileFilterValue");
            }
        }

        public TextFilterTypeVM TextFilterType
        {
            get { return EnumConverter.Convert<TextFilterTypeVM>(TextFilter.SearchType); }
            set
            {
                TextFilter.SearchType = EnumConverter.Convert<TextFilterType>(value);
                OnPropertyChanged("TextFilterType");
            }
        }

        public string TextFilterValue
        {
            get { return TextFilter.Value; }
            set
            {
                TextFilter.Value = value;
                OnPropertyChanged("TextFilterValue");
            }
        }

        public bool MatchCase
        {
            get { return TextFilter.MatchCase; }
            set
            {
                TextFilter.MatchCase = value;
                OnPropertyChanged("MatchCase");
            }
        }

        public bool MatchWord
        {
            get { return TextFilter.MatchWord; }
            set
            {
                TextFilter.MatchWord = value;
                OnPropertyChanged("MatchWord");
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
            }
        }

        public bool ResultsVisible
        {
            get { return _resultsVisibile; }
            set
            {
                _resultsVisibile = value;
                OnPropertyChanged("ResultsVisible");
            }
        }

        /// <summary>
        /// Begins or ends search depending on current running state
        /// </summary>
        private async void StartStopSearch()
        {
            if (IsRunning == false)
                await Search();
            else
                StopSearch();
        }
        
        /// <summary>
        /// Runs search based on search properties
        /// </summary>
        private async Task Search()
        {
            // run validation before we do anything
            ValidateDirectory();

            if (PropertyHasErrors("DirectorySearchPath"))
                return;

            IsRunning = true;
            ResultsVisible = true;
            _cancelToken = new CancellationTokenSource();

            Results.Clear();

            // DirectorySearch.EnumerateFiles will provide each directory (and relevant subdirectories) that should be searched
            foreach (string file in DirectorySearch.EnumerateFiles())
            {
                // check if search has been stopped
                if (_cancelToken.IsCancellationRequested)
                    break;

                // file filter matches
                if (FileFilter.Evaluate(file))
                {
                    string fileContents = await File.ReadAllTextAsync(file);

                    // evaluate text filter
                    if (TextFilter.Evaluate(fileContents))
                    {
                        var FM = new FileMatch(file);
                        Results.Add(FM);
                        await Task.Delay(1); // delay allows Results ui to refresh
                    }
                }
            }

            IsRunning = false;
        }

        private void StopSearch()
        {
            _cancelToken.Cancel();
        }

        /// <summary>
        /// run validation on the search directory
        /// </summary>
        private void ValidateDirectory()
        {
            if (string.IsNullOrEmpty(DirectorySearchPath))
            {
                RaiseError("DirectorySearchPath", "Search Folder Not Provided");
            }
            else if (Directory.Exists(DirectorySearchPath) == false)
            {
                RaiseError("DirectorySearchPath", "Search Folder Not Found");
            }
        }
    }
}
