
resource "azurerm_service_plan" "service_plan" {
  name                = var.project_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "az_func" {
  name                       = var.function_app_name
  location                   = azurerm_resource_group.resource_group.location
  resource_group_name        = azurerm_resource_group.resource_group.name
  service_plan_id            = azurerm_service_plan.service_plan.id
  storage_account_name       = azurerm_storage_account.storage_account.name
  storage_account_access_key = azurerm_storage_account.storage_account.primary_access_key
  https_only                 = true

  site_config {
    cors {
      allowed_origins     = ["*"]
    }
  }

  app_settings = {
    "COSMOSDB_CONNECTION_STRING" = "mongodb://${azurerm_cosmosdb_account.cosmosdb.name}:${azurerm_cosmosdb_account.cosmosdb.primary_key}@${azurerm_cosmosdb_account.cosmosdb.name}.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@${azurerm_cosmosdb_account.cosmosdb.name}@"
  }
}
