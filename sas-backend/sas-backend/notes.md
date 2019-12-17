# AWS serverless course notes
## SAM deploy
https://docs.aws.amazon.com/en_pv/serverless-application-model/latest/developerguide/serverless-sam-template-publishing-applications.html

Create an S3 bucket, give principal `serverlessrepo.amazonaws.com` GetObject access

```
 Set the profile
 
 $Env:AWS_PROFILE="tones"
```

```
sam package --template-file .\template.yaml --output-template-file .\template.packaged.yaml --s3-bucket sas-depl-artifacts --profile tones
```


 ```
sam deploy --template-file .\template.packaged.yaml --stack-name sas-backend --profile tones --capabilities CAPABILITY_IAM
```

```
aws cloudformation describe-stacks --stack-name sas-backend
```

```
dynamodb local (docker)

--docker run -p 8000:8000 amazon/dynamodb-local #don't use this one, it doesn't use -sharedDb by default!
docker run -p 8000:8000 amazon/dynamodb-local -jar DynamoDBLocal.jar -inMemory -sharedDb

--find the container id of the amazon/dynamodb-local image
docker ps -a

--re-start the dynamodb container
docker start {dynamodb container id}

--don't forget all your data will disappear when you stop the container

--list the tables
$Env:AWS_PROFILE="tones"; aws dynamodb list-tables --endpoint-url http://localhost:8000
```

```
aws dynamodb create-table `
  --endpoint-url http://localhost:8000 `
  --cli-input-json file://create-loans-table.json
  
aws dynamodb create-table `
  --endpoint-url http://localhost:8000 `
  --cli-input-json file://create-items-table.json
```