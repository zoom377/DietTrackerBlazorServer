ssh -i "C:\Users\Dell G3\Documents\Ubuntu.pem" azureuser@20.108.4.196 sudo rm -r ~/diet_tracker
scp -i "C:\Users\Dell G3\Documents\Ubuntu.pem" -r %cd%\bin\Debug\net7.0\publish azureuser@20.108.4.196:~/diet_tracker
ssh -i "C:\Users\Dell G3\Documents\Ubuntu.pem" azureuser@20.108.4.196 sudo rm -r /var/www/diet_tracker
ssh -i "C:\Users\Dell G3\Documents\Ubuntu.pem" azureuser@20.108.4.196 sudo cp -r ~/diet_tracker /var/www/diet_tracker
pause
