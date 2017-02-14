# WilliamEngD
http://stackoverflow.com/questions/1778088/how-to-clone-a-single-branch-in-git
# clone only the remote primary HEAD (default: origin/master)
git clone --single-branch

as in:
git clone <url> --branch <branch> --single-branch [<folder>]

example 

git clone https://github.com/CUERobotics/WilliamEngD.git -- branch Caisson_Control --single-branch MyDesiredDirectoryName
