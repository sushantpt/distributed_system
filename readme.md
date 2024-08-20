## Running the Project

To run the project, follow these steps:

### 1. Dockerize RabbitMQ

1.1. Navigate to the RabbitMQ Docker configuration directory.
1.2. Start RabbitMq
```bash
docker-compose up -d
```
1.3. Navigate to http://localhost:15672/ (Username: suser Password: suser)

### 2. Run an Consumer (to create a queue in RabbitMQ)

### 3. Run payment.API
3.1. SeedAccount

3.2. Make payment

3.3. Check queue or and consumers' log

![rabbitmq_demo drawio](https://github.com/user-attachments/assets/1da1eead-f0ec-4243-bfc2-414a8d61b5df)
