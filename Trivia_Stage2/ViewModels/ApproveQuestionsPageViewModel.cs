﻿using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trivia_Stage2.Models;
using Trivia_Stage2.Services;



namespace Trivia_Stage2.ViewModels
{
    public class ApproveQuestionsPageViewModel:ViewModel
    {
        private Service service;
        private ObservableCollection<Question> pendingQuestions;
        private List<Question> pendingQuestionKeeper;
        public ObservableCollection<Question> PendingQuestions { get => pendingQuestions; set { pendingQuestions = value; OnPropertyChanged(); } }
        private Question selectedPendingQuestion;
        public Question SelectedPendingQuestion { get => selectedPendingQuestion; set { selectedPendingQuestion = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }
         private string filterEntry;
        public string FilterEntry { get => filterEntry; set{ filterEntry = value; OnPropertyChanged(); } }
       



        public ICommand DeclineQuestionCommand { get; private set; }
        public ICommand ApproveQuestionCommand { get; private set; }
        public ICommand FilterBySubjectCommand { get; private set; }
        public ICommand LoadPendingQuestionsCommand { get; private set; }
        public ApproveQuestionsPageViewModel(Service service_)
        {
            service = service_;
            pendingQuestionKeeper = service.GetPendingQuestions();
            PendingQuestions = new ObservableCollection<Question>(service.GetPendingQuestions());
            DeclineQuestionCommand = new Command(async (Object obj) => { await DeclineQuestion(obj); });
            ApproveQuestionCommand = new Command(async (Object obj) => await ApproveQuestion(obj));
            FilterBySubjectCommand = new Command<string>((x) => FilterSubject(x));
            LoadPendingQuestionsCommand = new Command(async () => { await LoadPendingQuestions();});
        }

        private async Task ApproveQuestion(Object obj)
        {
            bool approve = await AppShell.Current.DisplayAlert("Question", "Would you like to approve the question?", "Yes", "Cancel");
            if (approve == true)
            {
                PendingQuestions.Remove(((Question)obj));
                service.ApproveQuestion(((Question)obj));
            }
        }
        
        private async Task DeclineQuestion(Object obj)
        {
            bool approve = await AppShell.Current.DisplayAlert("Question", "Would you like to decline the question?", "Yes", "Cancel");
            if (approve == true)
            {
                PendingQuestions.Remove(((Question)obj));
                service.DeclineQuestion(((Question)obj));
            }
        }

        private void FilterSubject(string filter)
        {
            
            List<Question> filteredQ = service.GetPendingQuestionsBySubjectName(filter);
            PendingQuestions.Clear();
            foreach (Question q in filteredQ)
            {
                PendingQuestions.Add(q);
            }
        }

        private async Task LoadPendingQuestions()
        {
            IsRefreshing = true;
            FilterEntry = string.Empty;
            List<Question> filteredQ = service.GetPendingQuestionsBySubjectName(FilterEntry);
            PendingQuestions.Clear();
            foreach (Question q in filteredQ)
            {
                PendingQuestions.Add(q);
            }
            IsRefreshing = false;
        }
    }
}
