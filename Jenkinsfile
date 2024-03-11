pipeline {
    agent any
    stages {
        stage('Stage 1') {
            when{
                branch env.BRANCH_NAME
            }
            steps {
                script {
                    echo "*************************************** DEV BRANCH ***************************************"
                    echo "In Stage 1"
                    // echo "Trigger cause: ${currentBuild.getBuildCauses().toString()}"
                    TRIGGER_CAUSE=currentBuild.getBuildCauses().toString()
                    echo "Trigger cause : ${TRIGGER_CAUSE}"
                    echo "\"shortDescription\":\"Push event to branch"
                    if(TRIGGER_CAUSE.contains("\"shortDescription\":\"Push event to branch")) {
                        echo "Build triggered due to push event to branch ${env.BRANCH_NAME}"
                    }
                    echo "Commit Title: ${env.CHANGE_TITLE}"
                    // echo "Author: ${env.CHANGE_AUTHOR}"
                    echo "Author : ${GIT_COMMITTER_NAME}"
                }
            }
        }
    
        stage('PR stage') {
            when{
                branch 'PR-*'
            }
            steps {
                script {
                    echo "*************************************** DEV BRANCH ***************************************"
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

        stage("Independent stage"){
            steps {
                script {
                    echo "Inside Independent stage"
                }
            }
        }
    }
}
