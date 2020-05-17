using System;
using System.Collections.Generic;
using AnimalCrossing.ViewModels;
using AnimalCrossing.Models;

using Windows.UI.Xaml.Controls;
using System.Reactive.Linq;
using Windows.Foundation;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;

namespace AnimalCrossing.Views
{
    public sealed partial class LittleAnimalPage : Page
    {
        private LittleAnimalViewModel ViewModel
        {
            get { return ViewModelLocator.Current.LittleAnimalViewModel; }
        }

        public LittleAnimalPage()
        {
            InitializeComponent();
            Unloaded += LittleAnimalPage_Unloaded;
            var changed =
                Observable.FromEventPattern<TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>, AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>(
                handler => SearchAutoSuggestBox.TextChanged += handler,
                handler => SearchAutoSuggestBox.TextChanged -= handler);
            var input =
                changed.DistinctUntilChanged(temp => temp.Sender.Text).
                Throttle(TimeSpan.FromSeconds(1));
            var notUserInput =
                input.ObserveOnDispatcher().
                Where(temp => temp.EventArgs.Reason != AutoSuggestionBoxTextChangeReason.UserInput).
                Select(temp => new List<LittleAnimal>());
            var userInput =
                input.ObserveOnDispatcher().
                Where(temp => temp.EventArgs.Reason == AutoSuggestionBoxTextChangeReason.UserInput).
                Where(temp => !Helpers.StringHelper.IsNullOrEmptyOrWhiteSpace(temp.Sender.Text)).
                Select(temp => GetSuggestion(temp.Sender.Text));
            var merge = Observable.Merge(notUserInput, userInput);

            merge.ObserveOnDispatcher().Subscribe(suggestions =>
            {
                SearchAutoSuggestBox.ItemsSource = suggestions;
            });
        }

        private void LittleAnimalPage_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.DetailVisibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(MainGridView, detailImageControl);
            ViewModel.LoadData();
        }

        private void SearchAutoBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var items = MainGridView.ItemsSource as List<LittleAnimal>;
            if (Helpers.StringHelper.IsNullOrEmptyOrWhiteSpace(args.QueryText))
            {
                var notify = new Helpers.NotifyPopup("请输入搜索内容");
                notify.Show();
                return;
            }
            var result = items.Find(p => p.Name == args.QueryText);
            if (result != null)
            {
                //MainGridView.ItemsSource = result;
                MainGridView.ScrollIntoView(result);
                MainGridView.SelectedItem = result;
            }
            else
            {
                var notify = new Helpers.NotifyPopup("没有找到");
                notify.Show();
            }
        }

        private List<LittleAnimal> GetSuggestion(string qtext)
        {
            //Debug.WriteLine($"Get {qtext}");
            var items = MainGridView.ItemsSource as List<LittleAnimal>;
            var result = items.FindAll(p => p.Name.Contains(qtext));
            return result;
        }

        private void SearchAutoBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SearchAutoSuggestBox.Text = (args.SelectedItem as LittleAnimal).Name;
        }
    }
}
