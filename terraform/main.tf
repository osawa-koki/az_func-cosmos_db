
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "2.0.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "general"
    storage_account_name = "terraform4osawakoki"
    container_name       = "azfunc-cosmosdb"
    key                  = "azfunc-cosmosdb.tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "resource_group" {
  name = var.project_name
  location = "japaneast"

  tags = {
    environment = "dev"
  }
}
