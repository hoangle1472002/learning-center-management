# Learning Center Management System

Hệ thống quản lý trung tâm dạy học được xây dựng với ReactJS (frontend) và .NET 8 (backend API).

## Kiến trúc tổng thể

### Backend (.NET 8)
- **Clean Architecture** với 4 layers:
  - `LearningCenter.API` - Web API layer
  - `LearningCenter.Application` - Application layer (CQRS với MediatR)
  - `LearningCenter.Domain` - Domain layer (Entities, Interfaces)
  - `LearningCenter.Infrastructure` - Infrastructure layer (EF Core, Repository Pattern)

### Frontend (ReactJS + TypeScript)
- **ReactJS 18** với TypeScript
- **TailwindCSS** cho styling
- **Redux Toolkit** cho state management
- **React Query** cho API calls
- **React Router** cho routing

## Công nghệ sử dụng

### Backend
- .NET 8 Web API
- Entity Framework Core với PostgreSQL
- JWT Authentication
- MediatR (CQRS pattern)
- AutoMapper
- FluentValidation
- Serilog
- Clean Architecture
- Repository Pattern
- Unit of Work Pattern

### Frontend
- React 18 + TypeScript
- TailwindCSS
- Redux Toolkit
- React Query
- React Router
- Axios
- Radix UI components

## Cài đặt và chạy dự án

### Yêu cầu hệ thống
- .NET 8 SDK
- Node.js 18+
- PostgreSQL 12+
- Git

### Backend Setup

1. **Cài đặt PostgreSQL**
   ```bash
   # macOS với Homebrew
   brew install postgresql
   brew services start postgresql
   
   # Tạo database
   createdb LearningCenterDB
   ```

2. **Cấu hình connection string**
   - Mở file `backend/src/LearningCenter.API/appsettings.json`
   - Cập nhật connection string phù hợp với PostgreSQL của bạn

3. **Chạy backend**
   ```bash
   cd backend
   dotnet restore
   dotnet build
   dotnet run --project src/LearningCenter.API
   ```

   API sẽ chạy tại: `https://localhost:7000` hoặc `http://localhost:5000`

### Frontend Setup

1. **Cài đặt dependencies**
   ```bash
   cd frontend
   npm install
   ```

2. **Chạy frontend**
   ```bash
   npm start
   ```

   Frontend sẽ chạy tại: `http://localhost:3000`

## API Endpoints

### Authentication
- `POST /api/auth/login` - Đăng nhập
- `POST /api/auth/register` - Đăng ký
- `POST /api/auth/refresh-token` - Làm mới token
- `POST /api/auth/logout` - Đăng xuất

### Swagger Documentation
- Truy cập: `https://localhost:7000` (khi chạy backend)

## Cấu trúc thư mục

```
├── backend/
│   ├── src/
│   │   ├── LearningCenter.API/          # Web API
│   │   ├── LearningCenter.Application/   # Application layer
│   │   ├── LearningCenter.Domain/        # Domain layer
│   │   └── LearningCenter.Infrastructure/ # Infrastructure layer
│   └── tests/
│       ├── LearningCenter.UnitTests/
│       └── LearningCenter.IntegrationTests/
├── frontend/
│   ├── src/
│   │   ├── components/     # React components
│   │   ├── pages/         # Page components
│   │   ├── hooks/         # Custom hooks
│   │   ├── services/      # API services
│   │   ├── store/         # Redux store
│   │   ├── types/         # TypeScript types
│   │   └── utils/         # Utility functions
│   └── public/
└── README.md
```

## Tính năng chính

### Đã hoàn thành
- ✅ Cấu trúc dự án Clean Architecture
- ✅ Authentication & Authorization với JWT
- ✅ Entity Framework Core với PostgreSQL
- ✅ Repository Pattern & Unit of Work
- ✅ CQRS với MediatR
- ✅ API Documentation với Swagger
- ✅ Logging với Serilog
- ✅ Frontend setup với React + TypeScript + TailwindCSS

### Đang phát triển
- 🔄 Quản lý học viên
- 🔄 Quản lý giáo viên
- 🔄 Quản lý lớp học
- 🔄 Quản lý thanh toán
- 🔄 Dashboard thống kê

## Design Patterns được sử dụng

1. **Clean Architecture** - Tách biệt các layers
2. **Repository Pattern** - Abstract data access
3. **Unit of Work Pattern** - Quản lý transactions
4. **CQRS** - Tách biệt Commands và Queries
5. **Mediator Pattern** - Decouple request/response
6. **Dependency Injection** - Loose coupling
7. **Factory Pattern** - Tạo objects
8. **Strategy Pattern** - Algorithm selection

## Hướng dẫn phát triển

### Thêm tính năng mới

1. **Domain Layer**: Tạo entities và interfaces
2. **Application Layer**: Tạo DTOs, Commands, Queries, Handlers
3. **Infrastructure Layer**: Implement repositories và services
4. **API Layer**: Tạo controllers và endpoints
5. **Frontend**: Tạo components và pages

### Commit và Push

Sau khi hoàn thành mỗi tính năng nhỏ:
```bash
git add .
git commit -m "feat: add authentication system"
git push origin main
```

## Liên hệ

Nếu có thắc mắc hoặc cần hỗ trợ, vui lòng tạo issue trên GitHub repository.
