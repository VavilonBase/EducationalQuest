public class ResponseUser : UserDto
{
    public string login { get; set; } // Логин пользователя

    public override string ToString()
    {
        return $"ID: {id}\n" +
            $"Имя: {firstName}\n" +
            $"Фамилия: {lastName}\n" +
            $"Отчество: {middleName}\n" +
            $"Роль: {role}\n" +
            $"Активен ли пользователь: {isActivated}\n" +
            $"Логин: {login}";
    }
}

