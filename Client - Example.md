# Client (React + Vite)

This is the frontend React app for DevFolio. It consumes the Express API and provides login + project dashboard.

---

## Structure
```
src/
  main.jsx       # Entrypoint with React Router
  ui/
    App.jsx      # Layout + Nav
    Login.jsx    # Auth form
    Dashboard.jsx# Project CRUD
```

---

## Features
- Login form (stores JWT in localStorage)
- Protected routes (redirects unauthenticated users)
- CRUD UI for projects
- Basic state management with React hooks

---

## UI Flow
1. User visits `/login`
2. Submits credentials → receives JWT
3. JWT stored → access to `/` (dashboard)
4. Dashboard displays projects, allows CRUD

---

## ⚙ Config
API base URL is set to `http://localhost:4000`. Update in `src/ui/*` if needed.
