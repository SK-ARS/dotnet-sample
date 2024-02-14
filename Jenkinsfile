pipeline {
    agent any
    stages {
        
        stage('Initialization') {
            steps {
                script {
                    echo "Build cause : "+currentBuild.rawBuild.getCause(0)
                    echo "Inside stage 1"
                    def GIT_CHECKOUT_BRANCH=BRANCH_NAME 
                    echo "GIT_CHECKOUT_BRANCH = "+GIT_CHECKOUT_BRANCH
                    def MAIL_BODY="Mail Body"
                    MAIL_BODY+="\n\nJob triggered by Pull Reruest : ${PR_NUMBER} - Inittiated by ${PR_AUTHOR}\nPR source : ${PR_SOURCE_BRANCH}, target : ${PR_TARGET_BRANCH}"
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
