# Vibely — Real-Time Social Platform with CI/CD

A full-stack real-time social application built with **.NET 9** and **Angular 18**, featuring live messaging via **SignalR**, containerized with **Docker**, orchestrated using **Kubernetes**, and deployed automatically through a **GitHub Actions** CI/CD pipeline.

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | .NET 9 — Web API |
| **Frontend** | Angular 18 |
| **Real-Time** | SignalR — WebSocket Communication |
| **Database** | SQL Server with Entity Framework Core |
| **Containerization** | Docker |
| **Orchestration** | Kubernetes (Load Balancing & Scaling) |
| **CI/CD** | GitHub Actions |
| **Testing** | xUnit + Moq — Unit Testing |

---

## ✨ Features

- 💬 **Real-Time Messaging** — Instant communication between users powered by SignalR
- 👥 **User Presence** — Live online/offline status tracking
- 🔔 **Live Notifications** — Real-time event broadcasting
- 📡 **WebSocket Fallback** — Graceful degradation with Long Polling
- ⚖️ **Load Balancing** — Kubernetes distributes traffic across multiple replicas
- 🔄 **Auto-Scaling** — Kubernetes HPA scales pods based on load

---

## 🔄 CI/CD Pipeline

### Continuous Integration (CI)
Every push or pull request to `main` triggers:

- **Build** — Restores dependencies and builds backend + frontend in production mode
- **Code Formatting** — Verifies backend formatting with `dotnet format` and frontend with ESLint
- **Unit Tests** — Runs unit tests using xUnit and Moq
- **Integration Tests** — Runs integration tests against a real SQL Server instance in Docker
- **Docker Build & Push** — Builds Docker images for both API and Angular app and pushes them to GitHub Container Registry (`ghcr.io`)

### Continuous Deployment (CD)
After a successful CI run:

- **Database Migrations** — Automatically applies EF Core migrations
- **Deploy to Kubernetes** — Updates the Kubernetes Deployment with the latest Docker image via `kubectl set image`
- **Health Check** — Verifies rollout success with `kubectl rollout status`

---

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js 20+ & Angular CLI 18
- Docker & Docker Compose
- Kubernetes cluster (or Minikube for local)
- SQL Server (local or Docker)

### Run Locally with Docker Compose

```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/Vibely.git
cd Vibely

# Start all services (API + Angular + SQL Server)
docker-compose up --build
```

- **API** → `https://localhost:5001`
- **Angular App** → `http://localhost:4200`
- **SignalR Hub** → `https://localhost:5001/hubs/chat`

### Run Without Docker

```bash
# --- Backend ---
cd Vibely.API

# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update --project Vibely.API

# Run the API
dotnet run --project Vibely.API

# --- Frontend (new terminal) ---
cd Vibely.Client
npm install
ng serve
```

---

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test Vibely.Tests

# Run integration tests only
dotnet test Vibely.IntegrationTests
```

> **Note:** Integration tests require a running SQL Server instance. The connection string is read from the `SQL_PASSWORD` environment variable.

---

## ☸️ Kubernetes Deployment

```bash
# Apply all manifests
kubectl apply -f k8s/

# Check deployments
kubectl get deployments

# Check pods and load balancer
kubectl get pods
kubectl get services

# View rollout status
kubectl rollout status deployment/vibely-api
kubectl rollout status deployment/vibely-client
```

### Kubernetes Architecture
```bash

                    ┌─────────────────────────┐
                    │   LoadBalancer Service   │
                    └────────────┬────────────┘
                                 │
          ┌──────────────────────┼──────────────────────┐
          ▼                      ▼                       ▼
  ┌──────────────┐      ┌──────────────┐       ┌──────────────┐
  │  API Pod #1  │      │  API Pod #2  │       │  API Pod #3  │
  │  (.NET 9)    │      │  (.NET 9)    │       │  (.NET 9)    │
  └──────┬───────┘      └──────┬───────┘       └──────┬───────┘
         │                     │                       │
         └─────────────────────┴───────────────────────┘
                               │
                      ┌────────▼────────┐
                      │   SQL Server    │
                      │  (StatefulSet)  │
                      └─────────────────┘
```

## 📁 Project Structure
```bash
 Vibely/
├── Vibely.API/                        # .NET 9 Web API
│   ├── Hubs/                          # SignalR Hubs
│   │   └── ChatHub.cs
│   ├── Controllers/
│   ├── Models/
│   └── Program.cs
├── Vibely.Client/                     # Angular 18 App
│   ├── src/
│   │   ├── app/
│   │   │   ├── services/
│   │   │   │   └── signalr.service.ts # SignalR client service
│   │   │   └── components/
│   │   └── environments/
│   └── angular.json
├── Vibely.Tests/                      # Unit tests
├── Vibely.IntegrationTests/           # Integration tests
├── k8s/                               # Kubernetes manifests
│   ├── api-deployment.yaml
│   ├── client-deployment.yaml
│   ├── services.yaml
│   └── hpa.yaml                       # Horizontal Pod Autoscaler
├── .github/
│   └── workflows/
│       └── ci-cd.yml                  # CI/CD pipeline
├── docker-compose.yml
├── Dockerfile.api                     # API Docker image
└── Dockerfile.client                  # Angular Docker image
```

## 🌐 Live Demo

> The application is live and deployed on Kubernetes:

**App** → `https://vibely.yourdomain.com`
**API Swagger** → `https://api.vibely.yourdomain.com/swagger`
<img width="500" height="450" alt="image" src="https://github.com/user-attachments/assets/6a235b42-bcce-4364-b7bf-e2d8b9897b57" />
