pipeline {
    agent any

    environment {
        // Define any environment variables if needed
        DOCKER_IMAGE_NAME = "ToDoAPI"
    }

    stages {
        stage('Clone Repository') {
            steps {
                // Clone the GitHub repository on the Linux server
                checkout scm
            }
        }
        stage('Build Docker Image') {
            steps {
                script {
                    // Build the Docker image directly on the Linux server
                    docker.build("${DOCKER_IMAGE_NAME}:${env.BUILD_ID}")
                }
            }
        }
        stage('Run Docker Container') {
            steps {
                script {
                    // Stop and remove any existing container with the same name
                    sh '''
                        if [ "$(docker ps -q -f name=dotnet-app)" ]; then
                            docker rm -f dotnet-app
                        fi
                    '''
                    
                    // Run the Docker container with the newly built image
                    sh '''
                        docker run -d --name dotnet-app -p 5236:5236 ${DOCKER_IMAGE_NAME}:${env.BUILD_ID}
                    '''
                }
            }
        }
    }
}
