using com.dke.data.agrirouter.api.env;

namespace com.dke.data.agrirouter.api.test.service
{
    public class AbstractIntegrationTest
    {
        protected static string ApplicationId => "39d18ae2-04e3-42de-8a42-935565a6b261";
        protected static string CertificationVersionId => "719afec8-d2ff-4cf8-8194-e688ae56b3b5";
        
        protected Environment Environment => new QA();

    }
}