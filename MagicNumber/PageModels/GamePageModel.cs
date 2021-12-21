﻿using MagicNumber.Pages;
using MagicNumber.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MagicNumber.PageModels
{
    public class GamePageModel : PageModelBase
    {
        private readonly IMessageService _messageService;
        public int NumberToSearch { get; set; }
        private Nullable<int> _numberEnter;
        public Nullable<int> NumberEnter
        {
            get { return _numberEnter; }
            set
            {
                SetProperty(ref _numberEnter, value);
                (VerifyAnswerCommand as Command).ChangeCanExecute();
            }
        }

        public GamePageModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            NumberToSearch = new Random().Next(1, 99);
            VerifyAnswerCommand = new Command(execute: async () =>
            {
                if (NumberEnter == null)
                {
                    await _messageService.ShowMessageAsync("Le nombre recherché est plus grand.");
                }
                if (NumberToSearch == NumberEnter)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new WinPage(NumberToSearch), true);
                }
                else if (NumberToSearch > NumberEnter)
                {
                    await _messageService.ShowMessageAsync("Le nombre recherché est plus grand.");
                    NumberEnter = null;
                }
                else if (NumberEnter > NumberToSearch)
                {
                    await _messageService.ShowMessageAsync("Le nombre recherché est plus petit.");
                    NumberEnter = null;
                }
            },
            canExecute: () =>
            {
                return NumberEnter == null || NumberEnter == 0 ? false : true;
            });
        }

        public ICommand VerifyAnswerCommand { get; }
    }
}
