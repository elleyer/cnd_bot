using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CndBot.Core.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CndBot.Core.Actions
{
    public class RegisterFormAction
    {
        public const string REG_DANCE_FORM = "Хореографія";
        public const string REG_ART_FORM = "Малювання";
        public const string REG_SIGN_FORM = "Вокал";

        public long UserId;

        private RegisterFormStage _stage;

        private ITelegramBotClient _botClient;

        public RegisterFormAction(ITelegramBotClient botClient, long userId)
        {
            _botClient = botClient;
            _stage = RegisterFormStage.None;
            UserId = userId;
        }

        //Task<FormDataModel> ???
        public async Task ProcessNextStage(FormDataModel dataModel, Update update)
        {
            if (update.Message != null)
            {
                var chat = update.Message.Chat;
                
                switch (_stage)
                {
                    case RegisterFormStage.Init:
                        var markup = new ReplyKeyboardMarkup(new List<KeyboardButton>
                        {
                            new KeyboardButton(REG_DANCE_FORM),
                            new KeyboardButton(REG_SIGN_FORM),
                            new KeyboardButton(REG_ART_FORM)
                        });
                        markup.ResizeKeyboard = true;
                        
                        await _botClient.SendTextMessageAsync(chat, "Куди саме Ви плануєте записатись?",
                            replyMarkup: markup);
                        break;
                    case RegisterFormStage.Requested:
                        SetFormType(ref dataModel, update.Message.Text);
                        await _botClient.SendTextMessageAsync(chat, "Як Вас звати?", 
                            replyMarkup: new ReplyKeyboardRemove());
                        break;
                    case RegisterFormStage.Name:
                        SetUsername(ref dataModel, update.Message.Text);;
                        await _botClient.SendTextMessageAsync(chat, "Скільки Вам років?");
                        break;
                    case RegisterFormStage.Age:
                        SetAge(ref dataModel, int.Parse(update.Message.Text ?? string.Empty));
                        await _botClient.SendTextMessageAsync(chat, "Будь ласка, опишіть себе. Чи займались ви" +
                                                                    "цим раніше?");
                        break;
                    case RegisterFormStage.Description:
                        SetDescription(ref dataModel, update.Message.Text);;
                        await _botClient.SendTextMessageAsync(chat, "Дякуємо, Вашу анкету прийнято та відправлено" +
                                                                    " на обробку!");
                        await SaveModel(dataModel);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                _stage = (RegisterFormStage) ((int) _stage << 1);
            }
        }

        public async Task InitForm(FormDataModel dataModel, Update update)
        {
            if (update.Message?.From == null) 
                return;
            
            dataModel.Username = $"@{update.Message.From.Username}";
            dataModel.UserId = update.Message.From.Id;

            _stage = RegisterFormStage.Init;
            await ProcessNextStage(dataModel, update);
        }

        public void SetFormType(ref FormDataModel dataModel, string type)
        {
            dataModel.FormType = type switch
            {
                REG_DANCE_FORM => FormType.Dancing,
                REG_ART_FORM => FormType.Art,
                REG_SIGN_FORM => FormType.Signing,
                _ => throw new Exception("Invalid type")
            };
        }

        public void SetAge(ref FormDataModel dataModel, int age)
        {
            dataModel.Age = age;
        }

        public void SetUsername(ref FormDataModel dataModel, string username)
        {
            dataModel.Username = username;
        }
        
        public void SetDescription(ref FormDataModel dataModel, string description)
        {
            dataModel.Description = description;
        }

        public async Task SaveModel(FormDataModel dataModel)
        {
            await Client.DataBaseProvider.FormDataModels.AddAsync(dataModel);
            await Client.DataBaseProvider.SaveChangesAsync();
        }
    }

    public enum FormType
    {
        Art = 0,
        Dancing = 1,
        Signing = 2
    }
    
    [Flags]
    public enum RegisterFormStage
    {
        None = 0,
        Init = 1,
        Requested = 2,
        Name = 4,
        Age = 8,
        Description = 16
    }
}