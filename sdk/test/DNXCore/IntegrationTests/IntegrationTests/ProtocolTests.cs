using Amazon.CloudSearch;
using Amazon.CloudSearch.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.ElasticTranscoder;
using Amazon.ElasticTranscoder.Model;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using Amazon.DNXCore.IntegrationTests;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.DNXCore.IntegrationTests
{
    /// <summary>
    /// Test class for testing various AWS protocols.
    /// </summary>
    
    public class ProtocolTests
    {
        //[Fact]
        public void TestJson()
        {
            UtilityMethods.RunAsSync(TestJson_Async);
        }

        private async Task TestJson_Async()
        {
            using (var client = UtilityMethods.CreateClient<AmazonSimpleWorkflowClient>())
            {
                var domainName = "dotnet-test-domain-" + DateTime.Now.ToFileTime();

                await client.RegisterDomainAsync(new RegisterDomainRequest
                {
                    Name = domainName,
                    Description = "Test domain",
                    WorkflowExecutionRetentionPeriodInDays = "5"
                });

                var domains = (await client.ListDomainsAsync(new ListDomainsRequest
                {
                    RegistrationStatus = RegistrationStatus.REGISTERED
                })).DomainInfos;

                Assert.NotNull(domains);
                Assert.NotNull(domains.Infos);
                Assert.NotEqual(0, domains.Infos.Count);

                await client.DeprecateDomainAsync(new DeprecateDomainRequest
                {
                    Name = domainName
                });

                var ure = await AssertExtensions.ExpectExceptionAsync<UnknownResourceException>(client.DeprecateDomainAsync(new DeprecateDomainRequest
                {
                    Name = "really-fake-domain-that-should-not-exist" + DateTime.Now.ToFileTime()
                }));
                Assert.NotNull(ure);
                Assert.NotNull(ure.Message);
                Assert.NotNull(ure.ErrorCode);
                Assert.Equal(ure.ErrorType, ErrorType.Unknown);
            }
        }

        [Fact]
        public void TestRestJson()
        {
            UtilityMethods.RunAsSync(TestRestJson_Async);
        }

        private async Task TestRestJson_Async()
        {
            using (var client = UtilityMethods.CreateClient<AmazonElasticTranscoderClient>())
            {
                var presets = (await client.ListPresetsAsync(new ListPresetsRequest())).Presets;
                Assert.NotNull(presets);
                Assert.NotEqual(0, presets.Count);

                var fakeId = "1111111111111-abcde1";
                var aete = await AssertExtensions.ExpectExceptionAsync<AmazonElasticTranscoderException>(client.DeletePipelineAsync(new DeletePipelineRequest
                {
                    Id = fakeId
                }));
                Assert.NotNull(aete);
                Assert.NotNull(aete.Message);
                Assert.True(aete.Message.IndexOf(fakeId, StringComparison.OrdinalIgnoreCase) >= 0);
                //Assert.NotNull(aete.ErrorCode);
                Assert.Equal(aete.ErrorType, ErrorType.Sender);
            }
        }

        [Fact]
        public void TestRestXml()
        {
            UtilityMethods.RunAsSync(TestRestXml_Async);
        }

        private async Task TestRestXml_Async()
        {
            using (var client = UtilityMethods.CreateClient<AmazonS3Client>())
            {
                var bucketName = await UtilityMethods.CreateBucketAsync(client, "TestRestXml_Async");

                var buckets = (await client.ListBucketsAsync(new ListBucketsRequest())).Buckets;
                Assert.NotNull(buckets);
                Assert.NotEqual(0, buckets.Count);
                Assert.Equal(1, buckets
                    .Count(b =>
                        string.Equals(bucketName, b.BucketName, StringComparison.OrdinalIgnoreCase)));

                var fakeBucketName = "really-fake-bucket-that-shout-not-exist" + DateTime.Now.ToFileTime();
                var as3e = await AssertExtensions.ExpectExceptionAsync<AmazonS3Exception>(client.DeleteBucketAsync(new DeleteBucketRequest
                {
                    BucketName = fakeBucketName
                }));
                Assert.NotNull(as3e);
                Assert.NotNull(as3e.Message);
                //Assert.NotNull(aete.ErrorCode);
                Assert.Equal(as3e.ErrorType, ErrorType.Sender);

                await UtilityMethods.DeleteBucketWithObjectsAsync(client, bucketName);
            }
        }

        [Fact]
        public void TestQuery()
        {
            UtilityMethods.RunAsSync(TestQuery_Async);
        }

        private async Task TestQuery_Async()
        {
            var digits = DateTime.Now.Ticks.ToString();
            var domainName = string.Format("net-sdk-test-{0}", digits.Substring(digits.Length - 15));

            using (var client = UtilityMethods.CreateClient<AmazonCloudSearchClient>())
            {
                await client.CreateDomainAsync(new CreateDomainRequest
                {
                    DomainName = domainName
                });
                var domains = (await client.ListDomainNamesAsync(new ListDomainNamesRequest())).DomainNames;
                Assert.NotNull(domains);
                Assert.NotEqual(0, domains.Count);

                await client.DeleteDomainAsync(new DeleteDomainRequest
                {
                    DomainName = domainName
                });

                var fakeDomain = new string('a', 30); // service defines valid domains as 28 characters long
                await AssertExtensions.ExpectExceptionAsync(client.DeleteDomainAsync(new DeleteDomainRequest
                {
                    DomainName = fakeDomain
                }));
            }
        }
    }
}