pipeline {
    agent any
    stages {
        stage('Stage 1') {
            steps {
                script {
                    bat 'Stage 1'
                    bat "set"
                }
            }
        }
    
        stage('PR stage') {
            when{
                branch 'PR-*'
            }
            steps {
                script {
                    echo "Build triggered by PR"
                    echo "Pull Request Information:"
                    echo "PR Number: ${env.CHANGE_ID}"
                    echo "PR Title: ${env.CHANGE_TITLE}"
                    echo "PR Author: ${env.CHANGE_AUTHOR}"
                    echo "PR Source Branch: ${env.CHANGE_BRANCH}"
                    echo "PR Target Branch: ${env.CHANGE_TARGET}"
                }
            }
        }
    }
}
