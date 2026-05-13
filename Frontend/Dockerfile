# ── Stage 1: Build ──
FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./
RUN npm install --legacy-peer-deps

COPY . .
RUN npm run build --configuration=production

# ── Stage 2: Serve with Nginx ──
FROM nginx:alpine

# Copy built files from Stage 1
COPY --from=build /app/dist/vibely-frontend/browser /usr/share/nginx/html

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]