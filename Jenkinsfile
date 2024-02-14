pipeline {
    agent any
    stages {
        
        stage('Initialization') {
            steps {
                script {
                    echo "Inside stage 1"
                    def GIT_CHECKOUT_BRANCH=BRANCH_NAME 
                    def MAIL_BODY="Mail Body"
                    MAIL_BODY+="\n\nJob triggered by Pull Reruest : ${GITHUB_PR_NUMBER} - Inittiated by ${GITHUB_PR_AUTHOR_EMAIL}\nPR source : ${GITHUB_PR_SOURCE_BRANCH}, target : ${GITHUB_PR_TARGET_BRANCH}"
                    echo "MAIL_BODY : "+MAIL_BODY+"\n\n*******************************************************************************************"
                }
            }
        }
        
        stage('PR stage') {
            when{
                branch 'PR-*'
            }
            steps {
                script {
                    echo "PR beanch inside steps and script"
                }
            }
        }
    }
}
