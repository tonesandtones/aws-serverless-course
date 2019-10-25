# AWS serverless course notes
## SAM deploy
https://docs.aws.amazon.com/en_pv/serverless-application-model/latest/developerguide/serverless-sam-template-publishing-applications.html

Create an S3 bucket, give principal `serverlessrepo.amazonaws.com` GetObject access

```
eg, given S3 bucket name "sas-depl-artifacts"

sam package --template-file .\template.yaml --output-template-file .\template.packaged.yaml --s3-bucket sas-depl-artifacts --profile tones
 ```
 
 ```
 sam deploy --template-file .\template.packaged.yaml --stack-name sas-backend --profile tones --capabilities CAPABILITY_IAM
 ```
 