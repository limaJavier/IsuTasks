# ===== 1. Build Stage =====
FROM node:22 AS build
WORKDIR /app

# Copy lock files first for better cache
COPY package.json pnpm-lock.yaml ./

# Install pnpm
RUN npm install -g pnpm

# Install dependencies
RUN pnpm install

# Copy the rest of your project
COPY . .

# Build Angular app
RUN pnpm build


# ===== 2. Nginx Runtime Stage =====
FROM nginx:1.29

# Remove the default nginx page
RUN rm -rf /usr/share/nginx/html/*

# Copy the built Angular app
COPY --from=build /app/dist/isutasks/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
