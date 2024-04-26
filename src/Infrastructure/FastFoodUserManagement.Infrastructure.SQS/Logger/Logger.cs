using Amazon.SQS;
using Amazon.SQS.Model;
using FastFoodUserManagement.Domain.Contracts.Logger;

namespace FastFoodUserManagement.Infrastructure.SQS.Logger;

public class Logger(AmazonSQSClient sqsClient) : ILogger
{
    public async Task Log(string stackTrace, string message, string exception)
    {
        Dictionary<string, MessageAttributeValue> messageAttributes = new Dictionary<string, MessageAttributeValue>
        {
            { "Service",   new MessageAttributeValue { DataType = "String", StringValue = "FastFoodUserManagement" } },
            { "StackTrace",   new MessageAttributeValue { DataType = "String", StringValue = stackTrace } },
            { "ExceptionMessage",  new MessageAttributeValue { DataType = "String", StringValue = message } },
            { "Ex", new MessageAttributeValue { DataType = "String", StringValue = exception } },
            { "Time", new MessageAttributeValue { DataType = "String", StringValue = DateTime.Now.ToString() } }
        };

        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = Environment.GetEnvironmentVariable("AWS_SQS"),
            MessageBody = message,
            MessageGroupId = Environment.GetEnvironmentVariable("AWS_SQS_GROUP_ID"),
            MessageAttributes = messageAttributes,
            MessageDeduplicationId = Guid.NewGuid().ToString()
        };

        var sendMessageResponse = await sqsClient.SendMessageAsync(sendMessageRequest);
    }
}
