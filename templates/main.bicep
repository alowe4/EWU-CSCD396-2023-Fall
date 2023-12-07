resource appServicePlan 'Microsoft.Web/serverFarms@2022-03-01' = {
  name: 'TriviaPlan'
  location: 'East US'
  sku: {
    name: 'F1'
  }
}

resource appServiceApp 'Microsoft.Web/sites@2022-03-01' = {
  name: 'FinalNewRegion'
  location: 'East US'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
  }
}
