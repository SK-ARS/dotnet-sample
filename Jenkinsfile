pipeline {
    agent any
    stages {
        stage('Stage 1') {
            when{
                branch env.BRANCH_NAME
            }
            steps {
                script {
                    echo "*************************************** MAIN BRANCH ***************************************"
                    echo "In Stage 1"
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
                    echo "*************************************** MAIN BRANCH ***************************************"
                    echo "Build triggered by PR"
                    echo "Pull Request Information:"
                    echo "PR Number: ${env.CHANGE_ID}"
                    echo "PR Title: ${env.CHANGE_TITLE}"
                    echo "PR Author: ${env.CHANGE_AUTHOR}"
                    echo "PR Source Branch: ${env.CHANGE_BRANCH}"
                    echo "PR Target Branch: ${env.CHANGE_TARGET}"
                    MAIL_BODY+="\n\n\tJob triggered due to PR-${env.CHANGE_ID} from ${env.CHANGE_BRANCH} to ${env.CHANGE_TARGET}"
                    
                }
            }
        }
    }
}
