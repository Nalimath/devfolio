# Server (Node.js + Express + MySQL)

This is the backend API for DevFolio. It provides authentication and project management endpoints.

---

## API Endpoints

### Auth
- `POST /api/auth/signup` → Register new user
- `POST /api/auth/login` → Authenticate & receive JWT

### Projects (JWT required)
- `GET /api/projects` → List user's projects
- `POST /api/projects` → Create new project
- `PUT /api/projects/:id` → Update project
- `DELETE /api/projects/:id` → Remove project

---

## Environment Variables
`.env` example:
```
PORT=4000
JWT_SECRET=dev_secret_change_me
DB_HOST=127.0.0.1
DB_PORT=3306
DB_USER=appuser
DB_PASSWORD=apppass
DB_NAME=devfolio
```

---

## Security
- Passwords hashed with bcrypt
- JWT for stateless auth
- Joi validation for inputs

---

## Database Setup
```bash
npm run db:setup
```
Creates tables + seeds a demo user (`demo@example.com` / `demo1234`).
