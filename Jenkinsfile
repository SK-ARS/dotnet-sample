pipeline {
    agent any
    Stages {
        stage('Initialization') {
            steps {
                script {
                    params.each { paramName, paramValue ->
                        echo "${paramName}: ${paramValue}"
                    }
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
                    if(env.PR_NUMBER!=null && env.PR_NUMBER!='') {
                        echo "PR_NUMBER = "+PR_NUMBER
                    }
                    else {
                        echo "PR_NUMBER variable is null"
                    }
                    
                    if (env.CHANGE_ID!=null && env.CHANGE_ID!='') {
                        echo "Pull Request Information:"
                        echo "PR Number: ${env.CHANGE_ID}"
                        echo "PR Title: ${env.CHANGE_TITLE}"
                        echo "PR Author: ${env.CHANGE_AUTHOR}"
                        echo "PR Source Branch: ${env.CHANGE_BRANCH}"
                        echo "PR Target Branch: ${env.CHANGE_TARGET}"
                    }
                    else {
                        echo "CHANGE_ID variable is null"
                    }
                    
                    if(env.GITHUB_PR_SOURCE_BRANCH!=null && env.GITHUB_PR_SOURCE_BRANCH!='') {
                        echo "GITHUB_PR_SOURCE_BRANCH = "+GITHUB_PR_SOURCE_BRANCH
                    }
                    else {
                        echo "GITHUB_PR_SOURCE_BRANCH variable is null"
                    }
                    bat 'set'
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
