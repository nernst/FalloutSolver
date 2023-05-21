using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutSolver.ViewModels
{
    public partial class SolverViewModel : ObservableObject
    {
        public ObservableCollection<EntryViewModel> Entries { get; init; } = new ObservableCollection<EntryViewModel>();
        public ObservableCollection<int?> Counts { get; init; } = new ObservableCollection<int?>() { null, 0 };

        private bool _CanSolve = false;
        public bool CanSolve
        {
            get => _CanSolve;
            set => SetProperty(ref _CanSolve, value);
        }

        [ObservableProperty]
        int? _CharacterCount = null;

        public IRelayCommand SolveCommand { get; }
        public IRelayCommand AddEntryCommand { get; }
        public IRelayCommand<EntryViewModel> DeleteEntryCommand { get; }

        public SolverViewModel()
        {
            SolveCommand = new RelayCommand(Solve);
            AddEntryCommand = new RelayCommand(AddEntry);
            DeleteEntryCommand = new RelayCommand<EntryViewModel>(DeleteEntry);
            Entries.CollectionChanged += EntriesChanged;

            this.PropertyChanged += (o, a) =>
            {
                if (string.IsNullOrWhiteSpace(a.PropertyName))
                    return;

                switch(a.PropertyName)
                {
                    case nameof(CharacterCount):
                        SetCounts();
                        break;

                    default:
                        break;
                }

            };
        }

        bool CheckErrors()
        {
            var lengthFrequency = new Dictionary<int, int>();
            // First, find the frequency of each length
            foreach (var e in Entries)
            {
                int count;
                if (lengthFrequency.TryGetValue(e.Entry.Length, out count))
                    ++count;
                else
                    count = 1;

                lengthFrequency[e.Entry.Length] = count;
            }

            // If there's only one length, all the lengths are the same,
            // nothing is in error
            if (lengthFrequency.Count == 1)
            {
                foreach (var e in Entries)
                    e.HasError = false;
            }

            // Mark the entries that are not the most common length in error
            // If there's two most common lengths, arbitrary most common length is selected.
            var mostCommon = lengthFrequency.Select(kv => kv.Value).Max();
            var mostCommonLength = lengthFrequency.Where(kv => kv.Value == mostCommon).First().Key;
            foreach(var e in Entries)
            {
                e.HasError = e.Entry.Length != mostCommonLength;
            }

            return Entries.Any(e => e.HasError);
        }

        void SetCounts()
        {
            if (CharacterCount == null || CharacterCount <= 0)
            {
                while (Counts.Last() > 0)
                {
                    Counts.RemoveAt(Counts.Count - 1);
                }
            }
            else
            {
                while (Counts.Last() > CharacterCount)
                    Counts.RemoveAt(Counts.Count - 1);

                while (Counts.Last() < CharacterCount)
                    Counts.Add(Counts.Last() + 1);
            }
        }

        void EntryChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (sender is null)
                return;

            if (sender is EntryViewModel e)
            {
                if (e.Count is not null)
                    e.IsCandidate = null;

                if (CheckErrors())
                    return;

                CalculateCanSolve();
                SetCharacterCount();
            }
        }

        void EntriesChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems is not null)
            {
                foreach (var obj in args.OldItems)
                {
                    if (obj is EntryViewModel e)
                    {
                        e.PropertyChanged -= EntryChanged;
                    }
                }
            }
            if (args.NewItems is not null)
            {
                foreach (var obj in args.NewItems)
                {
                    if (obj is EntryViewModel e)
                    {
                        e.PropertyChanged += EntryChanged;
                    }
                }
            }

            if (CheckErrors())
                return;

            CalculateCanSolve();
            SetCharacterCount();
        }

        private void SetCharacterCount()
        {
            if (Entries.Count == 0)
                CharacterCount = null;
            else
                CharacterCount = Entries.Select(e => e.Entry.Length).Max();
        }

        void CalculateCanSolve()
        {
            if (Entries.Count == 0)
                goto cannot_solve;

            if (!Entries.Any(e => e.Count is not null))
                goto cannot_solve;

            var length = Entries[0].Entry.Length;
            if (Entries.Skip(1).Any(e => e.Entry.Length != length))
                goto cannot_solve;

            CanSolve = true;
            return;


        cannot_solve:
            CanSolve = false;
            return;
        }

        static int CommonCharacters(string left, string right) => Enumerable.Zip(left, right).Where(pair => pair.First == pair.Second).Count();

        void Solve()
        {
            var haveCounts = Entries.Where(e => e.Count is not null).ToList();
            var candidates = Entries.Where(e => e.Count is null);

            foreach(var candidate in candidates)
            {
                candidate.IsCandidate = haveCounts.All(e => CommonCharacters(e.Entry, candidate.Entry) == e.Count);
            }

        }

        void AddEntry()
        {
            Entries.Add(new EntryViewModel());
        }

        void DeleteEntry(EntryViewModel? entry)
        {
            throw new NotImplementedException();
        }
    }
}
