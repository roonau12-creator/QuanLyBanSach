pipeline {
    agent any

    stages {

        stage('Checkout') {
            steps {
                echo 'Checkout source code from GitHub'
                checkout scm
            }
        }

        stage('Check Workspace') {
            steps {
                sh '''
                    echo "===== CURRENT DIRECTORY ====="
                    pwd

                    echo "===== ROOT FILES ====="
                    ls -la

                    echo "===== ALL PROJECT FILES ====="
                    find . -name "*.csproj" -print
                '''
            }
        }

        stage('Restore') {
            steps {
                echo 'Restoring dependencies...'
                sh 'dotnet restore BookShopmvc/BookShopmvc.csproj'
            }
        }

        stage('Build') {
            steps {
                echo 'Building BookShop...'
                sh 'dotnet build BookShopmvc/BookShopmvc.csproj --no-restore'
            }
        }

        stage('Test') {
            steps {
                echo 'Running tests...'
                sh 'dotnet test BookShop.Tests/BookShop.Tests.csproj --no-restore'
            }
        }
    }

    post {
        success {
            echo 'CI Pipeline completed successfully!'
        }

        failure {
            echo 'CI Pipeline failed!'
        }
    }
}