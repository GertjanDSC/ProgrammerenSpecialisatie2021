# .NET 6 on WSL2

Zoek de laatste versie op met behulp van google.

Medio august 2021 wordt dit:

```sh
wget https://download.visualstudio.microsoft.com/download/pr/ce3fd989-b69d-439a-9cac-09ad40597db8/2848d49480b6e7b1b2a18cfa46d724e2/dotnet-sdk-6.0.100-preview.7.21379.14-linux-x64.tar.gz
```

```sh
mkdir -p $HOME/dotnet && tar zxf dotnet-sdk-6.0.100-preview.7.21379.14-linux-x64.tar.gz -C $HOME/dotnet
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet
```

Bovenstaande export statements maken dotnet enkel toegankelijk in de huidige sessie. Wens je dotnet permanent beschikbaar te hebben na het opstarten van een nieuwe shell, pas dan bij voorkeur .bashrc voor je gebruiker aan (alternatief: ~/.bash_profile, ~/.bashrc).

```sh
dotnet new console -o myApp
cd myApp
dotnet run
```

```sh
lsb_release -a
```

