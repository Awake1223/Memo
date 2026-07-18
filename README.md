# 📝 Memo - Short Notes with Public Links

> Короткие заметки с публичными ссылками.

## 🏗 Architecture

- **Clean Architecture**
- **Backend**: .NET 8 (API + Application + Domain + Infrastructure)
- **Frontend**: Angular 17
- **Mobile**: Uno Platform (iOS, Android)


📝 Memo - Mini-Pastebin
Сервис коротких заметок с публичными ссылками. Полноценное fullstack-приложение на ASP.NET Core + Angular.


Эндпоинты API
POST	/api/Auth/register	Регистрация	
POST	/api/Auth/login	Вход → JWT	
POST	/api/Notes	Создать заметку	
GET	/api/Notes/{shortCode}	Просмотр заметки	
GET	/api/Notes/my	Список своих заметок	
PUT	/api/Notes/{shortCode}	Редактировать заметку	
DELETE	/api/Notes/{shortCode}	Удалить заметку	
GET	/api/Search	Поиск по заметкам	
GET	/api/Tags/popular	Популярные теги	
🛠️ Стек технологий
Backend
.NET 9 + ASP.NET Core Web API

Entity Framework Core (ORM)

PostgreSQL (база данных)

JWT (авторизация)

BCrypt (хеширование паролей)

Swagger/OpenAPI (документация API)

Frontend
Angular 20

Bootstrap 5 (стили)

RxJS (асинхронность)

ngx-markdown (поддержка Markdown)

Инфраструктура
Git (контроль версий)

Docker (контейнеризация)

Render (хостинг бэкенда)

Netlify (хостинг фронтенда)

✅ Создание заметки (текст, время жизни: 1 час, 1 день, навсегда)

✅ Генерация короткой уникальной ссылки (6 символов, a-zA-Z0-9)

✅ Просмотр заметки по ссылке без авторизации

✅ Автоматическое удаление просроченных заметок (фоновый сервис)

✅ Список своих заметок для авторизованного пользователя

✅ Упрощенная регистрация по email/паролю

Дополнительные фичи
✅ Редактирование заметок (CRUD Update)

✅ Сожжение после прочтения (Self-destruct)

✅ Поддержка Markdown

✅ Счётчик просмотров

✅ Поиск по своим заметкам

✅ Автоматические теги (#хэштеги)

✅ Облако популярных тегов

✅ Пагинация (для списка заметок)

🚀 Запуск проекта
Требования
.NET 9 SDK

PostgreSQL 15+

Node.js 20+

Angular CLI (npm install -g @angular/cli)

1. Клонирование
bash
git clone https://github.com/Awake1223/Memo.git
cd Memo
2. Настройка базы данных
Создай базу данных PostgreSQL:

sql
CREATE DATABASE MiniPastebin;
3. Настройка бэкенда
Создай Memo.API/appsettings.Development.json:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=MiniPastebin;Username=postgres;Password=твой_пароль"
  },
  "Jwt": {
    "Key": "a-string-secret-at-least-256-bits-long",
    "Issuer": "MemoAPI"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
4. Запуск бэкенда
bash
cd Memo.Infrastructure
dotnet ef database update

cd ../Memo.API
dotnet run
Бэкенд запустится на https://localhost:7117
Swagger: https://localhost:7117/swagger

5. Запуск фронтенда
bash
cd Memo.Angular
npm install
ng serve --open
Фронтенд запустится на http://localhost:4200



📄 Лицензия
MIT © Awake1223
