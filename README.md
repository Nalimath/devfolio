# DevFolio Full-Stack Example

This is a small sample, showcasing full-stack development with Node.js, React, and MySQL.  

---

## Tech Stack
- **Backend:** Node.js, Express, MySQL (mysql2), JWT, bcrypt, Joi validation
- **Frontend:** React (Vite), React Router, modern hooks
- **Database:** MySQL schema + seed data
- **DevOps:** Docker Compose (MySQL + Adminer)
- **Game Dev Samples:** Unity (C#) and Unreal Engine (C++) snippets

---

## Features
- User authentication (signup/login) with JWT
- CRUD operations on "Projects" (title, description, status)
- Protected API routes with role-ready user model
- React client with login and dashboard UI
- Seeded demo user for instant testing

---

## Quick Start

### 1. Start MySQL with Docker
```bash
docker compose up -d
# MySQL: localhost:3306 | Adminer: http://localhost:8080
```

### 2. Backend Setup
```bash
cd server
cp .env.example .env
npm install
npm run db:setup   # sets up DB + seed demo user
npm run dev        # http://localhost:4000
```

### 3. Frontend Setup
```bash
cd ../client
npm install
npm run dev        # http://localhost:5173
```

### 4. Demo Login
```
Email: demo@example.com
Password: demo1234
```

---

## Project Structure
```
devfolio-fullstack-pro/
  ├─ README.md
  ├─ docker-compose.yml
  ├─ server/         # Node.js API
  ├─ client/         # React frontend
  └─ samples/        # Unity & Unreal code
```

---

## Extend Ideas
- Add role-based authorization (admin vs user)
- Pagination and search for projects
- File uploads (e.g., project images)
- GitHub Actions CI (lint/tests)
- Deployment with Docker/Heroku

---

## Author
**Joseph Rofrano**  
joe@bzzy.dev

---

## License
MIT License — free to use and modify.
