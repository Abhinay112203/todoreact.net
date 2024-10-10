node {
    stage('Clone Repository') {
        // Clone the latest code from the repository configured in Jenkins
        checkout scm
    }

    stage('Build Docker Image') {
        // Build a Docker image for the .NET application
        sh '''
            docker build -t ToDoAPI:${BUILD_ID} .
        '''
    }

    stage('Stop Existing Docker Container') {
        // Stop and remove any existing Docker container named 'dotnet-app'
        sh '''
            if [ "$(docker ps -q -f name=dotnet-app)" ]; then
                docker rm -f dotnet-app
            fi
        '''
    }

    stage('Deploy Docker Container') {
        // Run a new Docker container with the built image and expose it on port 5236
        sh '''
            docker run -d --name dotnet-app -p 5236:5236 ToDoAPI:${BUILD_ID}
        '''
    }

    stage('Clean Up Old Docker Images') {
        // Remove old Docker images to save space
        sh '''
            docker image prune -f
        '''
    }
}
