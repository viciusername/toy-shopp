# Vagrant configuration for ToyPlanet deployment

Vagrant.configure("2") do |config|
  # Базовий образ
  config.vm.box = "ubuntu/jammy64"
  
  # VM конфігурація
  config.vm.provider "virtualbox" do |v|
    v.memory = 2048
    v.cpus = 2
  end

  # Мережева конфігурація
  config.vm.network "private_network", ip: "192.168.1.10"
  config.vm.network "forwarded_port", guest: 5000, host: 5000
  config.vm.network "forwarded_port", guest: 5555, host: 5555

  # Синхронізація папок
  config.vm.synced_folder ".", "/vagrant", type: "virtualbox"

  # Provisioning
  config.vm.provision "shell", path: "vagrant/provision.sh"

  # Windows VM
  config.vm.define "toyplanet_web" do |web|
    web.vm.box = "peru/windows-10-enterprise-eval"
    web.vm.provider "virtualbox" do |v|
      v.gui = false
      v.memory = 4096
      v.cpus = 4
    end
    web.vm.provision "shell", path: "vagrant/provision_windows.ps1", privileged: true
  end

  # Linux VM
  config.vm.define "toyplanet_linux" do |linux|
    linux.vm.box = "ubuntu/jammy64"
    linux.vm.provider "virtualbox" do |v|
      v.memory = 2048
      v.cpus = 2
    end
    linux.vm.provision "shell", path: "vagrant/provision_linux.sh"
  end

  # macOS VM (якщо доступно)
  config.vm.define "toyplanet_macos" do |macos|
    macos.vm.box = "ghcr.io/chefhassan/macos-13"
    macos.vm.provider "virtualbox" do |v|
      v.memory = 4096
      v.cpus = 4
    end
    macos.vm.provision "shell", path: "vagrant/provision_macos.sh"
  end

end