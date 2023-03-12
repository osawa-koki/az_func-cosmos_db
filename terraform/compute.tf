
resource "azurerm_function_app" "az_func" {
  name = var.function_app_name
  location = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name

  app_service_plan_id = azurerm_app_service_plan.service_plan.id
  storage_account_name = azurerm_storage_account.storage_account.name
  storage_account_access_key = azurerm_storage_account.storage_account.primary_access_key
  version = "~4"
  https_only = true
}
