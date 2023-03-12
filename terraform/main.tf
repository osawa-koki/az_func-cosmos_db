
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "2.0.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "general"
    key                  = "azfunc-cosmosdb.tfstate"
  }
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
  client_id       = var.client_id
  client_secret   = var.client_secret
  tenant_id       = var.tenant_id
}

resource "azurerm_resource_group" "resource_group" {
  name     = var.project_name
  location = "japaneast"

  tags = {
    environment = "dev"
  }
}
