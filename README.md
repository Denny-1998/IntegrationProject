# IntegrationProject

## Overview

This project is a microservices-based social networking platform inspired by Twitter. It enables users to share updates, follow other users, and interact through likes and comments. 
The system is designed to demonstrate the principles of microservices architecture, allowing each service to function independently while communicating seamlessly with others.

### Key Features
- User account creation and authentication
- Ability to post updates
- Follow and unfollow other users
- View updates from users you follow (unfinished)

## Microservices Architecture

The system is built using a microservices architecture, which includes the following key services:

- **User Service**: Manages user profiles, authentication, and user relationships (e.g., following/unfollowing).
- **Post Service**: Handles the creation and retrieval of posts.
- **Feed Service**: Aggregates posts from users that a given user follows, providing a personalized feed.

The services are communicating with each other mostly using the "fire and forget" pattern. They send a request to the other service but do not depend on it's outcome. This approach was chosen to keep the project simple. In a mission critical application, a messaging service like RabbitMQ could have been used to make the microservices more independent from each other. 

In the current state, there is no front end, so to make the application work, the front end had to send requests to different services depending on the user's action. 

To also scale the project in the X-axis, the duplication could be done in the docker-compose.yml file. In that case, a load balancer would be needed. 

### Diagram
![Leeres Diagramm](https://github.com/user-attachments/assets/dc189524-332d-466d-8327-682fa20d79d9)

### Benefits of Microservices
- **Scalability**: Each service can be scaled independently, allowing for efficient resource allocation.
- **Flexibility**: Different technologies can be used for different services, enabling teams to choose the best tools for each task.
- **Resilience**: If one service fails, others can continue to function, reducing the risk of total system downtime.
- **Development Speed**: Teams can work on different services concurrently, speeding up development.

## Considerations

While microservices offer many advantages, they also introduce complexity. Here are some considerations when working with a microservices architecture:

- **Service Communication**: Choosing the right communication method is crucial for efficient data exchange between services.
  In this case simple HTTP requests were used, but that comes with the drawback, that services need to be online at the same time and thus less fault tolerance. Another approach could have been a messaging service.
- **Data Management**: Each microservice should manage its own database, which can lead to data consistency challenges.
- **Deployment Complexity**: Managing multiple services increases deployment complexity. Tools like Docker and Kubernetes can help mitigate this.
- **Monitoring and Logging**: Implementing robust monitoring and logging solutions is essential for tracking service health and performance.


