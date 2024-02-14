pipeline {
    agent any
    stages {
        
        stage('Initialization') {
            steps {
                script {
                    echo "Inside stage 1"
                }
            }
        }
        
        stage('PR stage') {
            when{
                branch "PR-*"
            }
            steps {
                script {
                    echo "PR beanch inside steps and script"
                }
            }
        }
    }
}
