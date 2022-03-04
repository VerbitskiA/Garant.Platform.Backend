namespace Garant.Platform.Messaging.Consts
{
    /// <summary>
    /// Класс констант для уведомлений.
    /// </summary>
    public static class NotifyMessage
    {
        public const string NOTIFY_EMPTY_USER_INFO = "Заявка не может быть создана. Не заполнены данные о себе. Перейдите в профиль для их заполнения.";

        public const string REQUEST_MODERATION = "Ваша заявка успешно отправлена на модерацию.";

        public const string REQUEST_REVIEW = "Заявка на рассмотрении";
        
        public const string REQUEST_REVIEW_DETAIL = "Ваша заявка находится на рассмотрении. В случае изменения ее статуса вы получите оповещение.";

        public const string CARD_MODERATION_TEXT = "Карточка успешно создана и отправлена на модерацию.";

        public const string CARD_MODERATION_TITLE = "Создание новой карточки";

        public const string ERROR_EMPTY_SPHERE_CATEGORY = "Не заполнены обязательные поля для создания сферы или категории.";
    }
}