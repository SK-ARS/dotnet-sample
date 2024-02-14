pipeline {
    agent any
    stages {
        
        stage('Initialization') {
            steps {
                script {
                    def GIT_CHECKOUT_BRANCH=BRANCH_NAME
                    def MAIL_BODY="Mail Body"
                    echo "currentBuild.rawBuild.causes[0] = "+currentBuild.rawBuild.causes[0]
                    echo "currentBuild.rawBuild.causes[0].class.simpleName = "+currentBuild.rawBuild.causes[0].class.simpleName
                    if("GitHubPRCause".equals(currentBuild.rawBuild.causes[0].class.simpleName)){
                        echo "Build triggered by PR"
                    }
                    else{
                        echo "****************Build not triggered by PR*********************"
                    }
                    echo "Inside stage 1"
                    echo "GIT_CHECKOUT_BRANCH = "+GIT_CHECKOUT_BRANCH
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
