node {
    stage('Build .NET Application') {
        stage('Clone Repository') {
            // Pull the latest code from the GitHub repository
            checkout scm
        }
        stage('Stopping Existing Docker Container') {
            sh '''
                # Stop and remove the existing container if it exists
                if [ "$(docker ps -q -f name=dotnet-app)" ]; then
                    docker rm -f dotnet-app
                fi
            '''
        }
        stage('Install .NET SDK and Required Packages') {
            // Install .NET SDK if not already installed
            sh '''
                # Check if .NET SDK is installed
                if ! dotnet --version &> /dev/null; then
                    echo ".NET SDK not found. Installing .NET SDK..."
                    sudo apt update
                    sudo apt install -y apt-transport-https \
                                       dotnet-sdk-7.0
                else
                    echo ".NET SDK is already installed"
                fi
            '''
        }
        stage('Build the Application') {
            // Navigate to project directory and build the application
            sh '''
                # Clean and build the application
                dotnet clean
                dotnet build -c Release
            '''
        }
        stage('Publish Application') {
            // Publish the application to a folder
            sh '''
                dotnet publish -c Release -o ./out
            '''
        }
        stage('Clear Previous Deploy Files') {
            // Clear any previous files from deployment directory
            sh '''
                sudo rm -rf /usr/share/nginx/html/dotnet-app/*
            '''
        }
        stage('Move Published Files') {
            // Copy the newly published files to the deployment directory
            sh '''
                sudo cp -rf ./out/* /usr/share/nginx/html/dotnet-app/
            '''
        }
        stage('Restarting Application') {
            // Start a new Docker container to run the .NET application
            sh '''
                docker run -d --name dotnet-app -p 5236:5236 \
                           -v /usr/share/nginx/html/dotnet-app:/app \
                           mcr.microsoft.com/dotnet/aspnet:7.0 \
                           dotnet /app/YourApp.dll
            '''
        }
    }
}
