namespace MiniHttpServer.Services.Authentication
{
    /// <summary>
    /// Указывает, что выполнение метода контроллера
    /// разрешено только аутентифицированному пользователю.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AuthorizeAttribute : Attribute
    {
    }
}