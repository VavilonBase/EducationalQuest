public enum Message
{
    // ОБЩИЕ
    NotError = 0, // Нет ошибки
    JWTError = 100, // Ошибка с jwt
    IncorrectTokenFormat = 101, // Токена не было вообще или был, но неправильного формата
    UnauthorizedUser = 102, // Пользователь не авторизован
    AccessDenied = 103, // Нет доступа
    NotFoundQueryParam = 104, // Не передан query параметр в url
    NotFoundRequiredData = 105, // Нет обязательных данных
    PageNotFound = 106, // Страница не найдена
    CanNotCreateFolder = 107, // Не получилось создать папку учителя
    CanNotPublishFile = 108, // Не удалось сделать файл публичным
    CanNotGetPublicUrl = 109, // Не удалось получить публичный ключ
    CanNotLoadFile = 110, // Не удалось загрузить файл
    CanNotDeleteFileOrFolder = 111, // Не удалось удалить папку
    // БД
    DBErrorExecute = 200, // Ошибка при выполнении запроса в базе данных
    // ПОЛЬЗОВАТЕЛИ
    CanNotCreateUser = 300, // Невозможно создать пользователя, возможно какое-то значение не передано, либо нет доступа к базе данных
    UserExist = 301, // Ошибка при регистрации пользователя, когда такой пользователь уже существует
    UserNotExist = 302, // Ошибка при обращении к несуществующему пользователю
    IncorrectPassword = 303, // Неверный пароль
    CanNotUpdateUser = 304, // Ошибка при обновлении пользователя
    CanNotGetUsers = 305, // Ошибка при получении пользователей
    IsNotTeacher = 306, // Пользователь не является учителем
    PassowordNotEquals = 307, // При смене пароля, пароли не совпали
    // ГРУППЫ
    CanNotCreateGroup = 400, // Не удалось создать группу
    TeacherHasNotGroups = 401, // У учителя нет групп
    GroupNotFound = 402, // Группа не найдена
    StudentIsInAGroup = 403, // Ученик уже находится в группе
    StudentIsNotInAGroup = 404, // Ученик не состоит в этой группе
    CanNotJoinStudentInTheGroup = 404, // Не удалось добавить ученика в группу
    StudentHasNotGroups = 405, // У ученика нет групп
    GroupHasNotStudents = 406, // У группы нет учеников
    UserIsNotCreatorGroup = 407, // Пользователь не является создателем группы
    CanNotUpdateGroup = 408, // Не удалось обновить группы
    CanNotDeleteGroup = 409, // Не удалось удалить группу
    // ТЕСТЫ
    CanNotCreateTest = 500, // Не удалось создать тест
    GroupHasNotTests = 501, // В группе нет тестов
    TestNotExist = 502, // Такого теста не существует
    CanNotUpdateTest = 503, // Не удалось обновить тест
    TestCanNotOpenedAndClosedTogether = 504, // Тесте не может быть одновременно открыт и закрыт
    UserIsNotCreatorTest = 505, // Пользователь не является создателем теста
    TestIsClosed = 506, // Тест закрыт
    TestIsNotOpened = 507, // Тест не открыт
    TestCanNotOpened = 508, // Не получилось открыть тест
    TestCanNotClosed = 509, // Не получилось закрыть тест
    TestHasNotQuestions = 510, // Тест не имеет вопросов
    // ВОПРОСЫ
    CanNotCreateQuestion = 600, // Не удалось создать вопрос
    NotFoundQuestion = 601, // Вопрос не найден
    CanNotUpdateQuestion = 602, // Не удалось обновить вопрос
    CanNotGetQuestion = 603, // Не удалось получить вопросы
    QuestionHasNotAnswers = 604, // Нет ответов на вопрос
    // ОТВЕТЫ
    CanNotCreateAnswer = 700, // Не удалось создать ответ
    AnswerNotFound = 701, // Ответ не найден
    CanNotUpdateAnswer = 702, // Не удалось обновить ответ
    // РЕЗУЛЬТАТЫ
    CanNotCreateResult = 800, // Не удалось создать результат
    CanNotAddTotalScores = 801, // Не удалось добавить общее количество очков в
    NotFoundResult = 802, // Результат не найден
}