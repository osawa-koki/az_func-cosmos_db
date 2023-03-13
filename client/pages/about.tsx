import React from "react";
import Layout from "../components/Layout";

export default function AboutPage() {
  return (
    <Layout>
      <div id="About">
        <h1>🐙 Az Func × Cosmos DB 🐙</h1>
        <div className="mt-3">
          <h2>🐢 Azure Functions</h2>
          `Azure Functions`はサーバレスアーキテクチャの一つで、<br />
          イベントドリブンなアプリケーションを構築するためのサービスです。<br />
          <br />
          FaaS(Function as a Service)とも呼ばれ、サーバーを用意する必要がなく、その分コストが安くなります。<br />
          また、管理が簡単なので、開発者が開発に集中できます。<br />
          <br />
          AWSではLambda、GCPではCloud Functionsというサービスがあります。
        </div>
        <div className="mt-3">
          <h2>🐬 Cosmos DB</h2>
          `Cosmos DB`は、Azure上で動くNoSQLデータベースです。<br />
          <br />
          SQLやMongoDB、Cassandraなどのデータベースをサポートしており、<br />
          それぞれのデータベースの特徴を活かしながら、データベースを選択することができます。<br />
          <br />
          また、Azure上で動くため、Azureのサービスと連携することができます。<br />
          例えば、Azure Functionsと連携することで、Azure FunctionsのトリガーとしてCosmos DBを使用することができます。
        </div>
      </div>
    </Layout>
  );
};
