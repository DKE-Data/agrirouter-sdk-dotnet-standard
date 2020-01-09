using com.dke.data.agrirouter.api.env;
using Serilog;

namespace com.dke.data.agrirouter.api.test.service
{
    public class AbstractSecuredIntegrationTest
    {

        protected string PrivateKey => "-----BEGIN PRIVATE KEY-----MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoI" +
                                       "BAQC5iAveKHkO+vl+NuYafRgmWuRwhmXrM6abmDYeXUK5VSgDWlQfbL89NZCn06kisHSV9zik8" +
                                       "PdCDS91TdE4I7KgsSCTsxFkAF6U2HXHMXD0TXUpDU3I50Rp4xLGdGofud5SvyFvS9rWQYyvI+J" +
                                       "ezRKaCDcOkvObM247ir3FmCRdZJi2UfOxOucdqCOr6+h7cnoqHTV13Xrf79HUL2A5Xayw5dpHf" +
                                       "cK0lxhpuYIH9lJdO045PQiqqz0UOfi7XKmCoR1k7z30o83jsSox78Gz/b4t8Y5FgDU51123U/3" +
                                       "4AlBh0HRCPz41r4lflRjmr9eEWdSN8YLOu+ZNitCbC2kgSGeZAgMBAAECggEAAU1/n6JxkV28O" +
                                       "bFtcNjS96lgxc4ylUHhg1TTzXQ6oy+oqYrDwQA75zq/u1pYvpv8h5VXr7Q0i9sCmJLjyEToGYM" +
                                       "afAfQYSv0MUMEI0v/mgS/Hbsm9VhzxVmqzWhIGshGWiOjx3x1F6PLo6riqVp/ugOBTam4JCCRD" +
                                       "BuiAz4/6oSmtNCv6PT72tVeOmSTGPcE59l6TnK9QooPxd/fSltbqNuhCQ1kMnFUsb2+2kBTb2t" +
                                       "0H6Euqkdy0hJjQMVUi1a2erYoCGKCcPti2omp+yQa4hyi5suUEiqRCFLl5wLBZwQR4oUL+pulx" +
                                       "WoGnozqJnXj7anwEbdQT6G8Ti/zAkMNgQKBgQDy+Smn8R6hBbUCbM0IGP9pi1FkmD/UDZAar/Z" +
                                       "oYmKkvDip1C3fF/Sh428Z3WEAZUvUXSdqmk5e+QzQl3g3e1UegsKguvUy3O/ht+hzQbRRZz99qz" +
                                       "cFO1SF8KWyhlM+JjGB40cY0bCB5zSK5+hCemM1tf87llNcU3IY5AW5V1uFgQKBgQDDeny+YaN2" +
                                       "6f/rJznAap4yAJ5crgafctt80fJrcFB8LIlPnTqjb7mSLMIZuniRA8bz2ptilomC/DL/hlrKv9" +
                                       "lJ0Z460agGzWJXxpvUrmdlgcWR6c7DLtHW7t8DTLZy6+5U8OZRK2ec+NhBA91nL1pGlScDDmqc" +
                                       "K7imZqWO9NNeGQKBgQCEG0cz4dzmbgTx8Zg1C+prR22UxVcHA+zTJdNTBBNgQFqKtCvQveSr1M" +
                                       "4GgSCJp4noSFHzGzz7VGxMSd76Q5uPQEd0PtKsugXPcz/20sWo8PtLZ7k9pfmZ7bMZ8wD1rKyE" +
                                       "U7/HVdOjfcNKtzbIVmT0wiSpEF34uAI0WF5oXIANgQKBgGEgVCj4NJNaMzlxFQXhM9cebZEZOts" +
                                       "w45PCcVQmyybXriYrtj4MvkS+DndgKpXLLahuXzR+HbdCfkhuRmBlMJ64E4mgMH+ovwtj+HuVa" +
                                       "HSMJVGZvY1HjyXfLFnkXOb/CT2VMKr2CRZ6omCzfefOJGnbpDWljR0psCal6+77AKbJAoGBAIO" +
                                       "n21mxzv2gV76Zt4+PPYTE1iKVp6iu/hvtFs+2aKrd1aba6lPl/lkCgCWqJUiGyLDKyeAcNiC1r9" +
                                       "CAT7s3Dd7yM83gxpIYWryU8ZbbUIW8EgiC3E+0ZUSWZdUuvwFLoUqrohxuSDKOua6ccneFTAY/" +
                                       "VXw22RPXlLb3izbClFQI-----END PRIVATE KEY-----";
        
        protected string PublicKey => "-----BEGIN PUBLIC KEY-----\n" +
                                      "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAuYgL3ih5Dvr5fjbmGn0Y\n" +
                                      "JlrkcIZl6zOmm5g2Hl1CuVUoA1pUH2y/PTWQp9OpIrB0lfc4pPD3Qg0vdU3ROCOy\n" +
                                      "oLEgk7MRZABelNh1xzFw9E11KQ1NyOdEaeMSxnRqH7neUr8hb0va1kGMryPiXs0S\n" +
                                      "mgg3DpLzmzNuO4q9xZgkXWSYtlHzsTrnHagjq+voe3J6Kh01dd163+/R1C9gOV2s\n" +
                                      "sOXaR33CtJcYabmCB/ZSXTtOOT0Iqqs9FDn4u1ypgqEdZO899KPN47EqMe/Bs/2+\n" +
                                      "LfGORYA1Odddt1P9+AJQYdB0Qj8+Na+JX5UY5q/XhFnUjfGCzrvmTYrQmwtpIEhn\n" +
                                      "mQIDAQAB\n" +
                                      "-----END PUBLIC KEY-----\n";
        
        protected static string ApplicationId => "39d18ae2-04e3-42de-8a42-935565a6b261";
        
        protected static string CertificationVersionId => "719afec8-d2ff-4cf8-8194-e688ae56b3b5";

        protected Environment Environment => new QA();

        protected AbstractSecuredIntegrationTest()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
        }

    }
}