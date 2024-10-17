node {
    stage('Clone Repository') {
        // Pull the latest code from the repository configured in Jenkins
        checkout scm
    }

    stage('Build Docker Image') {

        // Build the Docker image using the Dockerfile in the 'docker' subfolder
        sh '''
            docker build -t my-dotnet-app:${BUILD_ID} -f ToDoAPI/Dockerfile .
        '''
        // Explanation:
        // - `-t my-dotnet-app:${BUILD_ID}`: Tag the image as 'my-dotnet-app' with the current Jenkins build ID.
        // - `-f docker/Dockerfile`: Specify the path to the Dockerfile in the 'docker' subfolder.
        // - `.`: Build context set to the root of the repository.
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
            docker run -d --name dotnet-app -v /data/configs/react-ui-net/appsettings.json:/app/appsettings.json -p 5236:80 my-dotnet-app:${BUILD_ID}
        '''
    }

    stage('Clean Up Old Docker Images') {
        // Remove old Docker images to save space
        sh '''
            docker image prune -f
        '''
    }
}
