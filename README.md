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


# Assignment 2


# Week 44 - API Gateway

## Overview
Implemented an Ocelot API Gateway to centralize and manage requests between microservices in the Twitter-like application.

## Components Implemented

### 1. Ocelot Gateway Service
- Created a new microservice for API Gateway using Ocelot
- Configured routing for all microservices through a single entry point
- Handles request forwarding to appropriate services

### 2. Service Routes
Implemented routes for multiple services:

#### Post Service Routes
- GET `/gateway/post` → `/api/post`
- POST `/gateway/post` → `/api/post`
- GET `/gateway/post/user/{id}` → `/api/post/user/{id}`

#### Feed Service Routes
- GET `/gateway/feed/{id}` → `/api/feed/{id}`

#### User Service Routes
- GET `/gateway/users` → `/api/user/all`
- GET `/gateway/user/{id}` → `/api/user/{id}`
- POST `/gateway/user` → `/api/user`
- POST `/gateway/user/{userId}/follow/{followUserId}` → `/api/user/{userId}/follow/{followUserId}`
- POST `/gateway/user/{userId}/unfollow/{unfollowUserId}` → `/api/user/{userId}/unfollow/{unfollowUserId}`
- GET `/gateway/user/posts` → `/api/user/posts`
- GET `/gateway/user/UserPosts` → `/api/user/UserPosts`

### 3. Configuration
- Set up HTTP routing
- Configured downstream services with proper hosts and ports
- Implemented consistent route patterns with `/gateway` prefix

## Benefits
- Single entry point for all API requests
- Simplified client-side integration
- Centralized request handling
- Better service isolation
- Consistent routing patterns


# Week 45 - Reliability Improvements

## Overview
Implemented several reliability measures in the PostService to make it more resilient to failures and provide better operational visibility.

## Key Improvements

### 1. Structured Logging (Serilog)
- Implemented comprehensive logging throughout the service
- Provides better visibility into service operations
- Helps with debugging and monitoring service health
- Logs are formatted in a structured way for better analysis

### 2. Global Exception Handling
- Implemented centralized error handling middleware
- Catches all unhandled exceptions in the service
- Returns user-friendly JSON error responses
- Prevents sensitive error details from leaking to clients
- Ensures consistent error response format

### 3. Database Resilience (Polly)
- Implemented retry policies for database operations
- Uses exponential backoff strategy:
  - First retry: 2 seconds wait
  - Second retry: 4 seconds wait
  - Third retry: 8 seconds wait
- Automatically recovers from temporary database failures
- Logs retry attempts for monitoring and debugging

## Benefits
These improvements make the service more reliable by:
- Automatically recovering from temporary failures
- Providing better error handling and reporting
- Giving better insights into operational issues
- Protecting against database connectivity problems
- Ensuring consistent error responses for better client integration


# Week 46 - Kubernetes

## Overview
I implemented a basic Kubernetes deployment for the microservices architecture. The implementation focused on deploying the services to a local Kubernetes cluster using Minikube.

## Components Implemented
- Local Kubernetes cluster using Minikube
- Kubernetes deployments and services for FeedService, UserService and PostService
- Conversion of Docker Compose configurations to Kubernetes manifests

## Implementation Steps

### 1. Local Kubernetes Setup
- Installed required tools:
  - Minikube for local Kubernetes cluster
  - kubectl for Kubernetes CLI interaction
  - kompose for converting Docker Compose to Kubernetes manifests

### 2. Service Migration to Kubernetes
- Used kompose to convert Docker Compose configurations to Kubernetes manifests
- Generated deployment and service configurations for each microservice
- Modified generated manifests to work with local images

### 3. Service Deployment
Successfully deployed:
- FeedService 
- UserService 

## Challenges and Solutions

### 1. Image Pulling Issues
- **Challenge**: Kubernetes couldn't find local Docker images
- **Solution**: Added `imagePullPolicy: Never` to force usage of local images
- **Implementation**: Modified deployment manifests to use local image registry

### 2. HTTPS Configuration
- **Challenge**: PostService failing due to HTTPS certificate requirements
- **Solution**: No solution yet

### 3. Container Naming
- **Challenge**: Invalid container name formats in generated manifests
- **Solution**: Separated container name from image specification
- **Implementation**: Updated deployment manifests with correct naming conventions

## Benefits Achieved
- Service orchestration through Kubernetes
- Improved scalability potential
- Better service management
- Foundation for future cloud deployment

## Current State
- Successfully running multiple services in Kubernetes
- Groundwork laid for adding more services to the cluster

## Future Improvements
- Implement proper HTTPS with certificate management
- Add resource limits and requests
- Implement proper health checks
- Implement proper load balancing

# Week 47 - Security

## Overview
- 

# Week 48 - Design Patterns

## Overview

# New Architecture

![Leeres Diagramm (1)](https://github.com/user-attachments/assets/18d71642-f2b5-4600-abff-a4bac072c1e9)
