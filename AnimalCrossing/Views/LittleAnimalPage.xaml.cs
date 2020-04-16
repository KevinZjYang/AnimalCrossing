using System;
using System.Collections.Generic;
using AnimalCrossing.ViewModels;
using AnimalCrossing.Models;

using Windows.UI.Xaml.Controls;

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

        private void SearchAutoBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var items = MainGridView.ItemsSource as List<LittleAnimal>;
                var box = sender as AutoSuggestBox;
                var result = items.FindAll(p => p.Name.Contains(box.Text));
                box.ItemsSource = result;
            }
        }

        private void SearchAutoBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SearchAutoBox.Text = (args.SelectedItem as LittleAnimal).Name;
        }
    }
}
