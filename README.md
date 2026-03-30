## Get started

Para testar rode a solução e execute no Postman ou similar o curl abaixo:

```
curl --location '{api_url}/api/v1/conversor' \
--header 'Content-Type: application/json' \
--data '{
"dadosDeEntrada": "glob é I\nprok é V\npish é X\ntegj é L\nglob glob Prata é 34 Créditos\nglob prok Ouro é 57800 Créditos\npish pish Ferro é 3910 Créditos\nquanto é pish tegj glob glob ?\nquantos Créditos é glob prok Prata ?\nquantos Créditos é glob prok Ouro ?\nquantos Créditos é glob prok Ferro ?\nquanto de madeira uma marmota poderia roer se uma marmota pudesse roer madeira ?"
}'
```
