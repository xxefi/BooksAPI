FROM node:18 AS builder
WORKDIR /app
COPY package*.json ./

RUN npm ci
COPY . .

RUN npm run build

FROM node:18-slim
WORKDIR /app

COPY --from=builder /app/package*.json ./

RUN npm install --only=production

COPY --from=builder /app/.next .next
COPY --from=builder /app/public public

EXPOSE 3000
CMD ["npm", "start"]
