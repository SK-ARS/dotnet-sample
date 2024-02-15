pipeline {
    agent any
    stages {
        
        stage('Initialization') {
            steps {
                script {
                    echo "BRANCH_NAME= "+BRANCH_NAME
                    def GIT_CHECKOUT_BRANCH=BRANCH_NAME
                    def MAIL_BODY="Mail Body"
                    echo "currentBuild.rawBuild.causes[0] = "+currentBuild.rawBuild.causes[0]
                    echo "currentBuild.rawBuild.causes[0].class.simpleName = "+currentBuild.rawBuild.causes[0].class.simpleName
                    /*if("GitHubPRCause".equals(currentBuild.rawBuild.causes[0].class.simpleName)){
                        echo "Build triggered by PR"
                    }
                    else{
                        echo "****************Build not triggered by PR*********************"
                    }*/
                    if(env.PR_NUMBER!+=null && env.PR_NUMBER!='') {
                        echo "PR_NUMBER = "+PR_NUMBER
                    if (env.CHANGE_ID!=null && env.CHANGE_ID!='') {
                        echo "Pull Request Information:"
                        echo "PR Number: ${env.CHANGE_ID}"
                        echo "PR Title: ${env.CHANGE_TITLE}"
                        echo "PR Author: ${env.CHANGE_AUTHOR}"
                        echo "PR Source Branch: ${env.CHANGE_BRANCH}"
                        echo "PR Target Branch: ${env.CHANGE_TARGET}"
                    } else {
                        echo "This build is not triggered by a pull request."
                    }
                    echo "Inside stage 1"
                    echo "GIT_CHECKOUT_BRANCH = "+GIT_CHECKOUT_BRANCH
                    //MAIL_BODY+="\n\nJob triggered by Pull Reruest : ${PR_NUMBER} - Inittiated by ${PR_AUTHOR}\nPR source : ${PR_SOURCE_BRANCH}, target : ${PR_TARGET_BRANCH}"
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
