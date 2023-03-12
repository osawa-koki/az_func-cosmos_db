# az_func-cosmos_db

`Azure Function` + `Cosmos DB` + `C#`でWEBサービスを構築するサンプルです。  
`Terraform`でAzure上にリソースを構築し、`GitHub Actions`でAzure上のリソースをデプロイします。  

## 準備

以下のGitHubシークレットを設定します。  

| シークレット名 | 説明 |
| --- | --- |
| AZURE_STORAGE_ACCESS_KEY | Azure Storage Accountのアクセスキー |
| CONTAINER_NAME | Azure Storage Accountのコンテナ名 (バックエンドサービス) |
| STORAGRE_ACOUNT_NAME | Azure Storage Accountのアカウント名 (バックエンドサービス) |
| TFVARS | Terraformの変数ファイルの内容 |

### Azure CLIのインストール

Azure CLIをインストールします。  
[Install Azure CLI | Microsoft Docs](https://docs.microsoft.com/ja-jp/cli/azure/install-azure-cli)  

以下のコマンドでログインします。  

```shell
az login
```

ログインが完了したら、サービスプリンシパルを作成します。  
[参考](https://github.com/marketplace/actions/azure-cli-action#configure-azure-credentials-as-github-secret)  

```shell
az ad sp create-for-rbac --name "★プリンシパル名★" --role contributor  --scopes /subscriptions/★サブスクリプションID★ --sdk-auth
```

これで出力されたJSONの中から以下の値を取得します。  

| シークレット名 | 説明 |
| --- | --- |
| client_id | クライアントID |
| client_secret | クライアントシークレット |
| subscription_id | サブスクリプションID |
| tenant_id | テナントID |

これらを`terraform.tfvars`に記載します。  
