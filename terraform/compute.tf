
resource "azurerm_function_app" "az_func" {
  name                      = var.function_app_name
  location                  = var.location
  resource_group_name       = azurerm_resource_group.resource_group.name
  app_service_plan_id       = azurerm_app_service_plan.app_service_plan.id
  storage_connection_string = azurerm_storage_account.storage_account.primary_blob_connection_string
  version = "~4"
  https_only = true
}
