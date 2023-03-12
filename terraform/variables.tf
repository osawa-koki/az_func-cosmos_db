
variable "backend_storage_account_name" {
  type = string
  description = "The name of the storage account (backend)"
}

variable "backend_container_name" {
  type = string
  description = "The name of the container (backend)"
}

variable "project_name" {
  type = string
  description = "The name of the project"
}

variable "storage_account_name" {
  type = string
  description = "The name of the storage account"
}

variable "function_app_name" {
  type = string
  description = "The name of the function app"
}
